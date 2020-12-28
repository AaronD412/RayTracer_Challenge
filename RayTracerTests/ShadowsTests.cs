using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class ShadowsTests
    {
        [Test()]
        public void LightingWithTheSurfaceInShadows()
        {
            // Given
            Material material = new Material();
            Point position = new Point(0, 0, 0);

            Vector eyeVector = new Vector(0, 0, -1);
            Vector normalVector = new Vector(0, 0, -1);

            PointLight light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));

            bool inShadow = true;

            // When
            Color result = material.GetLighting(new Sphere(), light, position, eyeVector, normalVector, inShadow);

            // Then
            Assert.IsTrue(result.NearlyEquals(new Color(0.1, 0.1, 0.1)));
        }

        [Test()]
        public void ThereIsNoShadowWhenNothingIsCollinearWithPointAndLight()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Point point = new Point(0, 10, 0);

            // Then
            Assert.IsFalse(defaultWorld.IsShadowed(point, defaultWorld.LightSources[0]));
        }

        [Test()]
        public void TheShadowWhenAnObjectIsBetweenThePointAndTheLight()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Point point = new Point(10, -10, 10);

            // Then
            Assert.IsTrue(defaultWorld.IsShadowed(point, defaultWorld.LightSources[0]));
        }

        [Test()]
        public void ThereIsNoShadowWhenAnObjectIsBehingTheLight()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Point point = new Point(-20, 20, -20);

            // Then
            Assert.IsFalse(defaultWorld.IsShadowed(point, defaultWorld.LightSources[0]));
        }

        [Test()]
        public void ThereIsNoShadowWhenAnObjectIsBehingThePoint()
        {
            // Given
            World defaultWorld = DefaultWorld.NewDefaultWorld();
            Point point = new Point(-2, 2, -2);

            // Then
            Assert.IsFalse(defaultWorld.IsShadowed(point, defaultWorld.LightSources[0]));
        }

        [Test()]
        public void ShadeHitIsGivenAnIntersectionInShadow()
        {
            // Given
            World world = new World();
            world.LightSources.Add(new PointLight(new Point(0, 0, -10), new Color(1, 1, 1)));

            Sphere sphereOne = new Sphere();
            world.SceneObjects.Add(sphereOne);

            Sphere sphereTwo = new Sphere();
            sphereTwo.Transform = sphereTwo.Transform.Translate(0, 0, 10);

            world.SceneObjects.Add(sphereTwo);

            Ray ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            Intersection intersection = new Intersection(4, sphereTwo);

            // When
            PreparedIntersection intersections = intersection.GetPreparedIntersection(ray, new Intersections(intersection));
            Color shadeHit = world.GetShadedHit(intersections);

            // Then
            Assert.IsTrue(shadeHit.NearlyEquals(new Color(0.1, 0.1, 0.1)));
        }

        [Test()]
        public void TheHitShouldOffsetThePoint()
        {
            // Given
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));

            Sphere shape = new Sphere();
            shape.Transform = shape.Transform.Translate(0, 0, 1);

            Intersection intersection = new Intersection(5, shape);

            // When
            PreparedIntersection preparedIntersection = intersection.GetPreparedIntersection(ray, new Intersections(intersection));

            // Then
            Assert.IsTrue(preparedIntersection.OverPoint.Z < (-Constants.Epsilon / 2));
            Assert.IsTrue(preparedIntersection.Point.Z > preparedIntersection.OverPoint.Z);
        }
    }
}
