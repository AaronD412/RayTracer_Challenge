namespace RayTracerLogic
{
    public abstract class Shape
    {
        #region Private Members

        private Matrix transformationMatrix;
        private Material material = new Material();
        private Shape parent = null;
        private bool useParentMaterial = false;

        #endregion

        #region Public Constructors

        public Shape()
        {
            transformationMatrix = Matrix.NewIdentityMatrix(4);
            transformationMatrix.PrecomputeInverse = true;
        }

        #endregion

        #region Public Methods

        public Intersections GetIntersections(Ray ray)
        {
            ray = ray.Transform(Transform.GetInverse());

            return GetIntersectionsLocal(ray);
        }

        public Vector GetNormalAt(Point worldPoint, Intersection hit = null)
        {
            Point objectPoint = ConvertWorldPointToObjectPoint(worldPoint);
            Vector localNormal = GetNormalAtLocal(objectPoint, hit);

            return ConvertNormalVectorToWorldSpace(localNormal);
        }

        public bool NearlyEquals(Shape shape)
        {
            if (shape == null)
            {
                return false;
            }

            if (!GetType().Equals(shape.GetType()))
            {
                return false;
            }

            return Transform.NearlyEquals(shape.Transform) &&
                Material.NearlyEquals(shape.Material) &&
                NearlyEqualsLocal(shape);
        }

        public Point ConvertWorldPointToObjectPoint(Point worldPoint)
        {
            if (parent != null)
            {
                worldPoint = parent.ConvertWorldPointToObjectPoint(worldPoint);
            }

            return Transform.GetInverse() * worldPoint;
        }

        public Vector ConvertNormalVectorToWorldSpace(Vector normalVector)
        {
            normalVector = Transform.GetInverse().Transpose() * normalVector;
            normalVector = normalVector.Normalize();

            if (parent != null)
            {
                normalVector = parent.ConvertNormalVectorToWorldSpace(normalVector);
            }

            return normalVector;
        }

        public virtual bool Includes(Shape shape)
        {
            return shape == this;
        }

        public abstract Vector GetNormalAtLocal(Point point, Intersection intersection);

        // ToDoBre16: Ggf. internal machen
        public abstract Intersections GetIntersectionsLocal(Ray ray);

        public abstract BoundingBox GetBoundingBox();

        #endregion

        #region Protected Methods

        protected abstract bool NearlyEqualsLocal(Shape shape);

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

        public Material Material
        {
            get
            {
                if (useParentMaterial && parent != null)
                {
                    return parent.Material;
                }

                return material;
            }
            set
            {
                if (useParentMaterial && parent != null)
                {
                    parent.Material = value;
                }
                else
                {
                    material = value;
                }
            }
        }

        public bool UseParentMaterial
        {
            get
            {
                return useParentMaterial;
            }
            set
            {
                useParentMaterial = value;
            }
        }

        public Shape Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        #endregion
    }
}
