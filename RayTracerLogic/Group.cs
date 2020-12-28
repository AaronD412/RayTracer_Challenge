using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Material.
    /// </summary>
    public class Group : Shape
    {
        #region Private Members
        private Shapes children = new Shapes();
        #endregion

        #region Public Method
        public void AddChild(Shape shape)
        {
            shape.Parent = this;
            children.Add(shape);
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

        public bool Contains(Shape shape)
        {
            return children.Contains(shape);
        }

        public override BoundingBox GetBoundingBox()
        {
            throw new NotImplementedException();
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

        public override Vector GetNormalAtLocal(Point point, Intersection hit = null)
        {
            throw new NotImplementedException("This method should be never called!");
        }

        protected override bool NearlyEqualsLocal(Shape shape)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
