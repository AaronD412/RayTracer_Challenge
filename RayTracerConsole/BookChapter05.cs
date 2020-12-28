using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result of the fifth chapter.
    /// </summary>
    public class BookChapter05
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 5)");

            Point origin = new Point(0, 0, -5);
            Canvas canvas = new Canvas(500, 500);

            double worldZ = 10;
            double wallSize = 7.0;
            double pixelSizeX = wallSize / canvas.Width;
            double pixelSizeY = wallSize / canvas.Height;
            double half = wallSize / 2;

            Color color = new Color(1, 0.7, 0);
            Sphere shape = new Sphere();

            // For each row of pixels in the canvas...
            for (int y = 0; y < canvas.Height - 1; y++)
            {
                // ... compute the world y coordinate (top = +half, bottom = -half)
                double worldY = half - pixelSizeY * y;

                // For each pixel in the row...
                for (int x = 0; x < canvas.Width; x++)
                {
                    // ... compute the world x coordinate (left = -half, right = +half)
                    double worldX = -half + pixelSizeX * x;

                    // Describe the point on the wall that the ray will target.
                    Point position = new Point(worldX, worldY, worldZ);
                    Ray ray = new Ray(origin, (position - origin).Normalize());
                    Intersections intersections = shape.GetIntersections(ray);

                    if (intersections.GetHit() != null)
                    {
                        canvas[x, y] = color;
                    }
                }
            }

            canvas.ToPpm("book-chapter05-ray-sphere-intersections.ppm");
            System.Console.WriteLine("    book-chapter05-ray-sphere-intersections.ppm successfully written.");
        }
    }
}
