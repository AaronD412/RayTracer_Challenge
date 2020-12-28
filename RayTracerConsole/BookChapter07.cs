using RayTracerLogic;

namespace RayTracerConsole
{
    public class BookChapter07
    {
        /// <summary>
        /// Creates the result of the seventh chapter.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 7)");

            World world = GetWorld();
            Camera camera = GetCamera();

            // Render the result to a canvas.
            Canvas canvas = camera.Render(world);

            canvas.ToPpm("book-chapter07-making-a-scene.ppm");
            System.Console.WriteLine("    book-chapter07-making-a-scene.ppm successfully written.");
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        {
            Sphere floor = new Sphere();
            floor.Transform = floor.Transform.Scale(10, 0.01, 10);
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;

            Sphere leftWall = new Sphere();
            leftWall.Transform = leftWall.Transform.Scale(10, 0.01, 10).RotateX(System.Math.PI / 2).RotateY(-System.Math.PI / 4).Translate(0, 0, 5);
            //leftWall.Transform = leftWall.Transform.Translate(0, 0, 5).RotateY(-System.Math.PI / 4).RotateX(System.Math.PI / 2).Scale(10, 0.01, 10);
            leftWall.Material = floor.Material;

            Sphere rightWall = new Sphere();
            rightWall.Transform = rightWall.Transform.Scale(10, 0.01, 10).RotateX(System.Math.PI / 2).RotateY(System.Math.PI / 4).Translate(0, 0, 5);
            rightWall.Material = floor.Material;

            Sphere middle = new Sphere();
            middle.Transform = middle.Transform.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;

            Sphere right = new Sphere();
            right.Transform = right.Transform.Scale(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;

            Sphere left = new Sphere();
            left.Transform = left.Transform.Scale(0.33, 0.33, 0.33).Translate(-1.5, 0.33, -0.75);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;

            Sphere top = new Sphere();
            top.Transform = top.Transform.Translate(0.5, 4, 0.5);
            top.Transform = top.Transform.Scale(0.5, 0.5, 0.5);
            top.Material.Color = new Color(0.1, 1, 0.5);

            Sphere topTwo = new Sphere();
            topTwo.Transform = topTwo.Transform.Translate(-2.5, 4, 0.5);
            topTwo.Transform = topTwo.Transform.Scale(0.5, 0.5, 0.5);
            topTwo.Material.Color = new Color(0.1, 1, 0.5);

            World world = new World();
            world.LightSources.Add(new PointLight(new Point(-10, 10, -10), Color.GetWhite()));

            world.Shape.Add(floor);
            world.Shape.Add(leftWall);
            world.Shape.Add(rightWall);
            world.Shape.Add(middle);
            world.Shape.Add(right);
            world.Shape.Add(left);
            world.Shape.Add(top);
            world.Shape.Add(topTwo);

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
