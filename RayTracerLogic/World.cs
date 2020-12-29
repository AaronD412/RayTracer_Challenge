using System;
using System.Collections.Generic;

namespace RayTracerLogic
{
    public class World
    {
        #region Private Members

        private List<Shape> shapes = new List<Shape>();
        private List<ILightSource> lightSources = new List<ILightSource>();

        #endregion

        #region Public Methods

        public Intersections GetIntersections(Ray ray)
        {
            Intersections intersections = new Intersections();

            foreach (Shape shape in shapes)
            {
                intersections.AddRange(shape.GetIntersections(ray));
            }

            intersections.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            return intersections;
        }

        public Color ShadeHit(PreparedIntersection hit, int remaining = 5)
        {
            Material material = hit.Shape.Material;

            Color surface = Color.GetBlack();
            foreach (ILightSource lightSource in lightSources)
            {
                double lightIntensity = lightSource.GetIntensityAt(hit.OverPoint, this);

                surface += material.GetLighting(
                    hit.Shape,
                    lightSource,
                    hit.OverPoint,
                    hit.EyeVector,
                    hit.NormalVector,
                    lightIntensity);
            }

            Color reflected = GetReflectedColor(hit, remaining);
            Color refracted = GetRefractedColor(hit, remaining);

            if (material.Reflective > 0 &&
                material.Transparency > 0)
            {
                double reflectance = hit.Schlick();

                return surface + reflected * reflectance + refracted * (1 - reflectance);
            }

            return surface + reflected + refracted;
        }

        public Color ColorAt(Ray ray, int remaining = 5)
        {
            Intersections intersections = GetIntersections(ray);

            foreach (Intersection intersection in intersections)
            {
                if (intersection.Distance >= 0)
                {
                    PreparedIntersection preparedIntersection = intersection.Prepare(ray, intersections);

                    return ShadeHit(preparedIntersection, remaining);
                }
            }

            return Color.GetBlack();
        }

        public bool IsShadowed(Point lightPosition, Point point)
        {
            Vector vector = lightPosition - point;
            double distance = vector.GetMagnitude();
            Vector direction = vector.Normalize();

            Ray ray = new Ray(point, direction);
            Intersections intersections = GetIntersections(ray);

            Intersection hit = intersections.GetHit();

            return (hit != null && hit.Distance < distance);
        }

        public Color GetReflectedColor(PreparedIntersection hit, int remaining = 5)
        {
            if (remaining < 1 ||
                hit.Shape.Material.Reflective == 0)
            {
                return Color.GetBlack();
            }

            Ray reflectRay = new Ray(hit.OverPoint, hit.ReflectionVector);
            Color color = ColorAt(reflectRay, remaining - 1);

            return color * hit.Shape.Material.Reflective;
        }

        public Color GetRefractedColor(PreparedIntersection hit, int remaining = 5)
        {
            if (remaining < 1 ||
                hit.Shape.Material.Transparency == 0)
            {
                return Color.GetBlack();
            }

            // Find the ratio of first index of refraction to the second.
            // (Yup, this is inverted from the definition of Snell's Law.)
            double nRatio = hit.N1 / hit.N2;

            // cos(theta_i) is the same as the dot product of the two vectors
            double cosI = hit.EyeVector.Dot(hit.NormalVector);

            // Find sin(theta_i)^2 via trigonometric identity
            double sin2T = nRatio * nRatio * (1 - cosI * cosI);

            if (sin2T > 1)
            {
                return Color.GetBlack();
            }

            // Find cos(theta_t) via trigonometric identity
            double cosT = Math.Sqrt(1.0 - sin2T);

            // Compute the direction of the refracted ray
            Vector direction = hit.NormalVector * (nRatio * cosI - cosT) - hit.EyeVector * nRatio;

            // Create the refracted ray
            Ray refractRay = new Ray(hit.UnderPoint, direction);

            // Find the color of the refracted ray, making sure to multiply
            // by the transparency value to account for any opacity
            Color color = ColorAt(refractRay, remaining - 1) * hit.Shape.Material.Transparency;

            return color;
        }

        #endregion

        #region Public Properties

        public List<Shape> Shapes
        {
            get
            {
                return shapes;
            }
        }

        public List<ILightSource> LightSources
        {
            get
            {
                return lightSources;
            }
        }

        #endregion
    }
}
