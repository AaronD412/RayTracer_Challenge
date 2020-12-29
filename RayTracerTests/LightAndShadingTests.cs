using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class LightAndShadingTests
    {
        [Test()]
        public void TheNormalOnASphereAtAPointOnTheXAxis()
        {
            // Given
            Sphere sphere = new Sphere();

            // When
            Vector normal = sphere.GetNormalAt(new Point(1, 0, 0));

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(1, 0, 0)));
        }

        [Test()]
        public void TheNormalOnASphereAtAPointOnTheYAxis()
        {
            // Given
            Sphere sphere = new Sphere();

            // When
            Vector normal = sphere.GetNormalAt(new Point(0, 1, 0));

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(0, 1, 0)));
        }

        [Test()]
        public void TheNormalOnASphereAtAPointOnTheZAxis()
        {
            // Given
            Sphere sphere = new Sphere();

            // When
            Vector normal = sphere.GetNormalAt(new Point(0, 0, 1));

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(0, 0, 1)));
        }

        [Test()]
        public void TheNormalOnASphereAtANonAxialPoint()
        {
            // Given
            Sphere sphere = new Sphere();

            // When
            Vector normal = sphere.GetNormalAt(new Point(System.Math.Sqrt(3) / 3, System.Math.Sqrt(3) / 3, System.Math.Sqrt(3) / 3));

            // Then
            Assert.IsTrue(normal.NearlyEquals(new Vector(System.Math.Sqrt(3) / 3, System.Math.Sqrt(3) / 3, System.Math.Sqrt(3) / 3)));
        }

        [Test()]
        public void TheNormalIsANormalizedVector()
        {
            // Given
            Sphere sphere = new Sphere();

            // When
            Vector normal = sphere.GetNormalAt(new Point(System.Math.Sqrt(3) / 3, System.Math.Sqrt(3) / 3, System.Math.Sqrt(3) / 3));

            // Then
            Assert.IsTrue(normal.NearlyEquals(normal.Normalize()));
        }

        [Test()]
        public void ReflectingAVectorApproachingAt45Degree()
        {
            // Given
            Vector vector = new Vector(1, -1, 0);
            Vector normal = new Vector(0, 1, 0);

            // When
            Vector reflection = vector.GetReflect(normal);

            // Then
            Assert.IsTrue(reflection.NearlyEquals(new Vector(1, 1, 0)));
        }

        [Test()]
        public void ReflectingAVectorOffASlantedSurface()
        {
            // Given
            Vector vector = new Vector(0, -1, 0);
            Vector normal = new Vector(System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2, 0);

            // When
            Vector reflection = vector.GetReflect(normal);

            // Then
            Assert.IsTrue(reflection.NearlyEquals(new Vector(1, 0, 0)));
        }

        [Test()]
        public void APointLightHAsAPositionAndIntensity()
        {
            // Given
            Color intensity = new Color(1, 1, 1);
            Point position = new Point(0, 0, 0);

            // When
            PointLight light = new PointLight(position, intensity);

            // Then
            Assert.IsTrue(light.Position.NearlyEquals(position));
            Assert.IsTrue(light.Intensity.NearlyEquals(intensity));
        }

        [Test()]
        public void TheDefaultMaterial()
        {
            // Given
            Material material = new Material();

            // Then
            Assert.IsTrue(material.Color.NearlyEquals(new Color(1, 1, 1)));
            Assert.IsTrue(material.Ambient.NearlyEquals(0.1));
            Assert.IsTrue(material.Diffuse.NearlyEquals(0.9));
            Assert.IsTrue(material.Specular.NearlyEquals(0.9));
            Assert.IsTrue(material.Shininess.NearlyEquals(200));
        }

        [Test()]
        public void LightingWithTheEyeBetweenTheLightAndTheSurface()
        {
            // Given
            Material material = new Material();
            Point position = new Point(0, 0, 0);

            Vector eyeVector = new Vector(0, 0, -1);
            Vector normalVector = new Vector(0, 0, -1);

            PointLight light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));

            // When
            Color result = material.GetLighting(new Sphere(), light, position, eyeVector, normalVector, 1);

            // Then
            Assert.IsTrue(result.NearlyEquals(new Color(1.9, 1.9, 1.9)));
        }

        [Test()]
        public void LightingWithTheEyeBetweenTheLightAndTheSurfaceEyeOffset45Degree()
        {
            // Given
            Material material = new Material();
            Point position = new Point(0, 0, 0);

            Vector eyeVector = new Vector(0, System.Math.Sqrt(2) / 2, -System.Math.Sqrt(2) / 2);
            Vector normalVector = new Vector(0, 0, -1);

            PointLight light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));

            // When
            Color result = material.GetLighting(new Sphere(), light, position, eyeVector, normalVector, 1);

            // Then
            Assert.IsTrue(result.NearlyEquals(new Color(1, 1, 1)));
        }

        [Test()]
        public void LightingWithEyeOppositeSurfaceLightOffset45Degree()
        {
            // Given
            Material material = new Material();
            Point position = new Point(0, 0, 0);

            Vector eyeVector = new Vector(0, 0, -1);
            Vector normalVector = new Vector(0, 0, -1);

            PointLight light = new PointLight(new Point(0, 10, -10), new Color(1, 1, 1));

            // When
            Color result = material.GetLighting(new Sphere(), light, position, eyeVector, normalVector, 1);

            // Then
            Assert.IsTrue(result.NearlyEquals(new Color(0.7364, 0.7364, 0.7364)));
        }

        [Test()]
        public void LightingWithEyeInThePathOfTheReflectionVector()
        {
            // Given
            Material material = new Material();
            Point position = new Point(0, 0, 0);

            Vector eyeVector = new Vector(0, -System.Math.Sqrt(2) / 2, -System.Math.Sqrt(2) / 2);
            Vector normalVector = new Vector(0, 0, -1);

            PointLight light = new PointLight(new Point(0, 10, -10), new Color(1, 1, 1));

            // When
            Color result = material.GetLighting(new Sphere(), light, position, eyeVector, normalVector, 1);

            // Then
            Assert.IsTrue(result.NearlyEquals(new Color(1.6364, 1.6364, 1.6364)));
        }

        [Test()]
        public void LightingWithTheLightBehindTheSurface()
        {
            // Given
            Material material = new Material();
            Point position = new Point(0, 0, 0);

            Vector eyeVector = new Vector(0, 0, -1);
            Vector normalVector = new Vector(0, 0, -1);

            PointLight light = new PointLight(new Point(0, 0, 10), new Color(1, 1, 1));

            // When
            Color result = material.GetLighting(new Sphere(), light, position, eyeVector, normalVector, 1);

            // Then
            Assert.IsTrue(result.NearlyEquals(new Color(0.1, 0.1, 0.1)));
        }
    }
}
