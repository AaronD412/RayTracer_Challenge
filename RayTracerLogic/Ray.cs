namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Ray, which is sent through the spheres.
    /// </summary>
    public class Ray
    {
        #region Private Members
       
        /// <summary>
        /// The origin.
        /// </summary>
        private Point origin;

        /// <summary>
        /// The direction.
        /// </summary>
        private Vector direction;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.Ray"/> class.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="direction">Direction.</param>
        public Ray(Point origin, Vector direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the origin.
        /// </summary>
        /// <value>The origin.</value>
        public Point Origin
        {
            get
            {
                return origin;
            }
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>The direction.</value>
        public Vector Direction
        {
            get
            {
                return direction;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the position, where the ray is.
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name="distance">Distance.</param>
        public Point GetPosition(double distance)
        {
            return Origin + (Direction * distance);
        }

        /// <summary>
        /// Transform the specified transformationMatrix.
        /// The Ray transforms with the sphere.
        /// </summary>
        /// <returns>The transform.</returns>
        /// <param name="transformationMatrix">Transformation matrix.</param>
        public Ray Transform(Matrix transformationMatrix)
        {
            Point transformedOrigin = transformationMatrix * Origin;

            Vector transformedDirection = transformationMatrix * Direction;

            return new Ray(transformedOrigin, transformedDirection);
        }

        #endregion
    }
}
