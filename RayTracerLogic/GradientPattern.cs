using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the gradient pattern.
    /// </summary>
    public class GradientPattern : Pattern
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

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the GradientPattern class.
        /// </summary>
        /// <param name="firstColor">First color.</param>
        /// <param name="secondColor">Second color.</param>
        public GradientPattern(Color firstColor, Color secondColor)
        {
            this.firstColor = firstColor;
            this.secondColor = secondColor;
        }

        #endregion

        #region Public Methodes

        /// <summary>
        /// Gets the pattern at a point.
        /// </summary>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/>.</returns>
        /// <param name="point">The point.</param>
        public override Color GetPatternAt(Point point)
        {
            Color distance = secondColor - firstColor;

            double fraction = point.X - Math.Floor(point.X);

            return firstColor + distance * fraction;
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
