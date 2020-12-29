using NUnit.Framework;
using RayTracerLogic;
using System.Collections.Generic;

namespace RayTracerTests
{
    // ToDoBre16: Schatten optional machen (wie auf Seite 173/174 bzw. 189/190 beschrieben)

    [TestFixture()]
    public class ReflectionAndRefractionTests
    {
        [Test()]
        public void ReflectivityForTheDefaultMaterial()
        {
            // Given
            Material material = new Material();

            // Then
            Assert.IsTrue(material.Reflective.NearlyEquals(0));
        }

        [Test()]
        public void PrecomputingTheReflectionVector()
        {
            // Given
            Shape shape = new Plane();
            Ray ray = new Ray(new Point(0, 1, -1), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));
            Intersection hit = new Intersection(System.Math.Sqrt(2), shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            // Then
            Assert.IsTrue(preparedIntersection.ReflectionVector.NearlyEquals(new Vector(0, System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2)));
        }

        [Test()]
        public void TheReflectedColorForANonReflectiveMaterial()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            Shape shape = world.Shapes[1];
            shape.Material.Ambient = 1;

            Intersection hit = new Intersection(1, shape);
            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            Color color = world.GetReflectedColor(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheReflectedColorForAReflectiveMaterial()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Matrix.NewTranslationMatrix(0, -1, 0);
            world.Shapes.Add(shape);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));

