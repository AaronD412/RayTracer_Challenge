﻿using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result of the tenth chapter.
    /// </summary>
    public class BookChapter10
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 10)");

            World world = GetWorld();
            Camera camera = GetCamera();

            // Render the result to a canvas.
            Canvas canvas = camera.Render(world);

            canvas.ToPpm("book-chapter10-patterns.ppm");
            System.Console.WriteLine("    book-chapter10-patterns.ppm successfully written.");
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        /// <returns>The world.</returns>
        public World GetWorld()
        {
            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 1, 1);
            floor.Material.Specular = 0;
            floor.Material.Pattern = new RingPattern(new Color(0.34, 0.98, 0.2), new Color(0.72, 0.25, 0.67));
            floor.Material.Pattern.Transform = Matrix.NewRotationYMatrix(System.Math.PI / 4) * Matrix.NewScalingMatrix(3, 3, 3);

            Sphere middle = new Sphere();
            middle.Transform = middle.Transform.Translate(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;
            middle.Material.Pattern = new StripePattern(Color.GetWhite(), Color.GetBlack());

            Sphere right = new Sphere();
            right.Transform = right.Transform.Scale(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;
            right.Material.Pattern = new GradientPattern(new Color(0, 1, 0), new Color(0, 0, 1));

            Sphere left = new Sphere();
            left.Transform = left.Transform.Scale(0.33, 0.33, 0.33).Translate(-1.5, 0.33, -0.75);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;
            left.Material.Pattern = new CheckersPattern(Color.GetWhite(), new Color(0, 0, 1));

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
            //world.LightSources.Add(new PointLight(new Point(10, 10, -10), Color.GetWhite()));

            world.Shapes.Add(floor);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);
            world.Shapes.Add(left);
            world.Shapes.Add(top);
            world.Shapes.Add(topTwo);

            return world;
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        /// <returns>The camera.</returns>
        public Camera GetCamera()
        {
            Camera camera = new Camera(1080, 720, System.Math.PI / 3);

            camera.Transform = new Point(0, 1.5, -5).ViewTransform(new Point(0, 1, 0), new Vector(0, 1, 0));

            return camera;
        }
    }
}
