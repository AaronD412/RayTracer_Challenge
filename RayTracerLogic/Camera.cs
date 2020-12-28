using System;
using System.Threading.Tasks;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Ray, which is sent through the spheres.
    /// </summary>
    public class Camera
    {
        #region Private Members

        /// <summary>
        /// The canvas.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// The field of view.
        /// </summary>
        private double fieldOfView;

        /// <summary>
        /// The transformation matrix.
        /// </summary>
        private Matrix transform = Matrix.NewIdentityMatrix(4);

        /// <summary>
        /// The width of the half.
        /// </summary>
        private double halfWidth;

        /// <summary>
        /// The height of the half.
        /// </summary>
        private double halfHeight;

        /// <summary>
        /// The size of a pixel.
        /// </summary>
        private double pixelSize;
       
        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the camera class.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="fieldOfView">Field of view.</param>
        public Camera(int width, int height, double fieldOfView)
        {
            canvas = new Canvas(width, height);

            this.fieldOfView = fieldOfView;

            double halfView = Math.Tan(fieldOfView / 2);

            double aspect = width / (double)height;

            if (aspect >= 1)
            {
                halfWidth = halfView;
                halfHeight = halfView / aspect;
            }
            else
            {
                halfWidth = halfView * aspect;
                halfHeight = halfView;
            }

            pixelSize = (halfWidth * 2) / width;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the ray for the given pixel coordinates.
        /// </summary>
        /// <returns>The ray for the given pixel coordinates.</returns>
        /// <param name="pixelX">Pixel x.</param>
        /// <param name="pixelY">Pixel y.</param>
        public Ray GetRayForPixel(int pixelX, int pixelY)
        {
            // The offset from the edge of the canvas to the pixel's center
            double xOffset = (pixelX + 0.5) * pixelSize;
            double yOffset = (pixelY + 0.5) * pixelSize;

            // The untransformed coordinates of the pixel in world space
            // (remember that the camera looks toward -z, so +x is to the *left*).
            double worldX = halfWidth - xOffset;
            double worldY = halfHeight - yOffset;

            // Using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's direction vector
            // (remember that the canvas is at z = -1).
            Matrix inverse = transform.GetInverse();

            Point pixel = inverse * new Point(worldX, worldY, -1);
            Point origin = inverse * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }

        /// <summary>
        /// Renders the specified world.
        /// </summary>
        /// <returns>The render world as a canvas.</returns>
        /// <param name="world">The world.</param>
        public Canvas Render(World world)
        {
            Console.CursorVisible = false;

            for (int y = 0; y < canvas.Height; y++)
            {
                // Show a progress bar
                int percentage = y * 100 / canvas.Height;

                Console.CursorLeft = 0;
                Console.Write("    " + new string('█', percentage / 2));
                Console.Write(new string('.', 50 - percentage / 2));

                for (int x = 0; x < canvas.Width; x++)
                {
                    Ray ray = GetRayForPixel(x, y);
                    Color color = world.GetColorAt(ray);
                    canvas[x, y] = color;
                }
            }

            Console.WriteLine();
            Console.CursorVisible = true;

            return canvas;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the canvas.
        /// </summary>
        /// <value>The canvas.</value>
        public Canvas Canvas
        {
            get
            {
                return canvas;
            }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return canvas.Width;
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
                return canvas.Height;
            }
        }

        /// <summary>
        /// Gets the field of view.
        /// </summary>
        /// <value>The field of view.</value>
        public double FieldOfView
        {
            get
            {
                return fieldOfView;
            }
        }

        /// <summary>
        /// Gets or sets the transformation matrix.
        /// </summary>
        /// <value>The transformation matrix.</value>
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

        /// <summary>
        /// Gets the width of the half.
        /// </summary>
        /// <value>The width of the half.</value>
        public double HalfWidth
        {
            get
            {
                return halfWidth;
            }
        }

        /// <summary>
        /// Gets the height of the half.
        /// </summary>
        /// <value>The height of the half.</value>
        public double HalfHeight
        {
            get
            {
                return halfHeight;
            }
        }

        /// <summary>
        /// Gets the size of a pixel.
        /// </summary>
        /// <value>The size of a pixel.</value>
        public double PixelSize
        {
            get
            {
                return pixelSize;
            }
        }

        #endregion
    }
}