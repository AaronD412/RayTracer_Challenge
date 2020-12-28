using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Represent the Projectile for Chapter 1.
    /// </summary>
    public class Projectile
    {
        #region Private attributes
        private readonly Point position;
        private readonly Vector velocity;
        #endregion

        #region Public constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerConsole.Projectile"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="velocity">Velocity.</param>
        public Projectile(Point position, Vector velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point Position
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Gets the velocity.
        /// </summary>
        /// <value>The velocity.</value>
        public Vector Velocity
        {
            get
            {
                return velocity;
            }
        }
        #endregion
    }
}
