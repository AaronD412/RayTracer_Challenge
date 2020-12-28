using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates a sample scene for the example.
    /// </summary>
    public class TermPaperChapter02_10
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("Example (term paper chapter 2.10)");

            World world = GetWorld();
            Camera camera = GetCamera();

            Canvas canvas = camera.Render(world);

            canvas.ToPpm("term-paper-chapter02-10.ppm");
            System.Console.WriteLine("    term-paper-chapter02-10.ppm successfully written.");
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        {
            Plane floor = new Plane();
            floor.Material.Reflective = 0.25;

            Sphere sphere = new Sphere();
            sphere.Material.Color = new Color(1, 0.2, 0.5);
            sphere.Transform = Matrix.NewTranslationMatrix(0, 1, 0);

            World world = new World();
            world.LightSources.Add(new PointLight(new Point(-10, 5, -10), Color.GetWhite()));

            world.SceneObjects.Add(floor);
            world.SceneObjects.Add(sphere);

            return world;
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <returns>The camera.</returns>
        public Camera GetCamera()
        {
            Camera camera = new Camera(1080, 720, System.Math.PI / 3);

            camera.Transform = new Point(0, 1.5, -5).GetViewTransform(new Point(0, 1, 0), new Vector(0, 1, 0));

            return camera;
        }
    }
}
