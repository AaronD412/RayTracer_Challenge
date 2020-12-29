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
            Assert.AreEqual(0, world.Shapes.Count);
            Assert.AreEqual(0, world.LightSources.Count);
        }

        [Test()]
        public void TheDefaultWorld()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();

            // Then
            PointLight light = new PointLight(
                new Point(-10, 10, -10),
                Color.GetWhite());

            Sphere sphere1 = new Sphere();

            sphere1.Material.Color = new Color(0.8, 1.0, 0.6);
            sphere1.Material.Diffuse = 0.7;
            sphere1.Material.Specular = 0.2;

            Sphere sphere2 = new Sphere();

            sphere2.Transform = Matrix.NewScalingMatrix(0.5, 0.5, 0.5);

            Assert.IsTrue(world.LightSources[0].NearlyEquals(light));

            bool containsSphere1 = false;
            bool containsSphere2 = false;

            foreach (Shape shape in world.Shapes)
            {
                if (shape.NearlyEquals(sphere1))
                {
                    containsSphere1 = true;
                }
                else if (shape.NearlyEquals(sphere2))
                {
                    containsSphere2 = true;
                }
            }

            Assert.AreEqual(true, containsSphere1);
            Assert.AreEqual(true, containsSphere2);
        }

        [Test()]
        public void IntersectAWorldWithARay()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Intersections intersections = world.GetIntersections(ray);

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
            Shape shape = new Sphere();
            Intersection hit = new Intersection(4, shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            // Then
            Assert.IsTrue(preparedIntersection.Distance.NearlyEquals(hit.Distance));
            Assert.IsTrue(preparedIntersection.Shape.NearlyEquals(hit.Shape));
            Assert.IsTrue(preparedIntersection.Point.NearlyEquals(new Point(0, 0, -1)));
            Assert.IsTrue(preparedIntersection.EyeVector.NearlyEquals(new Vector(0, 0, -1)));
            Assert.IsTrue(preparedIntersection.NormalVector.NearlyEquals(new Vector(0, 0, -1)));
        }

        [Test()]
        public void TheHitWhenAnIntersectionOccursOnTheOutside()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(4, shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            // Then
            Assert.AreEqual(false, preparedIntersection.Inside);
        }

        [Test()]
        public void TheHitWhenAnIntersectionOccursOnTheInside()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            Shape shape = new Sphere();
            Intersection hit = new Intersection(1, shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));

            // Then
            Assert.IsTrue(preparedIntersection.Point.NearlyEquals(new Point(0, 0, 1)));
            Assert.IsTrue(preparedIntersection.EyeVector.NearlyEquals(new Vector(0, 0, -1)));
            Assert.AreEqual(true, preparedIntersection.Inside);

            // Normal would have been (0, 0, 1), but is inverted!
            Assert.IsTrue(preparedIntersection.NormalVector.NearlyEquals(new Vector(0, 0, -1)));
        }

        [Test()]
        public void ShadingAnIntersection()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Shape shape = world.Shapes[0];
            Intersection hit = new Intersection(4, shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));
            Color color = world.ShadeHit(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.38066, 0.47583, 0.2855)));
        }

        [Test()]
        public void ShadingAnIntersectionFromTheInside()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            world.LightSources[0] = new PointLight(new Point(0, 0.25, 0), Color.GetWhite());

            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            Shape shape = world.Shapes[1];
            Intersection hit = new Intersection(0.5, shape);

            PreparedIntersection preparedIntersection = hit.Prepare(ray, new Intersections(hit));
            Color color = world.ShadeHit(preparedIntersection);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.90498, 0.90498, 0.90498)));
        }

        [Test()]
        public void TheColorWhenARayMisses()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
            Color color = world.ColorAt(ray);

            // Then
            Assert.IsTrue(color.NearlyEquals(Color.GetBlack()));
        }

        [Test()]
        public void TheColorWhenARayHits()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Color color = world.ColorAt(ray);

            // Then
            Assert.IsTrue(color.NearlyEquals(new Color(0.38066, 0.47583, 0.2855)));
        }

        [Test()]
        public void TheColorWithAnIntersectionBehindTheRay()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Shape outer = world.Shapes[0];
            outer.Material.Ambient = 1;
            Shape inner = world.Shapes[1];
            inner.Material.Ambient = 1;
            Ray ray = new Ray(new Point(0, 0, 0.75), new Vector(0, 0, -1));
            Color color = world.ColorAt(ray);

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

            Matrix viewTransform = from.ViewTransform(to, up);

            // Then
            Assert.IsTrue(viewTransform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
        }

        [Test()]
        public void AViewTransformationMatrixLookingInPositiveZDirection()
        {
            // Given
            Point from = new Point(0, 0, 0);
            Point to = new Point(0, 0, 1);
            Vector up = new Vector(0, 1, 0);

            Matrix viewTransform = from.ViewTransform(to, up);

            // Then
            Assert.IsTrue(viewTransform.NearlyEquals(Matrix.NewScalingMatrix(-1, 1, -1)));
        }

        [Test()]
        public void TheViewTransformationMovesTheWorld()
        {
            // Given
            Point from = new Point(0, 0, 8);
            Point to = new Point(0, 0, 0);
            Vector up = new Vector(0, 1, 0);

            Matrix viewTransform = from.ViewTransform(to, up);

            // Then
            Assert.IsTrue(viewTransform.NearlyEquals(Matrix.NewTranslationMatrix(0, 0, -8)));
        }

        [Test()]
        public void AnArbitraryViewTransformation()
        {
            // Given
            Point from = new Point(1, 3, 2);
            Point to = new Point(4, -2, 8);
            Vector up = new Vector(1, 1, 0);

            Matrix viewTransform = from.ViewTransform(to, up);

            // Then
            Assert.IsTrue(
                viewTransform.NearlyEquals(
                    new Matrix(
                        new double[,]
                        {
                            { -0.50709, 0.50709, 0.67612, -2.36643 },
                            { 0.76772, 0.60609, 0.12122, -2.82843 },
                            { -0.35857, 0.59761, -0.71714, 0 },
                            { 0, 0, 0, 1 }
                        }
                    )
                )
            );
        }

        [Test()]
        public void ConstructingACamera()
        {
            // Given
            int horizontalSize = 160;
            int verticalSize = 120;
            double fieldOfView = System.Math.PI / 2;
            Camera camera = new Camera(horizontalSize, verticalSize, fieldOfView);

            // Then
            Assert.AreEqual(160, camera.HorizontalSize);
            Assert.AreEqual(120, camera.VerticalSize);
            Assert.IsTrue((System.Math.PI / 2).NearlyEquals(camera.FieldOfView));
            Assert.IsTrue(camera.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
        }

        [Test()]
        public void ThePixelSizeForAHorizontalCanvas()
        {
            // Given
            Camera camera = new Camera(200, 125, System.Math.PI / 2);

            // Then
            Assert.IsTrue(camera.GetPixelSize().NearlyEquals(0.01));
        }

        [Test()]
        public void ThePixelSizeForAVerticalCanvas()
        {
            // Given
            Camera camera = new Camera(125, 200, System.Math.PI / 2);

            // Then
            Assert.IsTrue(camera.GetPixelSize().NearlyEquals(0.01));
        }

        [Test()]
        public void ConstructingARayThroughTheCenterOfTheCanvas()
        {
            // Given
            Camera camera = new Camera(201, 101, System.Math.PI / 2);
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
            camera.Transform = Matrix.NewTranslationMatrix(0, -2, 5).RotateY(System.Math.PI / 4);
            Ray ray = camera.GetRayForPixel(100, 50);

            // Then
            Assert.IsTrue(ray.Origin.NearlyEquals(new Point(0, 2, -5)));
            Assert.IsTrue(ray.Direction.NearlyEquals(new Vector(System.Math.Sqrt(2) / 2, 0, -System.Math.Sqrt(2) / 2)));
        }

        [Test()]
        public void RenderingAWorldWithACamera()
        {
            // Given
            World world = DefaultWorld.NewDefaultWorld();
            Camera camera = new Camera(11, 11, System.Math.PI / 2);

            Point from = new Point(0, 0, -5);
            Point to = new Point(0, 0, 0);
            Vector up = new Vector(0, 1, 0);

            camera.Transform = from.ViewTransform(to, up);

            Canvas image = camera.Render(world);

            // Then
            Assert.IsTrue(image[5, 5].NearlyEquals(new Color(0.38066, 0.47583, 0.2855)));
        }
    }
}
