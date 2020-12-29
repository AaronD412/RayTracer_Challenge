using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result of the sixth chapter.
    /// </summary>
    public class BookChapter06
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 6)");

            Point origin = new Point(0, 0, -5);
            Canvas canvas = new Canvas(500, 500);

            double worldZ = 10;
            double wallSize = 7.0;
            double pixelSizeX = wallSize / canvas.Width;
            double pixelSizeY = wallSize / canvas.Height;
            double half = wallSize / 2;

            Sphere sphere = new Sphere();
            sphere.Material.Color = new Color(1, 0.7, 0);

            Point lightPosition = new Point(-10, 10, -10);
            Color lightColor = new Color(1, 1, 1);
            PointLight light = new PointLight(lightPosition, lightColor);

            for (int y = 0; y < canvas.Height - 1; y++)
            {
                double worldY = half - pixelSizeY * y;

                for (int x = 0; x < canvas.Width; x++)
                {
                    double worldX = -half + pixelSizeX * x;
                    Point position = new Point(worldX, worldY, worldZ);
                    Ray ray = new Ray(origin, (position - origin).Normalize());
                    Intersections intersections = sphere.GetIntersections(ray);

                    Intersection hit = intersections.GetHit();

                    if (hit != null)
                    {
                        Point point = ray.GetPosition(hit.Distance);
                        Vector normal = hit.Shape.GetNormalAt(point);
                        Vector eye = -ray.Direction;

                        Color color = hit.Shape.Material.GetLighting(sphere, light, point, eye, normal, 1.0);

                        canvas[x, y] = color;
                    }
                }
            }

            canvas.ToPpm("book-chapter06-light-and-shading.ppm");
            System.Console.WriteLine("    book-chapter06-light-and-shading.ppm successfully written.");
        }
    }
}
