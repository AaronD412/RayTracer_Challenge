using RayTracerLogic;

namespace RayTracerConsole
{
    public class Chapter09
    {
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (chapter 9)");

            Plane floor = new Plane();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;

            Plane backdropWall = new Plane();
            backdropWall.Transform = Matrix.NewRotationXMatrix(System.Math.PI / 2)
                .Translate(0, 0, 5);
            backdropWall.Material = floor.Material;

            Plane rightWall = new Plane();
            rightWall.Transform = Matrix.NewRotationXMatrix(System.Math.PI / 2)
                .RotateY(System.Math.PI / 4)
                .Translate(0, 0, 5);
            rightWall.Material = floor.Material;

            Sphere middle = new Sphere();
            middle.Transform = Matrix.NewTranslationMatrix(-0.5, 1, 0.5);
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;

            Sphere right = new Sphere();
            right.Transform = Matrix.NewScalingMatrix(0.5, 0.5, 0.5).Translate(1.5, 0.5, -0.5);
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;

            Sphere left = new Sphere();
            left.Transform = Matrix.NewScalingMatrix(0.33, 0.33, 0.33).Translate(-1.5, 0.33, -0.75);
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;

            World world = new World();
            world.Shapes.Add(floor);
            world.Shapes.Add(backdropWall);
            world.Shapes.Add(left);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);
            world.LightSources.Add(new PointLight(new Point(-10, 10, -10), Color.GetWhite()));

            Camera camera = new Camera(800, 600, System.Math.PI / 3);
            camera.Transform = new Point(0, 1.5, -5).ViewTransform(new Point(0, 1, 0), new Vector(0, 1, 0));

            Canvas canvas = camera.Render(world);

            canvas.ToPpm("chapter09-planes.ppm");
            System.Console.WriteLine("    chapter09-planes.ppm successfully written.");
        }
    }
}
