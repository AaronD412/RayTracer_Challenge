using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class GroupTests
    {
        [Test()]
        public void CreatingANewGroup()
        {
            // Given
            Group group = new Group();

            // Then
            Assert.IsTrue(group.Transform.NearlyEquals(Matrix.NewIdentityMatrix(4)));
            Assert.AreEqual(0, group.Count);
        }

        [Test()]
        public void AShapeHasAParentAttribute()
        {
            // Given
            TestShape shape = new TestShape();

            // Then
            Assert.IsNull(shape.Parent);
        }

        [Test()]
        public void AddingAChildToAGroup()
        {
            // Given
            Group group = new Group();
            TestShape shape = new TestShape();

            // When
            group.AddChild(shape);

            // Then
            Assert.AreNotEqual(0, group.Count);
            Assert.IsTrue(group.Contains(shape));
            Assert.AreEqual(shape.Parent, group);
        }

        [Test()]
        public void IntersectinARayWithAnEmptyGroup()
        {
            // Given
            Group group = new Group();

            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            //When
            Intersections intersections = group.GetIntersections(ray);

            //Then
            Assert.AreEqual(0, intersections.Count);
        }

        [Test()]
        public void IntersectingARayWithAnNonEmptyGroup()
        {
            // ToDO: implement test intersecting a ray with an empty group.
            Group group = new Group();

            Sphere sphere_1 = new Sphere();
            Sphere sphere_2 = new Sphere();
            Sphere sphere_3 = new Sphere();

            sphere_2.Transform = Matrix.NewTranslationMatrix(0, 0, -3);
            sphere_3.Transform = Matrix.NewTranslationMatrix(5, 0, 0);
        }

    }
}
