namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Patterns.
    /// </summary>
    public abstract class Pattern
    {
        #region Private Members

        /// <summary>
        /// The transform.
        /// </summary>
        private Matrix transform = Matrix.NewIdentityMatrix(4);

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the pattern at an object.
        /// </summary>
        /// <returns>The pattern at object.</returns>
        /// <param name="sceneObject">Scene object.</param>
        /// <param name="worldPoint">World point.</param>
        public Color GetPatternAtObject(SceneObject sceneObject, Point worldPoint)
        {
            Point objectPoint = sceneObject.Transform.GetInverse() * worldPoint;
            Point patternPoint = this.Transform.GetInverse() * objectPoint;

            return GetPatternAt(patternPoint);
        }

        /// <summary>
        /// Gets the pattern at point.
        /// </summary>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/>.</returns>
        /// <param name="point">Point.</param>
        public abstract Color GetPatternAt(Point point);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        /// <value>The transform.</value>
        public Matrix Transform
        {
            get
            {
                return transform;
            }
            set
            {
                transform = value;
            }
        }

        #endregion
    }
}
