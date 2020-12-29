using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result oft the fourteenth chapter.
    /// </summary>
    public class BookChapter14
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 14)");

            World world = GetWorld();
            Camera camera = GetCamera();

            // Render the result to a canvas.
            Canvas canvas = camera.Render(world);

            canvas.ToPpm("book-chapter14-groups.ppm");
            System.Console.WriteLine("    book-chapter14-groups.ppm successfully written.");
        }

        public Shape HexagonCorner()
        {
            Sphere corner = new Sphere
            {
                Transform = (Matrix.NewTranslationMatrix(0, 0, -1) * Matrix.NewScalingMatrix(0.25, 0.25, 0.25))
            };

            return corner;
        }

        public Shape HexagonEdge()
        {
            Cylinder edge = new Cylinder
            {
                Minimum = 0,
                Maximum = 1,

                Transform = (Matrix.NewTranslationMatrix(0, 0, -1) *
                            Matrix.NewRotationYMatrix(-System.Math.PI / 6) *
                            Matrix.NewRotationZMatrix(-System.Math.PI / 2) *
                            Matrix.NewScalingMatrix(0.25, 1, 0.25))
            };

            return edge;
        }

        public Shape HexagonSide()
        {
            Group side = new Group();

            side.AddChild(HexagonCorner());
            side.AddChild(HexagonEdge());

            return side;
        }

        public Shape Hexagon()
        {
            Group hex = new Group();

            for (int i = 0; i < 6; i++)
            {
                Shape side = HexagonSide();
                side.Transform = Matrix.NewRotationYMatrix(i * (System.Math.PI/3));

                hex.AddChild(side);
            }

            return hex;
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        {
            World world = new World();
            world.LightSources.Add(new PointLight(new Point(-10, 10, -10), Color.GetWhite()));
            world.LightSources.Add(new PointLight(new Point(10, 5, 20), Color.GetWhite()));

            world.Shapes.Add(Hexagon());

            return world;
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <returns>The camera.</returns>
        public Camera GetCamera()
        {
            Camera camera = new Camera(480, 320, System.Math.PI / 3)
            {
                Transform = new Point(0, 1.5, -5).ViewTransform(new Point(0, 1, 0), new Vector(0, 1, 0))
            };

            return camera;
        }
    }
}
