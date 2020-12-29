namespace RayTracerLogic
{
    public abstract class Pattern
    {
        #region Private Members

        private Matrix transformationMatrix;

        #endregion

        #region Public Constructors

        public Pattern()
        {
            transformationMatrix = Matrix.NewIdentityMatrix(4);
            transformationMatrix.PrecomputeInverse = true;
        }

        #endregion

        #region Public Methods

        public Color GetPatternAtShape(Shape shape, Point worldPoint)
        {
            Point objectPoint = shape.ConvertWorldPointToObjectPoint(worldPoint);
            Point patternPoint = Transform.GetInverse() * objectPoint;

            return GetPatternAt(patternPoint);
        }

        public abstract Color GetPatternAt(Point point);

        #endregion

        #region Public Properties

        public Matrix Transform
        {
            get
            {
                return transformationMatrix;
            }
            set
            {
                transformationMatrix = value;
                transformationMatrix.PrecomputeInverse = true;
            }
        }

        #endregion
    }
}
