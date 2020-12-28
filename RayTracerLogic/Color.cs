namespace RayTracerLogic
{
    /// <summary>
    /// Represents a color.
    /// </summary>
    public class Color
    {
        #region Private Members

        /// <summary>
        /// The array holding the 3 color components.
        /// </summary>
        private readonly double[] values;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.Color"/> class.
        /// </summary>
        /// <param name="red">The red color component.</param>
        /// <param name="green">The green color component.</param>
        /// <param name="blue">The blue color component.</param>
        public Color(double red, double green, double blue)
        {
            values = new double[] { red, green, blue };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the current to the given color.
        /// </summary>
        /// <returns>The sum of the current and the given color.</returns>
        /// <param name="color">The color to add.</param>
        public Color Add(Color color)
        {
            return new Color(Red + color.Red, Green + color.Green, Blue + color.Blue);
        }

        /// <summary>
        /// Subtracts the given from the current color.
        /// </summary>
        /// <returns>The difference of the current and the given color.</returns>
        /// <param name="color">The color to subtract.</param>
        public Color Subtract(Color color)
        {
            return new Color(Red - color.Red, Green - color.Green, Blue - color.Blue);
        }

        /// <summary>
        /// Multiplies the current color with the given scalar.
        /// </summary>
        /// <returns>The product of the current color and the given scalar.</returns>
        /// <param name="scalar">The scalar to multiply.</param>
        public Color Scalar(double scalar)
        {
            return new Color(Red * scalar, Green * scalar, Blue * scalar);
        }

        /// <summary>
        /// Multiplies the current with the given color.
        /// </summary>
        /// <returns>The Hadamard product of the current and given color.</returns>
        /// <param name="color">The color to multiply.</param>
        public Color Multiply(Color color)
        {
            return new Color(Red * color.Red, Green * color.Green, Blue * color.Blue);
        }

        /// <summary>
        /// Compares the current and the given colors.
        /// </summary>
        /// <returns><c>true</c>, if the current and the given colors are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="color">The color to compare.</param>
        public bool NearlyEquals(Color color)
        {
            return Red.NearlyEquals(color.Red) && Green.NearlyEquals(color.Green) && Blue.NearlyEquals(color.Blue);
        }

        /// <summary>
        /// Gets the black color.
        /// </summary>
        /// <returns>The black.</returns>
        public static Color GetBlack()
        {
            return new Color(0, 0, 0);
        }

        /// <summary>
        /// Gets the white color.
        /// </summary>
        /// <returns>The white.</returns>
        public static Color GetWhite()
        {
            return new Color(1, 1, 1);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the red color component.
        /// </summary>
        /// <value>The red color component.</value>
        public double Red
        {
            get
            {
                return values[0];
            }
        }

        /// <summary>
        /// Gets the green color component.
        /// </summary>
        /// <value>The green color component.</value>
        public double Green
        {
            get
            {
                return values[1];
            }
        }

        /// <summary>
        /// Gets the blue color component.
        /// </summary>
        /// <value>The blue color component.</value>
        public double Blue
        {
            get
            {
                return values[2];
            }
        }

        /// <summary>
        /// Gets the color component at the specified index.
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

        #region Public Operators

        /// <summary>
        /// Adds a color to another color.
        /// </summary>
        /// <param name="color1">The first <see cref="RayTracerLogic.Color"/> to add.</param>
        /// <param name="color2">The second <see cref="RayTracerLogic.Color"/> to add.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/> that is the sum of the values of <c>color1</c> and <c>color2</c>.</returns>
        public static Color operator +(Color color1, Color color2)
        {
            return color1.Add(color2);
        }

        /// <summary>
        /// Subtracts a color from another color.
        /// </summary>
        /// <param name="color1">The <see cref="RayTracerLogic.Color"/> to subtract from (the minuend).</param>
        /// <param name="color2">The <see cref="RayTracerLogic.Color"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/> that is the <c>color1</c> minus <c>color2</c>.</returns>
        public static Color operator -(Color color1, Color color2)
        {
            return color1.Subtract(color2);
        }

        /// <summary>
        /// Computes the product of color and scalar.
        /// </summary>
        /// <param name="color">The <see cref="RayTracerLogic.Color"/> to multiply.</param>
        /// <param name="scalar">The <see cref="double"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/> that is the <c>color</c> * <c>scalar</c>.</returns>
        public static Color operator *(Color color, double scalar)
        {
            return color.Scalar(scalar);
        }

        /// <summary>
        /// Computes the product of color1 and color2.
        /// </summary>
        /// <param name="color1">The <see cref="RayTracerLogic.Color"/> to multiply.</param>
        /// <param name="color2">The <see cref="RayTracerLogic.Color"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Color"/> that is the <c>color1</c> * <c>color2</c>.</returns>
        public static Color operator *(Color color1, Color color2)
        {
            return color1.Multiply(color2);
        }

        #endregion
    }
}
