namespace RayTracerLogic
{
    public class Triangle : Shape
    {
        #region Private Members

        private Point point1;
        private Point point2;
        private Point point3;
        private Vector edgeVector1;
        private Vector edgeVector2;
        private Vector normalVector;

        #endregion

        #region Public Constructors

        public Triangle(Point point1, Point point2, Point point3)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.point3 = point3;

            edgeVector1 = point2 - point1;
            edgeVector2 = point3 - point1;

            normalVector = edgeVector2.Cross(edgeVector1).Normalize();
        }

        #endregion

        #region Public Methods

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            return normalVector;
        }

        public override Intersections GetIntersectionsLocal(Ray ray)
        {
            Vector directionCrossEdgeVector2 = ray.Direction.Cross(EdgeVector2);
            double determinant = EdgeVector1.Dot(directionCrossEdgeVector2);

            if (System.Math.Abs(determinant) < Constants.Epsilon)
            {
                return new Intersections();
            }

            double f = 1 / determinant;
            Vector point1ToOrigin = ray.Origin - Point1;
            double u = f * point1ToOrigin.Dot(directionCrossEdgeVector2);

            if (u < 0 || u > 1)
            {
                return new Intersections();
            }

            Vector originCrossEdgeVector1 = point1ToOrigin.Cross(EdgeVector1);
            double v = f * ray.Direction.Dot(originCrossEdgeVector1);

            if (v < 0 || (u + v) > 1)
            {
                return new Intersections();
            }

            double t = f * EdgeVector2.Dot(originCrossEdgeVector1);

            return new Intersections(new Intersection(t, this, u, v));
        }

        public override BoundingBox GetBoundingBox()
        {
            BoundingBox boundingBox = new BoundingBox();

            boundingBox.Add(point1);
            boundingBox.Add(point2);
            boundingBox.Add(point3);

            return boundingBox;
        }

        #endregion

        #region Protected Methods

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            return true;
        }

        #endregion

        #region Public Properties

        public Point Point1
        {
            get
            {
                return point1;
            }
        }

        public Point Point2
        {
            get
            {
                return point2;
            }
        }

        public Point Point3
        {
            get
            {
                return point3;
            }
        }

        public Vector EdgeVector1
        {
            get
            {
                return edgeVector1;
            }
        }

        public Vector EdgeVector2
        {
            get
            {
                return edgeVector2;
            }
        }

        public Vector NormalVector
        {
            get
            {
                return normalVector;
            }
        }

        #endregion
    }
}
