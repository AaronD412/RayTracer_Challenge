namespace RayTracerLogic
{
    /// <summary>
    /// Represents a sphere at the Point(0, 0, 0) with a radius of 1
    /// </summary>
    public abstract class SceneObject
    {
        #region Protected Members

        /// <summary>
        /// The transform.
        /// </summary>
        protected Matrix transform = Matrix.NewIdentityMatrix(4);
       
         /// <summary>
        /// The material.
        /// </summary>
        protected Material material = new Material();

        #endregion

        #region Public Constructors

        /// <summary>
        /// Gets the intersections.
        /// Returns a list of intersections, where the ray hits the sphere.
        /// </summary>
        /// <returns>The intersections.</returns>
        /// <param name="ray">Ray.</param>
        public Intersections GetIntersections(Ray ray)
        {
            Ray localRay = ray.Transform(this.transform.GetInverse());

            return GetIntersectionsLocal(localRay);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the normal at a worldPoint.
        /// </summary>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/>.</returns>
        /// <param name="worldPoint">World point.</param>
        public Vector GetNormalAt(Point worldPoint)
        {
            Matrix inverse = this.transform.GetInverse();

            Point objectPoint = inverse * worldPoint;

            Vector objectNormal = GetNormalAtLocal(objectPoint);

            Vector worldNormal = inverse.Transpose() * objectNormal;

            return worldNormal.Normalize();
        }

        /// <summary>
        /// Gets the normal at local.
        /// </summary>
        /// <returns>The normal at local.</returns>
        /// <param name="objectPoint">Object point.</param>
        public abstract Vector GetNormalAtLocal(Point objectPoint);

        /// <summary>
        /// Nearlies the equals.
        /// </summary>
        /// <returns><c>true</c>, if equals was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="sceneObject">Scene object.</param>
        public bool NearlyEquals(SceneObject sceneObject)
        {
            return transform.NearlyEquals(sceneObject.Transform) &&
                material.NearlyEquals(sceneObject.Material) &&
                NearlyEqualsLocal(sceneObject);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the intersections local.
        /// </summary>
        /// <returns>The intersections local.</returns>
        /// <param name="localRay">Local ray.</param>
        protected abstract Intersections GetIntersectionsLocal(Ray localRay);

        /// <summary>
        /// Nearlies the equals local.
        /// </summary>
        /// <returns><c>true</c>, if equals local was nearlyed, <c>false</c> otherwise.</returns>
        /// <param name="sceneObject">Scene object.</param>
        protected abstract bool NearlyEqualsLocal(SceneObject sceneObject);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the transform.
        /// </summary>
        /// <value>The transform.</value>
        public Matrix Transform
        {
            get
            {
                return transform;
            }
            set
            {
                transform = value;
            }
        }

        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>The material.</value>
        public Material Material
        {
            get
            {
                return material;
            }
            set
            {
                material = value;
            }
        }

        #endregion
    }
}
