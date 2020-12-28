using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Intersection.
    /// </summary>
    public class PreparedIntersection
    {
        #region Private Members

        /// <summary>
        /// The distance.
        /// </summary>
        private double distance;

        /// <summary>
        /// The scene object.
        /// </summary>
        private SceneObject sceneObject;

        /// <summary>
        /// The point.
        /// </summary>
        private Point point;

        /// <summary>
        /// The eye vector.
        /// </summary>
        private Vector eyeVector;

        /// <summary>
        /// The normal vector.
        /// </summary>
        private Vector normalVector;

        /// <summary>
        /// The inside.
        /// </summary>
        private bool inside;

        /// <summary>
        /// The over point.
        /// </summary>
        private Point overPoint;

        /// <summary>
        /// The reflect vector.
        /// </summary>
        private Vector reflectVector;

        /// <summary>
        /// The n1.
        /// </summary>
        private double n1;

        /// <summary>
        /// The n2.
        /// </summary>
        private double n2;

        /// <summary>
        /// The under point.
        /// </summary>
        private Point underPoint;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.Intersection"/> class.
        /// </summary>
        /// <param name="distance">Distance.</param>
        /// <param name="sceneObject">Scene object.</param>
        public PreparedIntersection(double distance, SceneObject sceneObject, Point point, Vector eyeVector, Vector normalVector, double n1, double n2)
        {
            if (normalVector.Dot(eyeVector) < 0)
            {
                inside = true;
                normalVector = -normalVector;
            }
            else
            {
                inside = false;
            }

            overPoint = point + normalVector * Constants.Epsilon;
            underPoint = point - normalVector * Constants.Epsilon;

            this.distance = distance;
            this.sceneObject = sceneObject;
            this.point = point;
            this.eyeVector = eyeVector;
            this.normalVector = normalVector;
            this.n1 = n1;
            this.n2 = n2;

            reflectVector = -eyeVector.GetReflect(normalVector);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Calculates the Schlick
        /// </summary>
        /// <returns>The schlick.</returns>
        public double Schlick()
        {
            // Find the cosine of the angle between the eye and normal vectors
            double cos = eyeVector.Dot(normalVector);

            // Total internal reflection can only occur if n1 > n2
            if (n1 > n2)
            {
                double n = n1 / n2;

                double sin2T = (n * n) * (1.0 - cos * cos);

                if (sin2T > 1.0)
                {
                    return 1.0;
                }

                // Compute the cosine of the thetaT using trig identity
                double cosT = Math.Sqrt(1.0 - sin2T);

                // When n1 > n2, use cos(thetaT) instead
                cos = cosT;
            }

            double r0 = ((n1 - n2) / (n1 + n2)) * ((n1 - n2) / (n1 + n2));

            return r0 + (1.0 - r0) * Math.Pow(1- cos, 5);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the distance.
        /// </summary>
        /// <value>The distance.</value>
        public double Distance
        {
            get
            {
                return distance;
            }
        }

        /// <summary>
        /// Gets the scene object.
        /// </summary>
        /// <value>The scene object.</value>
        public SceneObject SceneObject
        {
            get
            {
                return sceneObject;
            }
        }

        /// <summary>
        /// Gets the point.
        /// </summary>
        /// <value>The point.</value>
        public Point Point
        {
            get
            {
                return point;
            }
        }

        /// <summary>
        /// Gets the eye vector.
        /// </summary>
        /// <value>The eye vector.</value>
        public Vector EyeVector
        {
            get
            {
                return eyeVector;
            }
        }

        /// <summary>
        /// Gets the normal vector.
        /// </summary>
        /// <value>The normal vector.</value>
        public Vector NormalVector
        {
            get
            {
                return normalVector;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:RayTracerLogic.PreparedIntersection"/> is inside.
        /// </summary>
        /// <value><c>true</c> if inside; otherwise, <c>false</c>.</value>
        public bool Inside
        {
            get
            {
                return inside;
            }
        }

        /// <summary>
        /// Gets the over point.
        /// </summary>
        /// <value>The over point.</value>
        public Point OverPoint
        {
            get
            {
                return overPoint;
            }
        }

        /// <summary>
        /// Gets the reflect vector.
        /// </summary>
        /// <value>The reflect vector.</value>
        public Vector ReflectVector
        {
            get
            {
                return reflectVector;
            }
        }

        /// <summary>
        /// Gets the n1.
        /// </summary>
        /// <value>The n1.</value>
        public double N1
        {
            get
            {
                return n1;
            }
        }

        /// <summary>
        /// Gets the n2.
        /// </summary>
        /// <value>The n2.</value>
        public double N2
        {
            get
            {
                return n2;
            }
        }

        /// <summary>
        /// Gets the under point.
        /// </summary>
        /// <value>The under point.</value>
        public Point UnderPoint
        {
            get
            {
                return underPoint;
            }
        }

        #endregion
    }
}
