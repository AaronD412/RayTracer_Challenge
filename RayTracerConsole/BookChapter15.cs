using RayTracerLogic;
using System.IO;

namespace RayTracerConsole
{
    public class BookChapter15
    {
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (chapter 15)");

            using (StreamReader teapotStreamReader = new StreamReader("teapot.obj"))
            {
                Parser parser = new Parser(teapotStreamReader);

                Plane floor = new Plane();
                floor.Material.Specular = 0;
                floor.Material.Pattern = new CheckersPattern(new Color(0.7, 0, 0), new Color(0.7, 0.7, 0.7));
                floor.Material.Reflective = 0.2;

                Group teapot = parser.Group;
                teapot.Material.Diffuse = 0.7;
                teapot.Material.Specular = 0.3;
                teapot.Material.Pattern = new StripePattern(new Color(0.5, 0.5, 0.5), new Color(0.7, 0.7, 0.7));
                teapot.Material.Pattern.Transform = Matrix.NewScalingMatrix(3, 1, 1).RotateZ(System.Math.PI / 3);
                teapot.Transform = parser
                    .GetUniformedSizedAndCenteredModelTransformationMatrix()
                    .Translate(8, -parser.LowestUniformedSizedAndCenteredY, 0);

                System.Console.WriteLine(parser.HighestX);
                System.Console.WriteLine(parser.HighestY);
                System.Console.WriteLine(parser.HighestZ);


                AreaLight areaLight = new AreaLight(
                    new Point(-10, 5, -10),
                    new Vector(2, 0, 0),
                    4,
                    new Vector(0, 2, 0),
                    4,
                    Color.GetWhite()
                );
                areaLight.JitterBy = new Sequence(0.7, 0.3, 0.9, 0.1, 0.5);

                World world = new World();
                world.Shapes.Add(floor);
                world.Shapes.Add(teapot);
                //world.LightSources.Add(new PointLight(new Point(-10, 5, -10), Color.GetWhite()));
                world.LightSources.Add(areaLight);

                Camera camera = new Camera(720, 480, System.Math.PI / 3);
                camera.Transform = new Point(0, 1.5, -5).ViewTransform(new Point(0, 1, 0), new Vector(0, 1, 0));

                Canvas canvas = camera.Render(world);

                canvas.ToPpm("chapter15-triangles.ppm");
                System.Console.WriteLine("    chapter15-triangles.ppm successfully written.");
            }
        }
    }
}
