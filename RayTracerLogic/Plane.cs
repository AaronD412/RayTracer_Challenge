using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a sphere at the Point(0, 0, 0) with a radius of 1
    /// </summary>
    public class Plane : SceneObject
    {
        #region Public Methods

        /// <summary>
        /// Gets the normal at a local point.
        /// </summary>
        /// <returns>The normal at local.</returns>
        /// <param name="objectPoint">Object point.</param>
        public override Vector GetNormalAtLocal(Point objectPoint)
        {
            return new Vector(0, 1, 0);
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
            if (Math.Abs(localRay.Direction.Y) < Constants.Epsilon)
            {
                return new Intersections(); // Empty set -- no intersections
            }

            double distance = -localRay.Origin.Y / localRay.Direction.Y;

            return new Intersections(new Intersection(distance, this));
        }

        /// <summary>
        /// Proof if the sceneObject is a plane
        /// </summary>
        /// <returns><c>true</c>, if equals local was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="sceneObject">Scene object.</param>
        protected override bool NearlyEqualsLocal(SceneObject sceneObject)
        {
            return sceneObject is Plane;
        }

        #endregion
    }
}
