using System.Collections.Generic;

namespace RayTracerLogic
{
    public class Intersection
    {
        #region Private Members

        private readonly double distance;
        private readonly Shape shape;
        private readonly double? u = null;
        private readonly double? v = null;

        #endregion

        #region Public Constructors

        public Intersection(double distance, Shape shape)
        {
            this.distance = distance;
            this.shape = shape;
        }

        public Intersection(double distance, Shape shape, double u, double v) : this(distance, shape)
        {
            this.u = u;
            this.v = v;
        }

        #endregion

        #region Public Methods

        public PreparedIntersection Prepare(Ray ray, Intersections intersections)
        {
            Point point = ray.GetPositionAt(distance);
            Vector eyeVector = -ray.Direction;
            Vector normalVector = shape.GetNormalAt(point, this);

            bool inside = false;
            if (normalVector.Dot(eyeVector) < 0)
            {
                inside = true;
                normalVector = -normalVector;
            }

            Point overPoint = point + (normalVector * Constants.Epsilon);
            Point underPoint = point - (normalVector * Constants.Epsilon);
            Vector reflectionVector = ray.Direction.GetReflect(normalVector);

            List<Shape> shapes = new List<Shape>();
            double n1 = 0.0;
            double n2 = 0.0;

            foreach (Intersection intersection in intersections)
            {
                if (this == intersection)
                {
                    if (shapes.Count == 0)
                    {
                        n1 = 1.0;
                    }
                    else
                    {
                        n1 = shapes[shapes.Count - 1].Material.RefractiveIndex;
                    }
                }

                if (shapes.Contains(intersection.Shape))
                {
                    shapes.Remove(intersection.Shape);
                }
                else
                {
                    shapes.Add(intersection.Shape);
                }

                if (this == intersection)
                {
                    if (shapes.Count == 0)
                    {
                        n2 = 1.0;
                    }
                    else
                    {
                        n2 = shapes[shapes.Count - 1].Material.RefractiveIndex;
                    }

                    break;
                }
            }

            PreparedIntersection preparedIntersection = new PreparedIntersection(
                distance,
                shape,
                point,
                overPoint,
                underPoint,
                eyeVector,
                normalVector,
                reflectionVector,
                inside,
                n1,
                n2);

            return preparedIntersection;
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

        public double? U
        {
            get
            {
                return u;
            }
        }

        public double? V
        {
            get
            {
                return v;
            }
        }

        #endregion
    }
}
