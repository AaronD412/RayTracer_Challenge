using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class CubeTests
    {
        [Test()]
        public void ARayIntersectsACube()
        {
            // Given
            System.Tuple<Point, Vector, double, double>[] examples =
            {
                new System.Tuple<Point, Vector, double, double>(new Point(5, 0.5, 0), new Vector(-1, 0, 0), 4, 6),
                new System.Tuple<Point, Vector, double, double>(new Point(-5, 0.5, 0), new Vector(1, 0, 0), 4, 6),
                new System.Tuple<Point, Vector, double, double>(new Point(0.5, 5, 0), new Vector(0, -1, 0), 4, 6),
                new System.Tuple<Point, Vector, double, double>(new Point(0.5, -5, 0), new Vector(0, 1, 0), 4, 6),
                new System.Tuple<Point, Vector, double, double>(new Point(0.5, 0, 5), new Vector(0, 0, -1), 4, 6),
                new System.Tuple<Point, Vector, double, double>(new Point(0.5, 0, -5), new Vector(0, 0, 1), 4, 6),
                new System.Tuple<Point, Vector, double, double>(new Point(0, 0.5, 0), new Vector(0, 0, 1), -1, 1)
            };

            Cube cube = new Cube();

            foreach (System.Tuple<Point, Vector, double, double> example in examples)
            {
                Ray ray = new Ray(example.Item1, example.Item2);

                // When
                Intersections intersections = cube.GetIntersections(ray);

                // Then
                Assert.AreEqual(2, intersections.Count);

                Assert.IsTrue(intersections[0].Distance.NearlyEquals(example.Item3));
                Assert.IsTrue(intersections[1].Distance.NearlyEquals(example.Item4));
            }
        }

        [Test()]
        public void ARayMissesACube()
        {
            // Given
            System.Tuple<Point, Vector>[] examples =
            {
                new System.Tuple<Point, Vector>(new Point(-2, 0, 0), new Vector(0.2673, 0.5345, 0.8018)),
                new System.Tuple<Point, Vector>(new Point(0, -2, 0), new Vector(0.8018, 0.2673, 0.5345)),
                new System.Tuple<Point, Vector>(new Point(0, 0, -2), new Vector(0.5345, 0.8018, 0.2673)),
                new System.Tuple<Point, Vector>(new Point(2, 0, 2), new Vector(0, 0, -1)),
                new System.Tuple<Point, Vector>(new Point(0, 2, 2), new Vector(0, -1, 0)),
                new System.Tuple<Point, Vector>(new Point(2, 2, 0), new Vector(-1, 0, 0))
            };

            Cube cube = new Cube();

            foreach (System.Tuple<Point, Vector> example in examples)
            {
                Ray ray = new Ray(example.Item1, example.Item2);

                // When
                Intersections intersections = cube.GetIntersections(ray);

                // Then
                Assert.AreEqual(0, intersections.Count);
            }
        }

        [Test()]
        public void TheNormalOnTheSurfaceOfACube()
        {
            // Given
            System.Tuple<Point, Vector>[] examples =
            {
                new System.Tuple<Point, Vector>(new Point(1, 0.5, -0.8), new Vector(1, 0, 0)),
                new System.Tuple<Point, Vector>(new Point(-1, -0.2, 0.9), new Vector(-1, 0, 0)),
                new System.Tuple<Point, Vector>(new Point(-0.4, 1, -0.1), new Vector(0, 1, 0)),
                new System.Tuple<Point, Vector>(new Point(0.3, -1, -0.7), new Vector(0, -1, 0)),
                new System.Tuple<Point, Vector>(new Point(-0.6, 0.3, 1), new Vector(0, 0, 1)),
                new System.Tuple<Point, Vector>(new Point(0.4, 0.4, -1), new Vector(0, 0, -1)),
                new System.Tuple<Point, Vector>(new Point(1, 1, 1), new Vector(1, 0, 0)),
                new System.Tuple<Point, Vector>(new Point(-1, -1, -1), new Vector(-1, 0, 0))
            };

            Cube cube = new Cube();

            foreach (System.Tuple<Point, Vector> example in examples)
            {
                Point point = example.Item1;

                // When
                Vector normal = cube.GetNormalAtLocal(point);

                // Then
                Assert.IsTrue(normal.NearlyEquals(example.Item2));
            }
        }
    }
}
