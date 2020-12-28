using RayTracerLogic;

namespace RayTracerTests
{
    public class TestPattern : Pattern
    {
        public override Color GetPatternAt(Point point)
        {
            return new Color(point.X, point.Y, point.Z);
        }
    }
}