            Intersection hit = new Intersection(System.Math.Sqrt(2), shape);
            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            Color color = world.GetReflectedColor(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.19032, 0.2379, 0.14274)));
        }

        [Test()]
        public void ShadeHitWithReflectiveMaterial()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Matrix.NewTranslationMatrix(0, -1, 0);

            world.Shapes.Add(shape);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));

            Intersection hit = new Intersection(System.Math.Sqrt(2), shape);
            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            Color color = world.ShadeHit(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.87677, 0.92436, 0.82918)));
        }

        [Test()]
        public void ColorAtWithMutuallyReflectiveSurfaces()
        {
            // Given
            World world = new World();

            PointLight pointLight = new PointLight(new Point(0, 0, 0), Color.GetWhite());
            world.LightSources.Add(pointLight);

            Shape lower = new Plane();
            lower.Material.Reflective = 1;
            lower.Transform = Matrix.NewTranslationMatrix(0, -1, 0);
            world.Shapes.Add(lower);

            Shape upper = new Plane();
            upper.Material.Reflective = 1;
            upper.Transform = Matrix.NewTranslationMatrix(0, 1, 0);
            world.Shapes.Add(upper);

            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));

            // Then
            Assert.IsInstanceOf<Color>(world.ColorAt(ray));
        }

        [Test()]
        public void TheReflectedColorAtMaximumRecursiveDepth()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Matrix.NewTranslationMatrix(0, -1, 0);

            world.Shapes.Add(shape);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));

            Intersection hit = new Intersection(System.Math.Sqrt(2), shape);
            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            Color color = world.GetReflectedColor(preparedIntersection, 0);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TransparencyAndRefractiveIndexForTheDefaultMaterial()
        {
            // Given
            Material material = new Material();

            // Then
            Assert.IsTrue(material.Transparency.NearlyEquals(0.0));
            Assert.IsTrue(material.RefractiveIndex.NearlyEquals(1.0));
        }

        [Test()]
        public void AHelperForProducingASphereWithAGlassyMaterial()
        {
            // Given
            Sphere sphere = Sphere.NewGlassSphere();

            // Then
            Assert.IsTrue(sphere.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
            Assert.IsTrue(sphere.Material.Transparency.NearlyEquals(1.0));
            Assert.IsTrue(sphere.Material.RefractiveIndex.NearlyEquals(1.5));
        }

        [Test()]
        public void FindingN1AndN2AtVariousIntersections()
        {
            // Given
            Sphere a = Sphere.NewGlassSphere();
            a.Transform = Matrix.NewScalingMatrix(2, 2, 2);
            a.Material.RefractiveIndex = 1.5;

            Sphere b = Sphere.NewGlassSphere();
            b.Transform = Matrix.NewTranslationMatrix(0, 0, -0.25);
            b.Material.RefractiveIndex = 2.0;

            Sphere c = Sphere.NewGlassSphere();
            c.Transform = Matrix.NewTranslationMatrix(0, 0, 0.25);
            c.Material.RefractiveIndex = 2.5;

            Ray ray = new Ray(new Point(0, 0, -4), new Vector(0, 0, 1));
            Intersections intersections = new Intersections(
                new Intersection(2, a),
                new Intersection(2.75, b),
                new Intersection(3.25, c),
                new Intersection(4.75, b),
                new Intersection(5.25, c),
                new Intersection(6, a)
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

                PreparedIntersection preparedIntersection = intersection.Prepare(ray, intersections);

                // Then
                Assert.IsTrue(preparedIntersection.N1.NearlyEquals(examples[index].Item1));
                Assert.IsTrue(preparedIntersection.N2.NearlyEquals(examples[index].Item2));
            }
        }

        [Test()]
        public void TheUnderPointIsOffsetBelowTheSurface()
        {
            // Given
            // ToDoBre16: GlassSphere in Tests verfrachten
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            Shape shape = Sphere.NewGlassSphere();
            shape.Transform = Matrix.NewTranslationMatrix(0, 0, 1);

            Intersection hit = new Intersection(5, shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            // Then
            Assert.IsTrue(preparedIntersection.UnderPoint.Z > Constants.Epsilon / 2);
            Assert.IsTrue(preparedIntersection.Point.Z < preparedIntersection.UnderPoint.Z);
        }

        [Test()]
        public void TheRefractedColorWithOpaqueSurface()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Shape shape = world.Shapes[0];
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Intersections intersections = new Intersections(
                new Intersection(4, shape),
                new Intersection(6, shape)
            );

            PreparedIntersection preparedIntersection = intersections[0].Prepare(ray, intersections);

            Color color = world.GetRefractedColor(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheRefractedColorAtMaximumRecursiveDepth()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = world.Shapes[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;

            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Intersections intersections = new Intersections(
                new Intersection(4, shape),
                new Intersection(6, shape)
            );

            PreparedIntersection preparedIntersection = intersections[0].Prepare(ray, intersections);

            Color color = world.GetRefractedColor(preparedIntersection, 0);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheRefractedColorUnderTotalInternalReflection()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape shape = world.Shapes[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;

            Ray ray = new Ray(new Point(0, 0, System.Math.Sqrt(2) / 2), new Vector(0, 1, 0));
            Intersections intersections = new Intersections(
                new Intersection(-System.Math.Sqrt(2) / 2, shape),
                new Intersection(System.Math.Sqrt(2) / 2, shape)
            );

            // ToDoBre16: Alle Kommentare aus dem Buch übernehmen, auch die When's
            PreparedIntersection preparedIntersection = intersections[1].Prepare(ray, intersections);

            Color color = world.GetRefractedColor(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheRefractedColorWithARefractedRay()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape a = world.Shapes[0];
            a.Material.Ambient = 1.0;
            a.Material.Pattern = new TestPattern();

            Shape b = world.Shapes[1];
            b.Material.Transparency = 1.0;
            b.Material.RefractiveIndex = 1.5;

            Ray ray = new Ray(new Point(0, 0, 0.1), new Vector(0, 1, 0));
            Intersections intersections = new Intersections(
                new Intersection(-0.9899, a),
                new Intersection(-0.4899, b),
                new Intersection(0.4899, b),
                new Intersection(0.9899, a)
            );

            PreparedIntersection preparedIntersection = intersections[2].Prepare(ray, intersections);

            Color color = world.GetRefractedColor(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0, 0.99888, 0.04725)));
        }

        [Test()]
        public void ShadeHitWithATransparentMaterial()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            Shape floor = new Plane();
            floor.Transform = Matrix.NewTranslationMatrix(0, -1, 0);
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            world.Shapes.Add(floor);

            Shape ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Matrix.NewTranslationMatrix(0, -3.5, -0.5);
            world.Shapes.Add(ball);

            Ray ray = new Ray(new Point(0, 0, -3), new Vector(0, -System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2));
            Intersections intersections = new Intersections(new Intersection(System.Math.Sqrt(2), floor));

            PreparedIntersection preparedIntersection = intersections[0].Prepare(ray, intersections);

            Color color = world.ShadeHit(preparedIntersection, 5);

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

            PreparedIntersection preparedIntersection = intersections[1].Prepare(ray, intersections);
            double reflectance = preparedIntersection.Schlick();

            // Then
            Assert.IsTrue(reflectance.NearlyEquals(1.0));
        }

        [Test()]
        public void TheSchlickApproximationWithAPerpendicularViewingAngle()
        {
            // Given
            Shape shape = Sphere.NewGlassSphere();
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));
            Intersections intersections = new Intersections(
                new Intersection(-1, shape),
                new Intersection(1, shape)
            );

            PreparedIntersection preparedIntersection = intersections[1].Prepare(ray, intersections);
            double reflectance = preparedIntersection.Schlick();

            // Then
            Assert.IsTrue(reflectance.NearlyEquals(0.04));
        }

        [Test()]
        public void TheSchlickApproximationWithSmallAngleAndN2GreaterThanN1()
        {
            // Given
            Shape shape = Sphere.NewGlassSphere();
            Ray ray = new Ray(new Point(0, 0.99, -2), new Vector(0, 0, 1));
            Intersections intersections = new Intersections(
                new Intersection(1.8589, shape)
            );

            PreparedIntersection preparedIntersection = intersections[0].Prepare(ray, intersections);
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

            Shape floor = new Plane();
            floor.Transform = Matrix.NewTranslationMatrix(0, -1, 0);
            floor.Material.Reflective = 0.5;
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            world.Shapes.Add(floor);

            Shape ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Matrix.NewTranslationMatrix(0, -3.5, -0.5);
            world.Shapes.Add(ball);

            Intersections intersections = new Intersections(new Intersection(System.Math.Sqrt(2), floor));
            PreparedIntersection preparedIntersection = intersections[0].Prepare(ray, intersections);

            Color color = world.ShadeHit(preparedIntersection, 5);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.93391, 0.69643, 0.69243)));
        }
    }
}
