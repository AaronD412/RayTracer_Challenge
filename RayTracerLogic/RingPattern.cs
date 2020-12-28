using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Ring pattern.
    /// </summary>
    public class RingPattern : Pattern
    {
        #region Private Members

        /// <summary>
        /// The first color.
        /// </summary>
        private Color firstColor;

        /// <summary>
        /// The second color.
        /// </summary>
        private Color secondColor;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.RingPattern"/> class.
        /// </summary>
        /// <param name="firstColor">First color.</param>
        /// <param name="secondColor">Second color.</param>
        public RingPattern(Color firstColor, Color secondColor)
        {
            this.firstColor = firstColor;
            this.secondColor = secondColor;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the pattern at a point.
        /// </summary>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/>.</returns>
        /// <param name="point">Point.</param>
        public override Color GetPatternAt(Point point)
        {
            return (int)Math.Floor(Math.Sqrt(point.X * point.X + point.Z * point.Z)) % 2 == 0 ? firstColor : secondColor;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the first color.
        /// </summary>
        /// <value>The first color.</value>
        public Color FirstColor
        {
            get
            {
                return firstColor;
            }
        }

        /// <summary>
        /// Gets the second color.
        /// </summary>
        /// <value>The second color.</value>
        public Color SecondColor
        {
            get
            {
                return secondColor;
            }
        }

        #endregion
    }
}
