using System.Collections.Generic;

namespace RayTracerLogic
{
    public class Shapes : List<Shape>
    {
        #region Public Constructors

        public Shapes(params Shape[] shapes) : base(shapes)
        {
            // Do nothing
        }

        #endregion
    }
}
