namespace RayTracerLogic
{
    /// <summary>
    /// Represents the PointLight, which is sent through the spheres.
    /// </summary>
    public class PointLight
    {
        #region Private Members

        /// <summary>
        /// The position.
        /// </summary>
        private Point position;

        /// <summary>
        /// The intensity.
        /// </summary>
        private Color intensity;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.PointLight"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="intensity">Intensity.</param>
        public PointLight(Point position, Color intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Proofs if the light in the scene is nearly the pointlight.
        /// </summary>
        /// <returns><c>true</c>, if equals was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="light">Light.</param>
        public bool NearlyEquals(PointLight light)
        {
            return position.NearlyEquals(light.Position) &&
                intensity.NearlyEquals(light.Intensity);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Gets the intensity.
        /// </summary>
        /// <value>The intensity.</value>
        public Color Intensity
        {
            get
            {
                return intensity;
            }
        }

        #endregion
    }
}
