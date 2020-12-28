using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class MakingASceneTests
    {
        [Test()]
        public void CreatingAWorld()
        {
            // Given
            World world = new World();

            // Then
            Assert.AreEqual(0, world.SceneObjects.Count);
            Assert.AreEqual(0, world.LightSources.Count);
        }

        [Test()]
        public void TheDefaultWorld()
        {
            // Given
            PointLight light = new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1));

            Sphere sphereOne = new Sphere();
            sphereOne.Material.Color = new Color(0.8, 1.0, 0.6);
            sphereOne.Material.Diffuse = 0.7;
            sphereOne.Material.Specular = 0.2;

            Sphere sphereTwo = new Sphere();
            sphereTwo.Transform = sphereTwo.Transform.Scale(0.5, 0.5, 0.5);

            // When
            World defaultWorld = DefaultWorld.NewDefaultWorld();

            // Then
            Assert.IsTrue(defaultWorld.LightSources[0].NearlyEquals(light));
            Assert.IsTrue(defaultWorld.SceneObjects[0].NearlyEquals(sphereOne));
            Assert.IsTrue(defaultWorld.SceneObjects[1].NearlyEquals(sphereTwo));
        }

        [Test()]
        public void IntersectAWorldWithARay()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            // When
            Intersections intersections = defaultWorld.GetIntersections(ray);

            // Then
            Assert.AreEqual(4, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(4));
            Assert.IsTrue(intersections[1].Distance.NearlyEquals(4.5));
            Assert.IsTrue(intersections[2].Distance.NearlyEquals(5.5));
            Assert.IsTrue(intersections[3].Distance.NearlyEquals(6));
        }

        [Test()]
        public void PrecomputingTheStateOfAnIntersection()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere shape = new Sphere();
            Intersection intersection = new Intersection(4, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));

            // Then
            Assert.IsTrue(preparedIntersection.Distance.NearlyEquals(intersection.Distance));
            Assert.IsTrue(preparedIntersection.SceneObject.NearlyEquals(intersection.SceneObject));
            Assert.IsTrue(preparedIntersection.Point.NearlyEquals(new Point(0, 0, -1)));
            Assert.IsTrue(preparedIntersection.EyeVector.NearlyEquals(new Vector(0, 0, -1)));
            Assert.IsTrue(preparedIntersection.NormalVector.NearlyEquals(new Vector(0, 0, -1)));
        }

        [Test()]
        public void TheHitWhenAnIntersectionOccursOnTheOutside()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere shape = new Sphere();
            Intersection intersection = new Intersection(4, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));

            // Then
            Assert.IsFalse(preparedIntersection.Inside);
        }

        [Test()]
        public void TheHitWhenAnIntersectionOccursOnTheInside()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            Sphere shape = new Sphere();
            Intersection intersection = new Intersection(1, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));

            // Then
            Assert.IsTrue(preparedIntersection.Point.NearlyEquals(new Point(0, 0, 1)));
            Assert.IsTrue(preparedIntersection.EyeVector.NearlyEquals(new Vector(0, 0, -1)));
            Assert.IsTrue(preparedIntersection.Inside);

            // NormalVector would have been (0, 0, 1), but is inverted!
            Assert.IsTrue(preparedIntersection.NormalVector.NearlyEquals(new Vector(0, 0, -1)));
        }

        [Test()]
        public void ShadingAnIntersection()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            SceneObject shape = world.SceneObjects[0];
            Intersection intersection = new Intersection(4, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color color = world.GetShadedHit(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.38066, 0.47583, 0.2855)));
        }

        [Test()]
        public void ShadingAnIntersectionFromTheInside()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            world.LightSources[0] = new PointLight(new Point(0, 0.25, 0), new Color(1, 1, 1));
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            SceneObject shape = world.SceneObjects[1];
            Intersection intersection = new Intersection(0.5, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color color = world.GetShadedHit(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.90498, 0.90498, 0.90498)));
        }

        [Test()]
        public void TheColorWhenARayMisses()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));

            // When
            Color color = world.GetColorAt(ray);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheColorWhenARayHits()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            // When
            Color color = world.GetColorAt(ray);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.38066, 0.47583, 0.2855)));
        }

        [Test()]
        public void TheColorWithAnIntersectionBehindTheRay()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            SceneObject outer = world.SceneObjects[0];
            outer.Material.Ambient = 1;

            SceneObject inner = world.SceneObjects[1];
            inner.Material.Ambient = 1;

            Ray ray = new Ray(new Point(0, 0, 0.75), new Vector(0, 0, -1));

            // When
            Color color = world.GetColorAt(ray);

            // Then
            Assert.IsTrue(color.NearlyEquals(inner.Material.Color));
        }

        [Test()]
        public void TheTransformationMatrixForTheDefaultOrientation()
        {
            // Given
            Point from = new Point(0, 0, 0);
            Point to = new Point(0, 0, -1);
            Vector up = new Vector(0, 1, 0);

            // When
            Matrix transform = from.GetViewTransform(to, up);

            // Then
            Assert.IsTrue(transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
        }

        [Test()]
        public void AViewTransfromationMatrixLookingInPositiveZDirection()
        {
            // Given
            Point from = new Point(0, 0, 0);
            Point to = new Point(0, 0, 1);
            Vector up = new Vector(0, 1, 0);

            // When
            Matrix transform = from.GetViewTransform(to, up);

            // Then
            Assert.IsTrue(transform.NearlyEquals(Matrix.NewScalingMatrix(-1, 1, -1)));
        }

        [Test()]
        public void TheViewTransformationMovesTheWorld()
        {
            // Given
            Point from = new Point(0, 0, 8);
            Point to = new Point(0, 0, 0);
            Vector up = new Vector(0, 1, 0);

            // When
            Matrix transform = from.GetViewTransform(to, up);

            // Then
            Assert.IsTrue(transform.NearlyEquals(Matrix.NewTranslationMatrix(0, 0, -8)));
        }

        [Test()]
        public void AnArbitraryViewTransformation()
        {
            // Given
            Point from = new Point(1, 3, 2);
            Point to = new Point(4, -2, 8);
            Vector up = new Vector(1, 1, 0);

            // When
            Matrix transform = from.GetViewTransform(to, up);

            // Then
            Assert.IsTrue(transform.NearlyEquals(new Matrix(
                new double[,]
                {
                    { -0.50709, 0.50709, 0.67612, -2.36643 },
                    { 0.76772, 0.60609, 0.12122, -2.82843 },
                    { -0.35857, 0.59761, -0.71714, 0.00000 },
                    { 0.00000, 0.00000, 0.00000, 1.00000 }
                }
            )));
        }

        [Test()]
        public void ConstructingACamera()
        {
            // Given
            int horizontalSize = 160;
            int verticalSize = 120;
            double fieldOfView = System.Math.PI / 2;

            // When
            Camera camera = new Camera(horizontalSize, verticalSize, fieldOfView);

            // Then
            Assert.AreEqual(160, camera.Width);
            Assert.AreEqual(120, camera.Height);
            Assert.IsTrue(camera.FieldOfView.NearlyEquals(System.Math.PI / 2));
            Assert.IsTrue(camera.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
        }

        [Test()]
        public void ThePixelSizeForAHorizontalCanvas()
        {
            // Given
            Camera camera = new Camera(200, 125, System.Math.PI / 2);

            // Then
            Assert.IsTrue(camera.PixelSize.NearlyEquals(0.01));
        }

        [Test()]
        public void ThePixelSizeForAVerticalCanvas()
        {
            // Given
            Camera camera = new Camera(125, 200, System.Math.PI / 2);

            // Then
            Assert.IsTrue(camera.PixelSize.NearlyEquals(0.01));
        }

        [Test()]
        public void ConstructingARayThroughTheCenterOfTheCanvas()
        {
            // Given
            Camera camera = new Camera(201, 101, System.Math.PI / 2);

            // When
            Ray ray = camera.GetRayForPixel(100, 50);

            // Then
            Assert.IsTrue(ray.Origin.NearlyEquals(new Point(0, 0, 0)));
            Assert.IsTrue(ray.Direction.NearlyEquals(new Vector(0, 0, -1)));
        }

        [Test()]
        public void ConstructingARayThroughACornerOfTheCanvas()
        {
            // Given
            Camera camera = new Camera(201, 101, System.Math.PI / 2);

            // When
            Ray ray = camera.GetRayForPixel(0, 0);

            // Then
            Assert.IsTrue(ray.Origin.NearlyEquals(new Point(0, 0, 0)));
            Assert.IsTrue(ray.Direction.NearlyEquals(new Vector(0.66519, 0.33259, -0.66851)));
        }

        [Test()]
        public void ConstructingARayWhenTheCameraIsTransformed()
        {
            // Given
            Camera camera = new Camera(201, 101, System.Math.PI / 2);

            // When
            camera.Transform = Matrix.NewRotationYMatrix(System.Math.PI / 4) * Matrix.NewTranslationMatrix(0, -2, 5);
            Ray ray = camera.GetRayForPixel(100, 50);

            // Then
            Assert.IsTrue(ray.Origin.NearlyEquals(new Point(0, 2, -5)));
            Assert.IsTrue(ray.Direction.NearlyEquals(new Vector(System.Math.Sqrt(2) / 2, 0, -System.Math.Sqrt(2) / 2)));
        }

        [Test()]
        public void RenderingAWorldWithACamera()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Camera camera = new Camera(11, 11, System.Math.PI / 2);
            Point from = new Point(0, 0, -5);
            Point to = new Point(0, 0, 0);
            Vector up = new Vector(0, 1, 0);
            camera.Transform = from.GetViewTransform(to, up);

            // When
            Canvas image = camera.Render(defaultWorld);

            // Then
            Assert.IsTrue(image[5, 5].NearlyEquals(new Color(0.38066, 0.47583, 0.2855)));
        }
    }
}
