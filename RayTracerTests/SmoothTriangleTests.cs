using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class SmoothTriangleTests
    {
        [Test()]
        public void ConstructingASmoothTriangle()
        {
            // Given
            Point point1 = new Point(0, 1, 0);
            Point point2 = new Point(-1, 0, 0);
            Point point3 = new Point(1, 0, 0);

            Vector normalVector1 = new Vector(0, 1, 0);
            Vector normalVector2 = new Vector(-1, 0, 0);
            Vector normalVector3 = new Vector(1, 0, 0);

            // When
            SmoothTriangle triangle = new SmoothTriangle(point1, point2, point3, normalVector1, normalVector2, normalVector3);

            // Then
            Assert.AreSame(point1, triangle.Point1);
            Assert.AreSame(point2, triangle.Point2);
            Assert.AreSame(point3, triangle.Point3);
            Assert.AreSame(normalVector1, triangle.NormalVector1);
            Assert.AreSame(normalVector2, triangle.NormalVector2);
            Assert.AreSame(normalVector3, triangle.NormalVector3);
        }

        [Test()]
        public void AnIntersectionCanEncapsulateUandV()
        {
            // Givne
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            // When
            Intersection intersection = new Intersection(3.5, triangle, 0.2, 0.4);

            // Then
            Assert.AreEqual(intersection.U, 0.2);
            Assert.AreEqual(intersection.V, 0.4);
        }

        [Test()]
        public void AnIntersectionWithASmoothTriangleStoresUAndV()
        {
            // Given
            Point point1 = new Point(0, 1, 0);
            Point point2 = new Point(-1, 0, 0);
            Point point3 = new Point(1, 0, 0);

            Vector normalVector1 = new Vector(0, 1, 0);
            Vector normalVector2 = new Vector(-1, 0, 0);
            Vector normalVector3 = new Vector(1, 0, 0);

            // When
            SmoothTriangle triangle = new SmoothTriangle(point1, point2, point3, normalVector1, normalVector2, normalVector3);
            Ray ray = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            Intersections intersections = triangle.GetIntersectionsLocal(ray);

            // Then
            Assert.IsTrue(intersections[0].U.GetValueOrDefault(0.0).NearlyEquals(0.45));
            Assert.IsTrue(intersections[0].V.GetValueOrDefault(0.0).NearlyEquals(0.25));
        }

        [Test()]
        public void ASmoothTrianlgeUsesUAndVToInterpolateTheNormal()
        {
            // Given
            Point point1 = new Point(0, 1, 0);
            Point point2 = new Point(-1, 0, 0);
            Point point3 = new Point(1, 0, 0);

            Vector normalVector1 = new Vector(0, 1, 0);
            Vector normalVector2 = new Vector(-1, 0, 0);
            Vector normalVector3 = new Vector(1, 0, 0);

            // When
            SmoothTriangle triangle = new SmoothTriangle(point1, point2, point3, normalVector1, normalVector2, normalVector3);
            Intersection intersection = new Intersection(1, triangle, 0.45, 0.25);
            Vector normal = triangle.GetNormalAt(new Point(0, 0, 0), intersection);

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(-0.5547, 0.83205, 0)));
        }

        [Test()]
        public void PreparingTheNormalOnASmoothTriangle()
        {
            // Given
            Point point1 = new Point(0, 1, 0);
            Point point2 = new Point(-1, 0, 0);
            Point point3 = new Point(1, 0, 0);

            Vector normalVector1 = new Vector(0, 1, 0);
            Vector normalVector2 = new Vector(-1, 0, 0);
            Vector normalVector3 = new Vector(1, 0, 0);

            // When
            SmoothTriangle triangle = new SmoothTriangle(point1, point2, point3, normalVector1, normalVector2, normalVector3);
            Ray ray = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));

            Intersection intersection = new Intersection(1, triangle, 0.45, 0.25);
            Intersections intersections = new Intersections(intersection);

            PreparedIntersection preparedIntersection = intersection.Prepare(ray, intersections);

            // Then
            Assert.IsTrue(preparedIntersection.NormalVector.NearlyEquals(new Vector(-0.5547, 0.83205, 0)));
        }
    }
}