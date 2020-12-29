using System.Collections.Generic;

namespace RayTracerLogic
{
    public class PreparedIntersection
    {
        #region Private Members

        private readonly double distance;
        private readonly Shape shape;
        private Point point;
        private Point overPoint;
        private Point underPoint;
        private Vector eyeVector;
        private Vector normalVector;
        private Vector reflectionVector;
        private bool inside;
        private double n1;
        private double n2;

        #endregion

        #region Public Constructors

        public PreparedIntersection(
            double distance,
            Shape shape,
            Point point,
            Point overPoint,
            Point underPoint,
            Vector eyeVector,
            Vector normalVector,
            Vector reflectionVector,
            bool inside,
            double n1,
            double n2)
        {
            this.distance = distance;
            this.shape = shape;
            this.point = point;
            this.overPoint = overPoint;
            this.underPoint = underPoint;
            this.eyeVector = eyeVector;
            this.normalVector = normalVector;
            this.reflectionVector = reflectionVector;
            this.inside = inside;
            this.n1 = n1;
            this.n2 = n2;
        }

        #endregion

        #region Public Methods

        public double Schlick()
        {
            // Find the cosine of the angle between the eye and normal vectors
            double cos = eyeVector.Dot(normalVector);

            // Total internal reflection can only occur if n1 > n2
            if (n1 > n2)
            {
                double n = n1 / n2;
                double sin2T = n * n * (1 - cos * cos);

                if (sin2T > 1)
                {
                    return 1;
                }

                // Compute cosine of theta_t using trig identity
                double cosT = System.Math.Sqrt(1 - sin2T);

                // When n1 > n2, use cos(theta_t) instead
                cos = cosT;
            }

            double r0 = System.Math.Pow((n1 - n2) / (n1 + n2), 2);

            return r0 + (1 - r0) * System.Math.Pow(1 - cos, 5);
        }

        #endregion

        #region Public Properties

        public double Distance
        {
            get
            {
                return distance;
            }
        }

        public Shape Shape
        {
            get
            {
                return shape;
            }
        }

        public Point Point
        {
            get
            {
                return point;
            }
        }

        public Point OverPoint
        {
            get
            {
                return overPoint;
            }
        }

        public Point UnderPoint
        {
            get
            {
                return underPoint;
            }
        }

        public Vector EyeVector
        {
            get
            {
                return eyeVector;
            }
        }

        public Vector NormalVector
        {
            get
            {
                return normalVector;
            }
        }

        public Vector ReflectionVector
        {
            get
            {
                return reflectionVector;
            }
        }

        public bool Inside
        {
            get
            {
                return inside;
            }
        }

        public double N1
        {
            get
            {
                return n1;
            }
        }

        public double N2
        {
            get
            {
                return n2;
            }
        }

        #endregion
    }
}
