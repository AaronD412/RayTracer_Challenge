using System;
using System.Collections.Generic;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a World.
    /// </summary>
    public class World
    {
        #region Private Members

        /// <summary>
        /// The scene objects.
        /// </summary>
        private List<SceneObject> sceneObjects = new List<SceneObject>();

        /// <summary>
        /// The light sources.
        /// </summary>
        private List<PointLight> lightSources = new List<PointLight>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the intersections.
        /// </summary>
        /// <returns>The intersections.</returns>
        /// <param name="ray">Ray.</param>
        public Intersections GetIntersections(Ray ray)
        {
            Intersections intersections = new Intersections();

            foreach (SceneObject sceneObject in sceneObjects)
            {
                intersections.AddRange(sceneObject.GetIntersections(ray));
            }

            intersections.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            return intersections;
        }

        /// <summary>
        /// Gets the shaded hit.
        /// </summary>
        /// <returns>The shaded hit.</returns>
        /// <param name="preparedIntersection">Prepared intersection.</param>
        /// <param name="remaining">Remaining.</param>
        public Color GetShadedHit(PreparedIntersection preparedIntersection, int remaining = Constants.MaxReflectionRecursionLevel)
        {
            Color surface = Color.GetBlack();

            foreach (PointLight lightSource in lightSources)
            {
                surface += preparedIntersection.SceneObject.Material.GetLighting(
                    preparedIntersection.SceneObject,
                    lightSource,
                    preparedIntersection.OverPoint,
                    preparedIntersection.EyeVector,
                    preparedIntersection.NormalVector,
                    IsShadowed(preparedIntersection.OverPoint, lightSource)
                );
            }
            
            Color reflected = GetReflectedColor(preparedIntersection, remaining);
            Color refracted = GetRefractedColor(preparedIntersection, remaining);

            Material material = preparedIntersection.SceneObject.Material;

            if (material.Reflective > 0 && material.Transparency > 0)
            {
                double reflectance = preparedIntersection.Schlick();
                return surface + reflected * reflectance + refracted * (1 - reflectance);
            }

            return surface + reflected + refracted;
        }

        /// <summary>
        /// Gets the color at ray with recursion.
        /// </summary>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/>.</returns>
        /// <param name="ray">Ray.</param>
        /// <param name="remaining">Remaining.</param>
        public Color GetColorAt(Ray ray, int remaining = Constants.MaxReflectionRecursionLevel)
        {
            Intersections intersections = GetIntersections(ray);
            Intersection hit = intersections.GetHit();

            if (hit == null)
            {
                return Color.GetBlack();
            }

            PreparedIntersection preparedIntersection = hit.GetPreparedIntersection(ray, intersections);

            return GetShadedHit(preparedIntersection, remaining);
        }

        /// <summary>
        /// Gets the color of the reflected.
        /// </summary>
        /// <returns>The reflected color.</returns>
        /// <param name="preparedIntersection">Prepared intersection.</param>
        /// <param name="remaining">Remaining.</param>
        public Color GetReflectedColor(PreparedIntersection preparedIntersection, int remaining = Constants.MaxReflectionRecursionLevel)
        {
            if (remaining < 1)
            {
                return Color.GetBlack();
            }

            if (preparedIntersection.SceneObject.Material.Reflective.CompareTo(0) == 0)
            {
                return Color.GetBlack();
            }

            Ray reflectRay = new Ray(preparedIntersection.OverPoint, preparedIntersection.ReflectVector);
            Color color = GetColorAt(reflectRay, remaining - 1);

            return color * preparedIntersection.SceneObject.Material.Reflective;
        }

        /// <summary>
        /// Proofs if the point is shadowed.
        /// </summary>
        /// <returns><c>true</c>, if shadowed was ised, <c>false</c> otherwise.</returns>
        /// <param name="point">Point.</param>
        /// <param name="lightSource">Light source.</param>
        public bool IsShadowed(Point point, PointLight lightSource)
        {
            Vector vector = lightSource.Position - point;
            double distance = vector.GetMagnitude();
            Vector direction = vector.Normalize();

            Ray ray = new Ray(point, direction);
            Intersections intersections = GetIntersections(ray);

            Intersection hit = intersections.GetHit();

            if (hit != null && hit.Distance < distance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the refracted color.
        /// </summary>
        /// <returns>The refracted color.</returns>
        /// <param name="preparedIntersection">Prepared intersection.</param>
        /// <param name="remaining">Remaining.</param>
        public Color GetRefractedColor(PreparedIntersection preparedIntersection, int remaining)
        {
            if (remaining == 0)
            {
                return Color.GetBlack();
            }

            if (preparedIntersection.SceneObject.Material.Transparency.CompareTo(0) == 0)
            {
                return Color.GetBlack();
            }

            // Find the ratio of the first index of refraction to the second
            // (Yup, this is inverted from the definition of Snell's law).
            double nRatio = preparedIntersection.N1 / preparedIntersection.N2;

            // cos(thetaI) is the same as the dot product of the two vectors.
            double cosI = preparedIntersection.EyeVector.Dot(preparedIntersection.NormalVector);

            // Find sin(thetaT)^2 via trigonometric identity.
            double sin2T = (nRatio * nRatio) * (1 - cosI * cosI);

            if (sin2T > 1)
            {
                return Color.GetBlack();
            }

            // Find cos(thetaT) via trigonometric identity
            double cosT = Math.Sqrt(1.0 - sin2T);

            // Compute the direction of the refracted ray
            Vector direction = preparedIntersection.NormalVector *
                (nRatio * cosI - cosT) -
                preparedIntersection.EyeVector *
                nRatio;

            // Create the refracted ray
            Ray refractedRay = new Ray(preparedIntersection.UnderPoint, direction);

            // Find the color of the refracted ray, making sure to multiply
            // by the transparency value to account for any opacity.
            Color color = GetColorAt(refractedRay, remaining - 1) * preparedIntersection.SceneObject.Material.Transparency;

            return color;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the scene objects.
        /// </summary>
        /// <value>The scene objects.</value>
        public List<SceneObject> SceneObjects
        {
            get
            {
                return sceneObjects;
            }
        }

        /// <summary>
        /// Gets the light sources.
        /// </summary>
        /// <value>The light sources.</value>
        public List<PointLight> LightSources
        {
            get
            {
                return lightSources;
            }
        }

        #endregion
    }
}
