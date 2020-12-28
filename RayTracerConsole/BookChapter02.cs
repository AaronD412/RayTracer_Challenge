using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result of the second chapter.
    /// </summary>
    public class BookChapter02
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 2)");

            Color color = new Color(255, 255, 255);
            Point start = new Point(0, 1, 0);
            Vector velocity = new Vector(1, 1.8, 0).Normalize().Multiply(11.25);
            Projectile projectile = new Projectile(start, velocity);
            Vector gravity = new Vector(0, -0.1, 0);
            Vector wind = new Vector(-0.01, 0, 0);
            Environment environment = new Environment(gravity, wind);
            Canvas canvas = new Canvas(900, 550);

            while (projectile.Position.Y > 0)
            {
                projectile = Tick(environment, projectile);

                // Convert the coordinates to integers
                int x = (int)projectile.Position.X;
                int y = (int)projectile.Position.Y;

                // Convert the world y coordinate to the canvas y coordinate
                y = canvas.Height - y;

                // Make sure we do not draw outside the bounds of the canvas
                if (x >= 0 &&
                    x < canvas.Width &&
                    y >= 0 &&
                    y < canvas.Height)
                {
                    canvas[x, y] = color;
                }
            }

            canvas.ToPpm("book-chapter02-canvas.ppm");
            System.Console.WriteLine("    book-chapter02-canvas.ppm successfully written.");
        }

        /// <summary>
        /// Tick the specified environment and projectile.
        /// </summary>
        /// <returns>The tick.</returns>
        /// <param name="environment">Environment.</param>
        /// <param name="projectile">Projectile.</param>
        private Projectile Tick(Environment environment, Projectile projectile)
        {
            Point position = projectile.Position + projectile.Velocity;
            Vector velocity = projectile.Velocity + environment.Gravity + environment.Wind;

            return new Projectile(position, velocity);
        }
    }
}
