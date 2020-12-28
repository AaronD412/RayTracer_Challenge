using System;

namespace RayTracerLogic
{
    public class Cylinder : Shape
    {
        #region Private Members

        private double minimum = double.NegativeInfinity;
        private double maximum = double.PositiveInfinity;
        private bool isClosed = false;

        #endregion

        #region Public Methods

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            // Compute the square of the distance from the y axis
            double distance = point.X * point.X + point.Z * point.Z;

            if (distance < 1 && point.Y >= Maximum - Constants.Epsilon)
            {
                return new Vector(0, 1, 0);
            }

            if (distance < 1 && point.Y <= Minimum + Constants.Epsilon)
            {
                return new Vector(0, -1, 0);
            }

            return new Vector(point.X, 0, point.Z);
        }

        public override Intersections GetIntersectionsLocal(Ray ray)
        {
            Intersections intersections = new Intersections();

            double a = ray.Direction.X * ray.Direction.X + ray.Direction.Z * ray.Direction.Z;

            // Ray is parallel to the y axis
            if (!a.NearlyEquals(0))
            {
                double b = 2 * ray.Origin.X * ray.Direction.X + 2 * ray.Origin.Z * ray.Direction.Z;
                double c = ray.Origin.X * ray.Origin.X + ray.Origin.Z * ray.Origin.Z - 1;

                double discriminant = b * b - 4 * a * c;

                // Ray does not intersect the cylinder
                if (discriminant < 0)
                {
                    return new Intersections();
                }

                double distance0 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                double distance1 = (-b + Math.Sqrt(discriminant)) / (2 * a);

                if (distance0 > distance1)
                {
                    // Swap distance0 and distance1
                    double temp = distance0;
                    distance0 = distance1;
                    distance1 = temp;
                }

                double y0 = ray.Origin.Y + distance0 * ray.Direction.Y;
                if (Minimum < y0 && y0 < Maximum)
                {
                    intersections.Add(new Intersection(distance0, this));
                }

                double y1 = ray.Origin.Y + distance1 * ray.Direction.Y;
                if (Minimum < y1 && y1 < Maximum)
                {
                    intersections.Add(new Intersection(distance1, this));
                }
            }

            IntersectCaps(ray, intersections);

            return intersections;
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(
                new Point(-1, minimum, -1),
                new Point(1, maximum, 1)
            );
        }

        #endregion

        #region Protected Methods

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            // ToDoBre16: Ergänzen
            // ToDoBre16: Auch bei den anderen Shapes checken, dass zumindest der Typ stimmt
            return true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// A Helper function to reduce duplication.
        /// Checks to see if the intersection at 'distance' is within a radius
        /// of 1 (the radius of our cylinder) from the y axis.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private bool CheckCap(Ray ray, double distance)
        {
            double x = ray.Origin.X + distance * ray.Direction.X;
            double z = ray.Origin.Z + distance * ray.Direction.Z;

            return x * x + z * z <= 1;
        }

        private void IntersectCaps(Ray ray, Intersections intersections)
        {
            // Caps only matters if the cylinder is closed, and might possibly be
            // intersected by the ray.
            if (!IsClosed || ray.Direction.Y.NearlyEquals(0))
            {
                return;
            }

            // Check for an intersection with the lower end-cap by intersecting
            // the ray with the plane at y = cylinder.Minimum
            double distance = (Minimum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, distance))
            {
                intersections.Add(new Intersection(distance, this));
            }

            // Check for an intersection with the upper end-cap by intersecting
            // the ray with the plane at y = cylinder.Maximum
            distance = (Maximum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, distance))
            {
                intersections.Add(new Intersection(distance, this));
            }
        }

        #endregion

        #region Public Properties

        public double Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
            }
        }

        public double Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }

        public bool IsClosed
        {
            get
            {
                return isClosed;
            }
            set
            {
                isClosed = value;
            }
        }

        #endregion
    }
}
