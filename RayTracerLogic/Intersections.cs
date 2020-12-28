using System.Collections.Generic;

namespace RayTracerLogic
{
    /// <summary>
    /// A list of interssections.
    /// </summary>
    public class Intersections : List<Intersection>
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the Intersections class.
        /// Constructor needs no arguments.
        /// </summary>
        public Intersections() : base()
        {
            // Do nothing
        }

        /// <summary>
        /// Initializes a new instance of the Intersections class.
        /// Constructor needs intersections.
        /// </summary>
        /// <param name="intersections">Intersections.</param>
        public Intersections(params Intersection[] intersections) : base(intersections)
        {
            // Do nothing
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the hit.
        /// The hit with the sphere is calculated with the intersections.
        /// </summary>
        /// <returns>The hit.</returns>
        public Intersection GetHit()
        {
            Intersection hit = null;

            foreach (Intersection intersection in this)
            {
                if (intersection.Distance < 0)
                {
                    continue;
                }

                if (hit == null || intersection.Distance < hit.Distance)
                {
                    hit = intersection;
                }
            }

            return hit;
        }

        #endregion
    }
}
