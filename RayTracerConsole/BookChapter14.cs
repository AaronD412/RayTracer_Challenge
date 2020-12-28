using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result oft the eleventh chapter.
    /// </summary>
    public class BookChapter14
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 11)");

            World world = GetWorld();
            Camera camera = GetCamera();

            // Render the result to a canvas.
            Canvas canvas = camera.Render(world);

            canvas.ToPpm("book-chapter11-reflection-and-refraction.ppm");
            System.Console.WriteLine("    book-chapter11-reflection-and-refraction.ppm successfully written.");
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        {
            public Shape HexagonCorner()
            {
                Sphere corner = new Sphere();
                corner.Transform = Matrix.NewTranslationMatrix()
            }

            return world;
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <returns>The camera.</returns>
        public Camera GetCamera()
        {
            Camera camera = new Camera(800, 600, System.Math.PI / 3);

            camera.Transform = new Point(0, 1.5, -5).GetViewTransform(new Point(0, 1, 0), new Vector(0, 1, 0));

            return camera;
        }
    }
}
