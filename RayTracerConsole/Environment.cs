using RayTracerLogic;

namespace RayTracerConsole
{
    /// <summary>
    /// Environment for the chapter 1.
    /// </summary>
    public class Environment
    {
        #region Private attrbutes
        private readonly Vector gravity;
        private readonly Vector wind;
        #endregion

        #region Properties
        public Environment(Vector gravity, Vector wind)
        {
            this.gravity = gravity;
            this.wind = wind;
        }

        public Vector Gravity
        {
            get
            {
                return gravity;
            }
        }

        public Vector Wind
        {
            get
            {
                return wind;
            }
        }
        #endregion
    }
}
