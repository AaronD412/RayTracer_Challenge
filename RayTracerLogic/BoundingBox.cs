using System;

namespace RayTracerLogic
{
    public class BoundingBox : Shape
    {
        #region Private Members

        private Point min = new Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
        private Point max = new Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

        #endregion

        #region Public Constructors

        public BoundingBox()
        {
            // Do nothing
        }

        public BoundingBox(Point min, Point max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion

        #region Public Methods

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            return null;
        }

        public override Intersections GetIntersectionsLocal(Ray ray)
        {
            return null;
        }

        public void Add(Point point)
        {
            min = new Point(Math.Min(min.X, point.X), Math.Min(min.Y, point.Y), Math.Min(min.Z, point.Z));
            max = new Point(Math.Max(max.X, point.X), Math.Max(max.Y, point.Y), Math.Max(max.Z, point.Z));
        }

        public bool ContainsPoint(Point point)
        {
            return
                min.X <= point.X &&
                point.X <= max.X &&
                min.Y <= point.Y &&
                point.Y <= max.Y &&
                min.Z <= point.Z &&
                point.Z <= max.Z;
        }

        public bool ContainsBox(BoundingBox boundingBox)
        {
            return
                ContainsPoint(boundingBox.Min) &&
                ContainsPoint(boundingBox.Max);
        }

        public void Add(BoundingBox boundingBox)
        {
            Add(boundingBox.Min);
            Add(boundingBox.Max);
        }

        public BoundingBox PerformTransformation(Matrix transform)
        {
            BoundingBox boundingBox = new BoundingBox();

            boundingBox.Add(transform * min);
            boundingBox.Add(transform * new Point(min.X, min.Y, max.Z));
            boundingBox.Add(transform * new Point(min.X, max.Y, min.Z));
            boundingBox.Add(transform * new Point(min.X, max.Y, max.Z));
            boundingBox.Add(transform * new Point(max.X, min.Y, min.Z));
            boundingBox.Add(transform * new Point(max.X, min.Y, max.Z));
            boundingBox.Add(transform * new Point(max.X, max.Y, min.Z));
            boundingBox.Add(transform * max);

            return boundingBox;
        }

        public override BoundingBox GetBoundingBox()
        {
            return this;
        }

        #endregion

        #region Protected Methods

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            // ToDoBre16: Implementieren (für alle Shapes)
            return true;
        }

        #endregion

        #region Public Properties

        public Point Min
        {
            get
            {
                return min;
            }
        }

        public Point Max
        {
            get
            {
                return max;
            }
        }

        #endregion
    }
}
