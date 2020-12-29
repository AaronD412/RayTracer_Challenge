namespace RayTracerLogic
{
    public class SmoothTriangle : Triangle
    {
        #region Private Members

        private Vector normalVector1;
        private Vector normalVector2;
        private Vector normalVector3;

        #endregion

        #region Public Constructors

        public SmoothTriangle(Point point1, Point point2, Point point3, Vector normalVector1, Vector normalVector2, Vector normalVector3)
            : base(point1, point2, point3)
        {
            this.normalVector1 = normalVector1;
            this.normalVector2 = normalVector2;
            this.normalVector3 = normalVector3;
        }

        #endregion

        #region Public Methods

        public override Vector GetNormalAtLocal(Point point, Intersection hit)
        {
            return normalVector2 * hit.U.Value + normalVector3 * hit.V.Value + normalVector1 * (1 - hit.U.Value - hit.V.Value);
        }

        #endregion

        #region Protected Methods

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            return true;
        }

        #endregion

        #region Public Properties

        public Vector NormalVector1
        {
            get
            {
                return normalVector1;
            }
        }

        public Vector NormalVector2
        {
            get
            {
                return normalVector2;
            }
        }

        public Vector NormalVector3
        {
            get
            {
                return normalVector3;
            }
        }

        #endregion
    }
}
