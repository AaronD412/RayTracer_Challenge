using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Creates the result of the first chapter.
    /// </summary>
    public class BookChapter01
    {
        /// <summary>
        /// Run this instance.
        /// </summary>
        public void Run()
        {
            System.Console.WriteLine("'Putting it together' example (book chapter 1)");

            // Projectile starts one unit above the origin.
            // Velocity is normalized to 1 unit/tick.
            Projectile projectile = new Projectile(new Point(0, 1, 0), new Vector(1, 1, 0).Normalize());

            // Gravity -0.1 unit/tick, and wind is -0.01 unit/tick.
            Environment environment = new Environment(new Vector(0, -0.1, 0), new Vector(-0.01, 0, 0));

            int tickCounter = 0;

            while (projectile.Position.Y > 0)
            {
                projectile = Tick(environment, projectile);

                System.Console.WriteLine("    Tick " + tickCounter + " / X: " + projectile.Position.X + " / Y: " + projectile.Position.Y);

                tickCounter++;
            }
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
