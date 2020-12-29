using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class TriangleTests
    {
        [Test()]
        public void ConstructingATriangle()
        {
            // Given
            Point point1 = new Point(0, 1, 0);
            Point point2 = new Point(-1, 0, 0);
            Point point3 = new Point(1, 0, 0);

            Triangle triangle = new Triangle(point1, point2, point3);

            // Then
            Assert.AreSame(point1, triangle.Point1);
            Assert.AreSame(point2, triangle.Point2);
            Assert.AreSame(point3, triangle.Point3);

            Assert.IsTrue(triangle.EdgeVector1.NearlyEquals(new Vector(-1, -1, 0)));
            Assert.IsTrue(triangle.EdgeVector2.NearlyEquals(new Vector(1, -1, 0)));
            Assert.IsTrue(triangle.NormalVector.NearlyEquals(new Vector(0, 0, -1)));
        }

        [Test()]
        public void FindingTheNormalOnATriangle()
        {
            // Given
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            // When
            Vector n1 = triangle.GetNormalAtLocal(new Point(0, 0.5, 0));
            Vector n2 = triangle.GetNormalAtLocal(new Point(-0.5, 0.75, 0));
            Vector n3 = triangle.GetNormalAtLocal(new Point(0.5, 0.25, 0));

            // Then
            Assert.AreSame(n1, triangle.NormalVector);
            Assert.AreSame(n2, triangle.NormalVector);
            Assert.AreSame(n3, triangle.NormalVector);
        }

        [Test()]
        public void IntersectingARayParallelToTheTriangle()
        {
            // Given
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            Ray ray = new Ray(new Point(0, -1, -2), new Vector(0, 1, 0));

            // When
            Intersections intersections = triangle.GetIntersectionsLocal(ray);

            // Then
            Assert.IsEmpty(intersections);
        }

        [Test()]
        public void ARayMissesTheP1ToP3Edge()
        {
            // Given
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            Ray ray = new Ray(new Point(1, 1, -2), new Vector(0, 0, 1));

            //When
            Intersections intersections = triangle.GetIntersectionsLocal(ray);

            //Then
            Assert.IsEmpty(intersections);
        }

        [Test()]
        public void ARayMissesTheP1ToP2Edge()
        {
            // Given
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            Ray ray = new Ray(new Point(-1, 1, -2), new Vector(0, 0, 1));

            // When
            Intersections intersections = triangle.GetIntersectionsLocal(ray);

            // Then
            Assert.IsEmpty(intersections);
        }

        [Test()]
        public void ARayMissesTheP2ToP3Edge()
        {
            // Given
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            Ray ray = new Ray(new Point(0, -1, -2), new Vector(0, 0, 1));

            // When
            Intersections intersections = triangle.GetIntersectionsLocal(ray);

            // Then
            Assert.IsEmpty(intersections);
        }

        [Test()]
        public void ARayStrikesATriangle()
        {
            // Given
            Triangle triangle = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));

            Ray ray = new Ray(new Point(0, 0.5, -2), new Vector(0, 0, 1));

            // When
            Intersections intersections = triangle.GetIntersectionsLocal(ray);

            // Then
            Assert.AreEqual(1, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(2));
        }
    }
}