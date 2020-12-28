using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class RaySphereIntersection
    {
        [Test()]
        public void CreatingAndQueringARay()
        {
            // Given
            Point origin = new Point(1, 2, 3);
            Vector direction = new Vector(4, 5, 6);

            // When
            Ray ray = new Ray(origin, direction);

            // Then
            Assert.IsTrue((ray.Origin).NearlyEquals(origin));
            Assert.IsTrue((ray.Direction).NearlyEquals(direction));
        }

        [Test()]
        public void ComputingAPointFromADistance()
        {
            // Given
            Ray ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));

            // Then
            Assert.IsTrue(ray.GetPosition(0).NearlyEquals(new Point(2, 3, 4)));
            Assert.IsTrue(ray.GetPosition(1).NearlyEquals(new Point(3, 3, 4)));
            Assert.IsTrue(ray.GetPosition(-1).NearlyEquals(new Point(1, 3, 4)));
            Assert.IsTrue(ray.GetPosition(2.5).NearlyEquals(new Point(4.5, 3, 4)));
        }

        [Test()]
        public void ARayIntersectsASphereAtTwoPoints()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();

            // When
            Intersections intersections = sphere.GetIntersections(ray);

            // Then
            Assert.AreEqual(2, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(4.0));
            Assert.IsTrue(intersections[1].Distance.NearlyEquals(6.0));
        }

        [Test()]
        public void ARayIntersectsASphereAtATangent()
        {
            // Given
            Ray ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();

            // When
            Intersections intersections = sphere.GetIntersections(ray);

            // Then
            Assert.AreEqual(2, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(5.0));
            Assert.IsTrue(intersections[1].Distance.NearlyEquals(5.0));
        }

        [Test()]
        public void ARayMissesASphere()
        {
            // Given
            Ray ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();

            // When
            Intersections intersections = sphere.GetIntersections(ray);

            // Then
            Assert.AreEqual(0, intersections.Count);
        }

        [Test()]
        public void ARayOriginatesInsideASphere()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();

            // When
            Intersections intersections = sphere.GetIntersections(ray);

            // Then
            Assert.AreEqual(2, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(-1.0));
            Assert.IsTrue(intersections[1].Distance.NearlyEquals(1.0));
        }

        [Test()]
        public void ASphereIsBehindARay()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();

            // When
            Intersections intersections = sphere.GetIntersections(ray);

            // Then
            Assert.AreEqual(2, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(-6.0));
            Assert.IsTrue(intersections[1].Distance.NearlyEquals(-4.0));
        }

        [Test()]
        public void AnIntersectionEncapsulatesDistanceAndObject()
        {
            // Given
            Sphere sphere = new Sphere();

            // When
            Intersection intersection = new Intersection(3.5, sphere);

            // Then
            Assert.IsTrue(intersection.Distance.NearlyEquals(3.5));
            Assert.AreSame(sphere, intersection.SceneObject);
        }

        [Test()]
        public void Aggregatingintersections()
        {
            // Given
            Sphere sphere = new Sphere();
            Intersection intersection1 = new Intersection(1, sphere);
            Intersection intersection2 = new Intersection(2, sphere);

            // When
            Intersections intersections = new Intersections(intersection1, intersection2);

            // Then
            Assert.AreEqual(2, intersections.Count);
            Assert.IsTrue(intersections[0].Distance.NearlyEquals(1));
            Assert.IsTrue(intersections[1].Distance.NearlyEquals(2));
        }

        [Test()]
        public void IntersectSetsTheObjectOnTheIntersection()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();

            // When
            Intersections intersections = sphere.GetIntersections(ray);

            // Then
            Assert.AreEqual(2, intersections.Count);
            Assert.AreSame(sphere, intersections[0].SceneObject);
            Assert.AreSame(sphere, intersections[1].SceneObject);
        }

        [Test()]
        public void TheHitWhenAllIntersectionsHavePositiveDistance()
        {
            // Given
            Sphere sphere = new Sphere();
            Intersection intersection1 = new Intersection(1, sphere);
            Intersection intersection2 = new Intersection(2, sphere);
            Intersections intersections = new Intersections(intersection2, intersection1);

            // When
            Intersection intersection = intersections.GetHit();

            // Then
            Assert.AreSame(intersection1, intersection);
        }

        [Test()]
        public void TheHitWhenSomeIntersectionsHaveNegativeDistance()
        {
            // Given
            Sphere sphere = new Sphere();
            Intersection intersection1 = new Intersection(-1, sphere);
            Intersection intersection2 = new Intersection(1, sphere);
            Intersections intersections = new Intersections(intersection2, intersection1);

            // When
            Intersection intersection = intersections.GetHit();

            // Then
            Assert.AreSame(intersection2, intersection);
        }

        [Test()]
        public void TheHitWhenAllIntersectionsHaveNegativeDistance()
        {
            // Given
            Sphere sphere = new Sphere();
            Intersection intersection1 = new Intersection(-2, sphere);
            Intersection intersection2 = new Intersection(-1, sphere);
            Intersections intersections = new Intersections(intersection2, intersection1);

            // When
            Intersection intersection = intersections.GetHit();

            // Then
            Assert.IsNull(intersection);
        }

        [Test()]
        public void TheHitIsAlwaysTheLowestNonNegativeIntersection()
        {
            // Given
            Sphere sphere = new Sphere();

            Intersection intersection1 = new Intersection(5, sphere);
            Intersection intersection2 = new Intersection(7, sphere);
            Intersection intersection3 = new Intersection(-3, sphere);
            Intersection intersection4 = new Intersection(2, sphere);

            Intersections intersections = new Intersections(
                intersection1, 
                intersection2,
                intersection3,
                intersection4
            );

            // When
            Intersection intersection = intersections.GetHit();

            // Then
            Assert.AreSame(intersection4, intersection);
        }

        [Test()]
        public void TranslatingARay()
        {
            // Given
            Ray ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            Matrix translation = Matrix.NewTranslationMatrix(3, 4, 5);

            // When
            Ray transform = ray.Transform(translation);

            // Then
            Assert.IsTrue(transform.Origin.NearlyEquals(new Point(4, 6, 8)));
            Assert.IsTrue(transform.Direction.NearlyEquals(new Vector(0, 1, 0)));
        }

        [Test()]
        public void ScalingARay()
        {
            // Given
            Ray ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            Matrix scaling = Matrix.NewScalingMatrix(2, 3, 4);

            // When
            Ray transform = ray.Transform(scaling);

            // Then
            Assert.IsTrue(transform.Origin.NearlyEquals(new Point(2, 6, 12)));
            Assert.IsTrue(transform.Direction.NearlyEquals(new Vector(0, 3, 0)));
        }
    }
}
