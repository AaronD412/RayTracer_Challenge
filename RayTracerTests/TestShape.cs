using RayTracerLogic;

namespace RayTracerTests
{
    public class TestShape : Shape
    {
        private Ray savedRay = null;

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            return new Vector(point.X, point.Y, point.Z);
        }

        public override Intersections GetIntersectionsLocal(Ray ray)
        {
            savedRay = ray;

            return new Intersections();
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Point(-1, -1, -1),
                new Point(1, 1, 1)
            );
        }

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            return true;
        }

        public Ray SavedRay
        {
            get
            {
                return savedRay;
            }
        }
    }
}
