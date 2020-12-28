using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a sphere at the Point(0, 0, 0) with a radius of 1
    /// </summary>
    public class Sphere : Shape
    {
        #region Public Methods

        /// <summary>
        /// Gets the normal at local.
        /// </summary>
        /// <returns>The normal at local.</returns>
        /// <param name="objectPoint">Object point.</param>
        public override Vector GetNormalAtLocal(Point objectPoint, Intersection hit = null)
        {
            return new Vector(objectPoint.X, objectPoint.Y, objectPoint.Z);
        }

        /// <summary>
        /// Creates a new glassy sphere.
        /// </summary>
        /// <returns>The glass sphere.</returns>
        public static Sphere NewGlassSphere()
        {
            Sphere glassSphere = new Sphere();

            glassSphere.Material.Transparency = 1.0;
            glassSphere.Material.RefractiveIndex = 1.5;

            return glassSphere;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the intersections.
        /// Returns a list of intersections, where the ray hits the sphere.
        /// </summary>
        /// <returns>The intersections.</returns>
        /// <param name="ray">Ray.</param>
        protected override Intersections GetIntersectionsLocal(Ray localRay)
        {
            // The vector from the sphere's center, to the ray origin.
            // Remember: the sphere is centered at the world origin.
            Vector sphereToRay = localRay.Origin - new Point(0, 0, 0);

            double dotA = localRay.Direction.Dot(localRay.Direction);
            double dotB = 2 * localRay.Direction.Dot(sphereToRay);
            double dotC = sphereToRay.Dot(sphereToRay) - 1;

            double discriminant = dotB * dotB - 4 * dotA * dotC;

            if (discriminant < 0)
            {
                return new Intersections();
            }

            double distance1 = (-dotB - Math.Sqrt(discriminant)) / (2 * dotA);
            double distance2 = (-dotB + Math.Sqrt(discriminant)) / (2 * dotA);

            return new Intersections(
                new Intersection(distance1, this),
                new Intersection(distance2, this)
            );
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Point(-1, -1, -1),
                new Point(1, 1, 1));
        }

        /// <summary>
        /// Nearlies the equals local.
        /// </summary>
        /// <returns><c>true</c>, if equals local was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="sceneObject">Scene object.</param>
        protected override bool NearlyEqualsLocal(SceneObject sceneObject)
        {
            return sceneObject is Sphere;
        }

        #endregion
    }
}
