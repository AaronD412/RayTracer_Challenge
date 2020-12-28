using System.IO;
using System.Text;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a canvas which can be used to draw pixels and which can be
    /// saved to a file.
    /// </summary>
    public class Canvas
    {
        #region Private Constants

        /// <summary>
        /// The number of bytes per pixel.
        /// </summary>
        private const int BytesPerPixel = 3;

        #endregion

        #region Private Members

        /// <summary>
        /// The two-dimensional <see cref="RayTracerLogic.Color"/> array.
        /// </summary>
        private Color[,] colors;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.Canvas"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Canvas(int width, int height)
        {
            colors = new Color[height, width];

            // Initialize the array with black pixels
            Color black = Color.GetBlack();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colors[y, x] = black;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the <see cref="RayTracerLogic.Canvas"/> to a Portable Pixmap (PPM) string.
        /// </summary>
        /// <returns>The Portable Pixmap (PPM) as a string.</returns>
        public string ToPpm()
        {
            string ppmHeader = "P3\n" + Width + " " + Height + "\n255\n";
            StringBuilder ppmBody = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                StringBuilder ppmRow = new StringBuilder();

                for (int x = 0; x < Width; x++)
                {
                    Color color = this[x, y];

                    for (int index = 0; index < 3; index++)
                    {
                        string colorComponentToAdd = AdjustColorComponent(color[index]).ToString();

                        if (ppmRow.Length + colorComponentToAdd.Length > 70)
                        {
                            // Replace the last blank with a newline character
                            ppmRow[ppmRow.Length - 1] = '\n';
                            ppmBody.Append(ppmRow.ToString());
                            ppmRow = new StringBuilder();
                        }

                        ppmRow.Append(colorComponentToAdd + " ");
                    }
                }

                // Replace the last blank with a newline character
                ppmRow[ppmRow.Length - 1] = '\n';
                ppmBody.Append(ppmRow.ToString());
            }

            return ppmHeader + ppmBody;
        }

        /// <summary>
        /// Writes the <see cref="RayTracerLogic.Canvas"/> to a Portable Pixmap (PPM) to the specified file.
        /// </summary>
        /// <param name="filePath">The file path to write the Portable Pixmap (PPM) to.</param>
        public void ToPpm(string filePath)
        {
            using (var writer = File.CreateText(filePath))
            {
                writer.Write(ToPpm());
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adjusts the color component to range between 0 and 255 (incl.).
        /// </summary>
        /// <param name="colorComponent">The color component to adjust.</param>
        /// <returns>The adjusted color component.</returns>
        private int AdjustColorComponent(double colorComponent)
        {
            double adjustedColorComponent = colorComponent;

            if (colorComponent < 0)
            {
                adjustedColorComponent = 0;
            }
            else if (colorComponent > 1)
            {
                adjustedColorComponent = 1;
            }

            return (int)System.Math.Round(adjustedColorComponent * 255);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return colors.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return colors.GetLength(0);
            }
        }

        /// <summary>
        /// Gets the <see cref="RayTracerLogic.Color"/> at position (x, y).
        /// </summary>
        /// <returns>The <see cref="RayTracerLogic.Color"/> at position (x, y).</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Color this[int x, int y]
        {
            get
            {
                return colors[y, x];
            }
            set
            {
                colors[y, x] = value;
            }
        }

        #endregion
    }
}
