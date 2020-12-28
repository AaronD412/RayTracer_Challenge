using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class PlanesTests
    {
        [Test()]
        public void TheDefaultTransformation()
        {
            // Given
            TestShape testShape = new TestShape();

            // Then
            Assert.IsTrue(testShape.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
        }

        [Test()]
        public void AssigningATransformation()
        {
            // Given
            TestShape testShape = new TestShape();

            // When
            testShape.Transform = testShape.Transform.Translate(2, 3, 4);

            // Then
            Assert.IsTrue(testShape.Transform.NearlyEquals(Matrix.NewTranslationMatrix(2, 3, 4)));
        }

        [Test()]
        public void TheDefaultMaterial()
        {
            // Given
            TestShape testShape = new TestShape();

            // When
            Material material = testShape.Material;

            // Then
            Assert.IsTrue(material.NearlyEquals(new Material()));
        }

        [Test()]
        public void AssigningAMaterial()
        {
            // Given
            TestShape testShape = new TestShape();
            Material material = new Material();
            material.Ambient = 1;

            // When
            testShape.Material = material;

            // Then
            Assert.IsTrue(testShape.Material.NearlyEquals(material));
        }

        [Test()]
        public void IntersectingAScaledShapeWithARay()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            TestShape testShape = new TestShape();

            // When
            testShape.Transform = testShape.Transform.Scale(2, 2, 2);
            Intersections intersections = testShape.GetIntersections(ray);

            // Then
            Assert.IsTrue(testShape.LocalRay.Origin.NearlyEquals(new Point(0, 0, -2.5)));
            Assert.IsTrue(testShape.LocalRay.Direction.NearlyEquals(new Vector(0, 0, 0.5)));
        }

        [Test()]
        public void IntersectingATranslatedShapeWithARay()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            TestShape testShape = new TestShape();

            // When
            testShape.Transform = testShape.Transform.Translate(5, 0, 0);
            Intersections intersections = testShape.GetIntersections(ray);

            // Then
            Assert.IsTrue(testShape.LocalRay.Origin.NearlyEquals(new Point(-5, 0, -5)));
            Assert.IsTrue(testShape.LocalRay.Direction.NearlyEquals(new Vector(0, 0, 1)));
        }

        [Test()]
        public void ComputingTheNormalOnATranslatedShape()
        {
            // Given
            TestShape testShape = new TestShape();

            // When
            testShape.Transform = testShape.Transform.Translate(0, 1, 0);
            Vector normal = testShape.GetNormalAt(new Point(0, 1.70711, -0.70711));

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(0, 0.70711, -0.70711)));
        }

        [Test()]
        public void ComputingTheNormalOnATransformedShape()
        {
            // Given
            TestShape testShape = new TestShape();
            testShape.Transform = testShape.Transform.RotateZ(System.Math.PI / 5).Scale(1, 0.5, 1);

            // When
            Vector normal = testShape.GetNormalAt(new Point(0, System.Math.PI / 2, -System.Math.PI / 2));

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(0, 0.97014, -0.24254)));
        }

        [Test()]
        public void ASphereIsAShape()
        {
            // Given
            Sphere sphere = new Sphere();

            // Then
            Assert.IsInstanceOf<SceneObject>(sphere);
        }

        [Test()]
        public void TheNormalOfAPlaneIsConstantEverywhere()
        {
            // Given
            Plane plane = new Plane();

            // When
            Vector normalOne = plane.GetNormalAtLocal(new Point(0, 0, 0));
            Vector normalTwo = plane.GetNormalAtLocal(new Point(10, 0, -10));
            Vector normalThree = plane.GetNormalAtLocal(new Point(-5, 0, 150));

            // Then
            Assert.IsTrue(normalOne.NearlyEquals(new Vector(0, 1, 0)));
            Assert.IsTrue(normalTwo.NearlyEquals(new Vector(0, 1, 0)));
            Assert.IsTrue(normalThree.NearlyEquals(new Vector(0, 1, 0)));
        }

        [Test()]
        public void IntersectWithARayPrallelToThePlane()
        {
            // Given
            Plane plane = new Plane();
            Ray ray = new Ray(new Point(0, 10, 0), new Vector(0, 0, 1));

            // When
            Intersections intersections = plane.GetIntersections(ray);

            // Then
            Assert.AreEqual(0, intersections.Count);
        }

        [Test()]
        public void IntersectWithACoplanarRay()
        {
            // Given
            Plane plane = new Plane();
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            // When
            Intersections intersections = plane.GetIntersections(ray);

            // Then
            Assert.AreEqual(0, intersections.Count);
        }

        [Test()]
        public void ARayIntersectingAPlaneFromAbove()
        {
            // Given
            Plane plane = new Plane();
            Ray ray = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));

            // When
            Intersections intersections = plane.GetIntersections(ray);

            // Then
            Assert.AreEqual(1, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(1));
            Assert.AreSame(plane, intersections[0].SceneObject);
        }

        [Test()]
        public void ARayIntersectingAPlaneFromBelow()
        {
            // Given
            Plane plane = new Plane();
            Ray ray = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));

            // When
            Intersections intersections = plane.GetIntersections(ray);

            // Then
            Assert.AreEqual(1, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(1));
            Assert.AreSame(plane, intersections[0].SceneObject);
        }

        private class TestShape : SceneObject
        {
            private Ray localRay;

            protected override Intersections GetIntersectionsLocal(Ray localRay)
            {
                this.localRay = localRay;

                return new Intersections();
            }

            public override Vector GetNormalAtLocal(Point objectPoint)
            {
                return new Vector(objectPoint.X, objectPoint.Y, objectPoint.Z);
            }

            public Ray LocalRay
            {
                get
                {
                    return localRay;
                }
            }

            protected override bool NearlyEqualsLocal(SceneObject sceneObject)
            {
                return sceneObject is TestShape;
            }
        }
    }
}
