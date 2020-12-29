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
    }
}