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
            Plane floor = new Plane();
            floor.Material.Specular = 0;
            floor.Material.Pattern = new RingPattern(new Color(0.34, 0.1, 0.2), new Color(0.72, 0.25, 0.67));
            floor.Material.Pattern.Transform = Matrix.NewRotationYMatrix(System.Math.PI / 4) * Matrix.NewScalingMatrix(3, 3, 3);
            floor.Material.Reflective = 0.2;

            Sphere middle = new Sphere();
            middle.Transform = middle.Transform.Translate(-0.5, 1, 0.5);
            middle.Material.Color = Color.GetBlack();
            middle.Material.Transparency = 0.7;
            middle.Material.Reflective = 0.7;

            Sphere right = new Sphere();
            right.Transform = right.Transform.Scale(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            right.Material.Pattern = new GradientPattern(new Color(1, 0, 0), new Color(0, 0, 1));
            right.Material.Pattern.Transform = Matrix.NewScalingMatrix(2, 2, 2) * Matrix.NewTranslationMatrix(1.5, 0.5, -0.5);

            Sphere left = Sphere.NewGlassSphere();
            left.Transform = left.Transform.Scale(0.33, 0.33, 0.33).Translate(-1.5, 0.33, -0.75);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;

            World world = new World();
            world.LightSources.Add(new PointLight(new Point(-10, 10, -10), Color.GetWhite()));
            world.LightSources.Add(new PointLight(new Point(10, 5, 20), Color.GetWhite()));

            world.Shape.Add(floor);
            world.Shape.Add(middle);
            world.Shape.Add(right);
            world.Shape.Add(left);

            //Plane floor = new Plane();

            //Sphere sphere = new Sphere();
            //sphere.Material.Color = new Color(1, 0.2, 0.5);
            //sphere.Material.Pattern = new CheckersPattern(Color.GetBlack(), Color.GetWhite());
            //sphere.Material.Pattern.Transform = Matrix.NewScalingMatrix(0.1, 0.1, 0.1);
            //sphere.Transform = Matrix.NewTranslationMatrix(0, 1, 0);
            //sphere.Material.Transparency = 0.9;
            //World world = new World();
            //world.LightSources.Add(new PointLight(new Point(-10, 5, -10), Color.GetWhite()));

            //world.SceneObjects.Add(floor);
            //world.SceneObjects.Add(sphere);

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
