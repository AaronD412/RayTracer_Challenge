using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a sphere at the Point(0, 0, 0) with a radius of 1
    /// </summary>
    public class Plane : Shape
    {
        #region Public Methods

        /// <summary>
        /// Gets the normal at a local point.
        /// </summary>
        /// <returns>The normal at local.</returns>
        /// <param name="objectPoint">Object point.</param>
        public override Vector GetNormalAtLocal(Point objectPoint, Intersection hit = null)
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
        public override Intersections GetIntersectionsLocal(Ray localRay)
        {
            if (Math.Abs(localRay.Direction.Y) < Constants.Epsilon)
            {
                return new Intersections(); // Empty set -- no intersections
            }

            double distance = -localRay.Origin.Y / localRay.Direction.Y;

            return new Intersections(new Intersection(distance, this));
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Point(double.NegativeInfinity, 0, double.NegativeInfinity),
                new Point(double.PositiveInfinity, 0, double.PositiveInfinity)
            );
        }

        /// <summary>
        /// Proof if the sceneObject is a plane
        /// </summary>
        /// <returns><c>true</c>, if equals local was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="sceneObject">Scene object.</param>
        protected override bool NearlyEqualsLocal(Shape shape)
        {
            return true;
        }

        #endregion
    }
}
