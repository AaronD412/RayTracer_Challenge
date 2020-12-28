using System.Collections.Generic;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents an Intersection.
    /// </summary>
    public class Intersection
    {
        #region Private Members

        /// <summary>
        /// The distance.
        /// </summary>
        private double distance;

        /// <summary>
        /// The scene object.
        /// </summary>
        private Shape shape;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the Intersection class.
        /// </summary>
        /// <param name="distance">Distance.</param>
        /// <param name="sceneObject">Scene object.</param>
        public Intersection(double distance, Shape shape)
        {
            this.distance = distance;
            this.shape = shape;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Gets the prepared intersection between a ray and an intersection.
        /// </summary>
        /// <returns>The prepared intersection.</returns>
        /// <param name="ray">Ray.</param>
        /// <param name="intersections">Intersections.</param>
        public PreparedIntersection GetPreparedIntersection(Ray ray, Intersections intersections)
        {
            Point point = ray.GetPosition(distance);

            double n1 = 0;
            double n2 = 0;

            List<Shape> container = new List<Shape>();

            foreach (Intersection intersection in intersections)
            {
                if (this == intersection)
                {
                    if (container.Count == 0)
                    {
                        n1 = 1.0;
                    }
                    else
                    {
                        n1 = container[container.Count - 1].Material.RefractiveIndex;
                    }
                }

                if (container.Contains(intersection.Shape))
                {
                    container.Remove(intersection.Shape);
                }
                else
                {
                    container.Add(intersection.Shape);
                }

                if (this == intersection)
                {
                    if (container.Count == 0)
                    {
                        n2 = 1.0;
                    }
                    else
                    {
                        n2 = container[container.Count - 1].Material.RefractiveIndex;
                    }

                    break;
                }
            }

            PreparedIntersection preparedIntersection = new PreparedIntersection(
                distance,
                shape,
                point,
                -ray.Direction,
                shape.GetNormalAt(point),
                n1,
                n2
            );

            return preparedIntersection;
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
        public Shape Shape
        {
            get
            {
                return shape;
            }
        }

        #endregion
    }
}
