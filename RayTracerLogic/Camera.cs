using System.Threading.Tasks;

namespace RayTracerLogic
{
    public class Camera
    {
        #region Private Members

        private readonly int horizontalSize;
        private readonly int verticalSize;
        private readonly double fieldOfView;
        private Matrix transformationMatrix;
        private double halfWidth;
        private double halfHeight;

        #endregion

        #region Public Constructors

        public Camera(int horizontalSize, int verticalSize, double fieldOfView)
        {
            this.horizontalSize = horizontalSize;
            this.verticalSize = verticalSize;
            this.fieldOfView = fieldOfView;
            transformationMatrix = Matrix.NewIdentityMatrix(4);
            transformationMatrix.PrecomputeInverse = true;
        }

        #endregion

        #region Public Methods

        public double GetPixelSize()
        {
            double halfView = System.Math.Tan(fieldOfView / 2);
            double aspect = (double)horizontalSize / verticalSize;

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

            return (halfWidth * 2) / horizontalSize;
        }

        public Ray GetRayForPixel(int x, int y)
        {
            double pixelSize = GetPixelSize();
            double xOffset = (x + 0.5) * pixelSize;
            double yOffset = (y + 0.5) * pixelSize;

            double worldX = halfWidth - xOffset;
            double worldY = halfHeight - yOffset;

            Matrix inversedTransform = transformationMatrix.GetInverse();

            Point pixel = inversedTransform * new Point(worldX, worldY, -1);
            Point origin = inversedTransform * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new Ray(origin, direction);
        }

        public Canvas Render(World world)
        {
            Canvas image = new Canvas(horizontalSize, verticalSize);

            //Parallel.For(0, verticalSize, y =>
            for (int y = 0; y < verticalSize; y++)
            {
                System.Console.WriteLine("    Rendering line " + (y + 1));

                for (int x = 0; x < horizontalSize; x++)
                {
                    Ray ray = GetRayForPixel(x, y);
                    Color color = world.ColorAt(ray);

                    image[x, y] = color;
                }
            }
            //});

            return image;
        }

        #endregion

        #region Public Properties

        public int HorizontalSize
        {
            get
            {
                return horizontalSize;
            }
        }

        public int VerticalSize
        {
            get
            {
                return verticalSize;
            }
        }

        public double FieldOfView
        {
            get
            {
                return fieldOfView;
            }
        }

        public Matrix Transform
        {
            get
            {
                return transformationMatrix;
            }
            set
            {
                transformationMatrix = value;
                transformationMatrix.PrecomputeInverse = true;
            }
        }

        public double HalfWidth
        {
            get
            {
                return halfWidth;
            }
        }

        public double HalfHeight
        {
            get
            {
                return halfHeight;
            }
        }

        #endregion
    }
}