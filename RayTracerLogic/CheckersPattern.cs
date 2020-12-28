using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the checkers pattern.
    /// </summary>
    public class CheckersPattern : Pattern
    {
        #region Private Members

        /// <summary>
        /// The first color.
        /// </summary>
        private Color firstColor;

        /// <summary>
        /// The color of the second.
        /// </summary>
        private Color secondColor;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the CheckersPattern class.
        /// </summary>
        /// <param name="firstColor">First color.</param>
        /// <param name="secondColor">Second color.</param>
        public CheckersPattern(Color firstColor, Color secondColor)
        {
            this.firstColor = firstColor;
            this.secondColor = secondColor;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the pattern at point.
        /// </summary>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/>.</returns>
        /// <param name="point">Point.</param>
        public override Color GetPatternAt(Point point)
        {
            return (int)(Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z)) % 2 == 0 ? firstColor : secondColor;
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
        /// Gets the color of the second.
        /// </summary>
        /// <value>The color of the second.</value>
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
