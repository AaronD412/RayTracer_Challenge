using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates a sample scene for the example.
    /// </summary>
    public class TermPaperChapter07_07
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("Example (term paper chapter 7.7)");

            World world = GetWorld();
            Camera camera = GetCamera();

            Canvas canvas = camera.Render(world);

            canvas.ToPpm("term-paper-chapter07-07.ppm");
            System.Console.WriteLine("    term-paper-chapter07-07.ppm successfully written.");
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        {
            Plane floor = new Plane();
            floor.Material.Specular = 0;
            floor.Material.Color = new Color(0.34, 0.1, 0.2);

            Sphere middle = new Sphere();
            middle.Transform = middle.Transform.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(1, 0.7, 0);

            Sphere right = new Sphere();
            right.Transform = right.Transform.Scale(0.5, 0.25, 0.25).RotateY(System.Math.PI * 0.33).Translate(1.5, 0.25, -0.5);
            right.Material.Color = new Color(0.5, 0, 0.5);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;

            Sphere left = new Sphere();
            left.Transform = left.Transform.Scale(0.5, 0.5, 0.5).Translate(-1.67, 0.5, -0.67);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            left.Material.Color = Color.GetWhite();

            World world = new World();
            world.LightSources.Add(new PointLight(new Point(-10, 10, -10), Color.GetWhite()));
            world.LightSources.Add(new PointLight(new Point(10, 5, 20), Color.GetWhite()));

            world.SceneObjects.Add(floor);
            world.SceneObjects.Add(middle);
            world.SceneObjects.Add(right);
            world.SceneObjects.Add(left);

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
