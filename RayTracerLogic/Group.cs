using System;
using System.Collections.Generic;

namespace RayTracerLogic
{
    public class Group : Shape
    {
        #region Private Members

        private Shapes children = new Shapes();

        #endregion

        #region Public Methods

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            throw new NotImplementedException("This method should never be called.");
        }

        public override Intersections GetIntersectionsLocal(Ray ray)
        {
            Intersections intersections = new Intersections();

            foreach (Shape child in children)
            {
                intersections.AddRange(child.GetIntersections(ray));
            }

            intersections.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            return intersections;
        }

        public void AddChild(Shape shape)
        {
            shape.Parent = this;
            children.Add(shape);
        }

        public void AddRange(IEnumerable<Shape> shapes)
        {
            foreach (Shape shape in shapes)
            {
                shape.Parent = this;
            }
            children.AddRange(shapes);
        }

        public bool Contains(Shape shape)
        {
            return children.Contains(shape);
        }

        public override bool Includes(Shape shape)
        {
            foreach (Shape child in children)
            {
                if (child.Includes(shape))
                {
                    return true;
                }
            }

            return false;
        }

        public override BoundingBox GetBoundingBox()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Methods

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            return true;
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                return children.Count;
            }
        }

        public Shape this[int index]
        {
            get
            {
                return children[index];
            }
        }

        #endregion
    }
}
