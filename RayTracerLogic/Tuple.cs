namespace RayTracerLogic
{
    /// <summary>
    /// Represents a 4-dimensional tuple.
    /// </summary>
    public abstract class Tuple
    {
        #region Protected Members

        /// <summary>
        /// The array holding the 4 dimensions.
        /// </summary>
        protected double[] values;

        #endregion

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.Tuple"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="w">The w coordinate.</param>
        protected Tuple(double x, double y, double z, double w)
        {
            values = new double[] { x, y, z, w };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        /// <value>The x coordinate.</value>
        public double X
        {
            get
            {
                return values[0];
            }
        }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        /// <value>The y coordinate.</value>
        public double Y
        {
            get
            {
                return values[1];
            }
        }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        /// <value>The z coordinate.</value>
        public double Z
        {
            get
            {
                return values[2];
            }
        }

        /// <summary>
        /// Gets the w coordinate.
        /// </summary>
        /// <value>The w coordinate.</value>
        public double W
        {
            get
            {
                return values[3];
            }
        }

        /// <summary>
        /// Gets the coordinate at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public double this[int index]
        {
            get
            {
                return values[index];
            }
        }

        #endregion
    }
}
