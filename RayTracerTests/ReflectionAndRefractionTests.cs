using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class ReflectionAndRefractionTests
    {
        [Test()]
        public void ReflectivityForTheDefaultMaterial()
        {
            // Given
            Material material = new Material();

            // Then
            Assert.IsTrue(material.Reflective.NearlyEquals(0.0));
        }

        [Test()]
        public void PrecomputingTheReflectionVector()
        {
            // Given
            Shape shape = new Plane();
            Ray ray = new Ray(new Point(0, 1, -1), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));
            Intersection intersection = new Intersection(System.Math.Sqrt(2), shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));

            // Then
            Assert.IsTrue(preparedIntersection.ReflectVector.NearlyEquals(new Vector(0, System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2)));
        }

        [Test()]
        public void TheReflectedColorForANonReflectiveMaterial()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            Shape shape = defaultWorld.Shape[1];
            shape.Material.Ambient = 1;

            Intersection intersection = new Intersection(1, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color color = defaultWorld.GetReflectedColor(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheReflectedColorForAReflectiveMaterial()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();

            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = shape.Transform.Translate(0, -1, 0);
            defaultWorld.Shape.Add(shape);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));
            Intersection intersection = new Intersection(System.Math.Sqrt(2), shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color color = defaultWorld.GetReflectedColor(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.19032, 0.2379, 0.14274)));
        }

        [Test()]
        public void ShadeHitWithAReflectiveMaterial()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();

            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = shape.Transform.Translate(0, -1, 0);
            defaultWorld.Shape.Add(shape);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));
            Intersection intersection = new Intersection(System.Math.Sqrt(2), shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color color = defaultWorld.GetShadedHit(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.87677, 0.92436, 0.82918)));
        }

        [Test()]
        public void GetColorAtWithMutuallyReflectiveSurfaces()
        {
            // Given
            World world = new World();

            world.LightSources.Add(new PointLight(new Point(0, 0, 0), Color.GetWhite()));

            Shape lower = new Plane();
            lower.Material.Reflective = 1;
            lower.Transform = lower.Transform.Translate(0, -1, 0);
            world.Shape.Add(lower);

            Shape upper = new Plane();
            upper.Material.Reflective = 1;
            upper.Transform = upper.Transform.Translate(0, 1, 0);
            world.Shape.Add(upper);

            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));

            // Then
            // ToDo: Test for endless recursion. This seems undoable with C#, as a
            // StackOverflowException cannot be catched.
            Color color = world.GetColorAt(ray);
        }

        [Test()]
        public void TheReflectedColorAtTheMaximumRecursiveDepth()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();

            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = shape.Transform.Translate(0, -1, 0);
            defaultWorld.Shape.Add(shape);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));

            Intersection intersection = new Intersection(System.Math.Sqrt(2), shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color color = defaultWorld.GetReflectedColor(preparedIntersection, 0);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TransparencyAndRefractiveIndexForTheDefaultMaterial()
        {
            // Given
            Material material = new Material();

            // Then
            material.Transparency = 0.0;
            material.RefractiveIndex = 1.0;
        }

        [Test()]
        public void AHelperForProducingASphereWithAGlassyMaterial()
        {
            // Given
            Shape sphere = Sphere.NewGlassSphere();

            // Then
            Assert.IsTrue(sphere.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
            Assert.IsTrue(sphere.Material.Transparency.NearlyEquals(1.0));
            Assert.IsTrue(sphere.Material.RefractiveIndex.NearlyEquals(1.5));
        }

        [Test()]
        public void FindingN1AndN2AtVariousIntersections()
        {
            // Given
            Shape shapeA = Sphere.NewGlassSphere();
            shapeA.Transform = Matrix.NewScalingMatrix(2, 2, 2);
            shapeA.Material.RefractiveIndex = 1.5;

            Shape shapeB = Sphere.NewGlassSphere();
            shapeB.Transform = Matrix.NewTranslationMatrix(0, 0, -0.25);
            shapeB.Material.RefractiveIndex = 2.0;

            Shape shapeC = Sphere.NewGlassSphere();
            shapeC.Transform = Matrix.NewTranslationMatrix(0, 0, 0.25);
            shapeC.Material.RefractiveIndex = 2.5;

            Ray ray = new Ray(new Point(0, 0, -4), new Vector(0, 0, 1));

            Intersections intersections = new Intersections(
                new Intersection(2, shapeA),
                new Intersection(2.75, shapeB),
                new Intersection(3.25, shapeC),
                new Intersection(4.75, shapeB),
                new Intersection(5.25, shapeC),
                new Intersection(6, shapeA)
            );

            System.Tuple<double, double>[] examples =
            {
                new System.Tuple<double, double>(1.0, 1.5),
                new System.Tuple<double, double>(1.5, 2.0),
                new System.Tuple<double, double>(2.0, 2.5),
                new System.Tuple<double, double>(2.5, 2.5),
                new System.Tuple<double, double>(2.5, 1.5),
                new System.Tuple<double, double>(1.5, 1.0)
            };

            for (int index = 0; index < intersections.Count; index++)
            {
                Intersection intersection = intersections[index];

                // When
                PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, intersections);

                // Then
                Assert.IsTrue(preparedIntersection.N1.NearlyEquals(examples[index].Item1));
                Assert.IsTrue(preparedIntersection.N2.NearlyEquals(examples[index].Item2));
            }
        }

        [Test()]
        public void TheUnderpointIsOffsetBelowTheSurface()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            Shape shape = Sphere.NewGlassSphere();
            shape.Transform = Matrix.NewTranslationMatrix(0, 0, 1);

            Intersection intersection = new Intersection(5, shape);
            Intersections intersections = new Intersections(intersection);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, intersections);

            // Then
            Assert.IsTrue(preparedIntersection.UnderPoint.Z > Constants.Epsilon / 2);
            Assert.IsTrue(preparedIntersection.Point.Z < preparedIntersection.UnderPoint.Z);
        }

        [Test()]
        public void TheRefractedColorWithAnOpaqueSurface()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Shape shape = defaultWorld.Shape[0];
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            Intersections intersections = new Intersections(
                new Intersection(4, shape),
                new Intersection(6, shape)
            );

            // When
            PreparedIntersection preparedIntersection = intersections[0].GetPreparedIntersection(ray, intersections);
            Color color = defaultWorld.GetRefractedColor(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheRefractedColorAtTheMaximumRecursiveDepth()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = world.Shape[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;

            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Intersections intersections = new Intersections(
                new Intersection(4, shape),
                new Intersection(6, shape)
            );

            // When
            PreparedIntersection preparedIntersection = intersections[0].GetPreparedIntersection(ray, intersections);
            Color color = world.GetRefractedColor(preparedIntersection, 0);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheRefractedColorUnderTotalInternalReflection()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = world.Shape[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;

            Ray ray = new Ray(new Point(0, 0, System.Math.PI / 2), new Vector(0, 1, 0));
            Intersections intersections = new Intersections(
                new Intersection(-System.Math.PI / 2, shape),
                new Intersection(System.Math.PI / 2, shape)
            );

            // When
            // Note: This time you're inside the sphere, so you need
            // to look at the second intersection, intersections[1], not intersections[0]
            PreparedIntersection preparedIntersection = intersections[1].GetPreparedIntersection(ray, intersections);
            Color color = world.GetRefractedColor(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheRefractedColorWithARefractedRay()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shapeA = world.Shape[0];
            shapeA.Material.Ambient = 1.0;
            shapeA.Material.Pattern = new TestPattern();

            Shape shapeB = world.Shape[1];
            shapeB.Material.Transparency = 1.0;
            shapeB.Material.RefractiveIndex = 1.5;

            Ray ray = new Ray(new Point(0, 0, 0.1), new Vector(0, 1, 0));

            Intersections intersections = new Intersections(
                new Intersection(-0.9899, shapeA),
                new Intersection(-0.4899, shapeB),
                new Intersection(0.4899, shapeB),
                new Intersection(0.9899, shapeA)
            );

            // When
            PreparedIntersection preparedIntersection = intersections[2].GetPreparedIntersection(ray, intersections);
            Color color = world.GetRefractedColor(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0, 0.99888, 0.04725)));
        }

        [Test()]
        public void ShadeHitWithATransparentMaterial()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Plane floor = new Plane();
            floor.Transform = Matrix.NewTranslationMatrix(0, -1, 0);
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;

            world.Shape.Add(floor);

            Shape ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Matrix.NewTranslationMatrix(0, -3.5, -0.5);

            world.Shape.Add(ball);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));
            Intersections intersections = new Intersections(new Intersection(System.Math.Sqrt(2), floor));

            // When
            PreparedIntersection preparedIntersection = intersections[0].GetPreparedIntersection(ray, intersections);
            Color color = world.GetShadedHit(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.93642, 0.68642, 0.68642)));
        }

        [Test()]
        public void TheSchlickApproximationUnderTotalInternalReflection()
        {
            // Given
            Shape shape = Sphere.NewGlassSphere();
            Ray ray = new Ray(new Point(0, 0, System.Math.Sqrt(2) / 2), new Vector(0, 1, 0));

            Intersections intersections = new Intersections(
                new Intersection(-System.Math.Sqrt(2) / 2, shape),
                new Intersection(System.Math.Sqrt(2) / 2, shape)
            );

            // When
            PreparedIntersection preparedIntersection = intersections[1].GetPreparedIntersection(ray, intersections);
            double reflectance = preparedIntersection.Schlick();

            // Then
            Assert.IsTrue(reflectance.NearlyEquals(1.0));
        }

        [Test()]
        public void TheSchlickApproximationWithAPerpendicularviewingAngle()
        {
            // Given
            Shape shape = Sphere.NewGlassSphere();
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));
            Intersections intersections = new Intersections(
                new Intersection(-1, shape),
                new Intersection(1, shape)
            );

            // When
            PreparedIntersection preparedIntersection = intersections[1].GetPreparedIntersection(ray, intersections);
            double reflectance = preparedIntersection.Schlick();

            // Then
            Assert.IsTrue(reflectance.NearlyEquals(0.04));
        }

        [Test()]
        public void TheSchlickApproximationWithSmallAngleAndN1BiggerThenN2()
        {
            // Given
            Shape shape = Sphere.NewGlassSphere();
            Ray ray = new Ray(new Point(0, 0.99, -2), new Vector(0, 0, 1));
            Intersections intersections = new Intersections(new Intersection(1.8589, shape));

            // When
            PreparedIntersection preparedIntersection = intersections[0].GetPreparedIntersection(ray, intersections);
            double reflectance = preparedIntersection.Schlick();

            // Then
            Assert.IsTrue(reflectance.NearlyEquals(0.48873));
        }

        [Test()]
        public void ShadeHitWithAReflectiveTransparentMaterial()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));

            Plane floor = new Plane();
            floor.Transform = Matrix.NewTranslationMatrix(0, -1, 0);
            floor.Material.Reflective = 0.5;
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;

            world.Shape.Add(floor);

            Sphere ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Matrix.NewTranslationMatrix(0, -3.5, -0.5);

            world.Shape.Add(ball);

            Intersections intersections = new Intersections(new Intersection(System.Math.Sqrt(2), floor));

            // When
            PreparedIntersection preparedIntersection = intersections[0].GetPreparedIntersection(ray, intersections);
            Color color = world.GetShadedHit(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.93391, 0.69643, 0.69243)));
        }
    }
}
