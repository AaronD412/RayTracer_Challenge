using System;

namespace RayTracerLogic
{
    public class Cube : Shape
    {
        #region Public Methods

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            double absoluteX = Math.Abs(point.X);
            double absoluteY = Math.Abs(point.Y);
            double absoluteZ = Math.Abs(point.Z);

            double maxAbsoluteCoordinate = Math.Max(absoluteX, Math.Max(absoluteY, absoluteZ));

            if (maxAbsoluteCoordinate == absoluteX)
            {
                return new Vector(point.X, 0, 0);
            }

            if (maxAbsoluteCoordinate == absoluteY)
            {
                return new Vector(0, point.Y, 0);
            }

            return new Vector(0, 0, point.Z);
        }

        public override Intersections GetIntersectionsLocal(Ray ray)
        {
            Tuple<double, double> xDistanceMinAndMax = CheckAxis(ray.Origin.X, ray.Direction.X);
            Tuple<double, double> yDistanceMinAndMax = CheckAxis(ray.Origin.Y, ray.Direction.Y);
            Tuple<double, double> zDistanceMinAndMax = CheckAxis(ray.Origin.Z, ray.Direction.Z);

            double distanceMin = Math.Max(xDistanceMinAndMax.Item1, Math.Max(yDistanceMinAndMax.Item1, zDistanceMinAndMax.Item1));
            double distanceMax = Math.Min(xDistanceMinAndMax.Item2, Math.Min(yDistanceMinAndMax.Item2, zDistanceMinAndMax.Item2));

            if (distanceMin > distanceMax)
            {
                return new Intersections();
            }

            return new Intersections(
                new Intersection(distanceMin, this),
                new Intersection(distanceMax, this)
            );
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Point(-1, -1, -1),
                new Point(1, 1, 1)
            );
        }

        #endregion

        #region Protected Methods

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            return true;
        }

        #endregion

        #region Private Methods

        private Tuple<double, double> CheckAxis(double origin, double direction)
        {
            double distanceMinNumerator = -1 - origin;
            double distanceMaxNumerator = 1 - origin;

            double distanceMin;
            double distanceMax;

            if (Math.Abs(direction) >= Constants.Epsilon)
            {
                distanceMin = distanceMinNumerator / direction;
                distanceMax = distanceMaxNumerator / direction;
            }
            else
            {
                distanceMin = distanceMinNumerator * double.PositiveInfinity;
                distanceMax = distanceMaxNumerator * double.PositiveInfinity;
            }

            if (distanceMin > distanceMax)
            {
                return new Tuple<double, double>(distanceMax, distanceMin);
            }

            return new Tuple<double, double>(distanceMin, distanceMax);
        }

        #endregion
    }
}
