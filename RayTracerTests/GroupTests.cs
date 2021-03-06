﻿using NUnit.Framework;
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
        public void IntersectingARayWithAnEmptyGroup()
        {
            // Given
            Group group = new Group();

            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));

            // When
            Intersections intersections = group.GetIntersections(ray);

            // Then
            Assert.AreEqual(0, intersections.Count);
        }

        [Test()]
        public void IntersectingARayWithAnNonEmptyGroup()
        {
            // Given
            Group group = new Group();

            Sphere sphere_1 = new Sphere();
            Sphere sphere_2 = new Sphere();
            Sphere sphere_3 = new Sphere();

            sphere_2.Transform = Matrix.NewTranslationMatrix(0, 0, -3);
            sphere_3.Transform = Matrix.NewTranslationMatrix(5, 0, 0);

            group.AddChild(sphere_1);
            group.AddChild(sphere_2);
            group.AddChild(sphere_3);

            // When
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Intersections intersections = group.GetIntersections(ray);

            // Then
            Assert.AreEqual(4, intersections.Count);
            Assert.IsTrue(intersections[0].Shape.NearlyEquals(sphere_2));
            Assert.IsTrue(intersections[1].Shape.NearlyEquals(sphere_2));
            Assert.IsTrue(intersections[2].Shape.NearlyEquals(sphere_1));
            Assert.IsTrue(intersections[3].Shape.NearlyEquals(sphere_1));
        }

        [Test()]
        public void IntersectingATransformedGroup()
        {
            // Given
            Sphere sphere = new Sphere();
            sphere.Transform = Matrix.NewTranslationMatrix(5, 0, 0);

            Group group = new Group();
            group.Transform = Matrix.NewScalingMatrix(2, 2, 2);
            group.AddChild(sphere);

            // When
            Ray ray = new Ray(new Point(10, 0, -10), new Vector(0, 0, 1));

            Intersections intersections = group.GetIntersections(ray);

            // Then
            Assert.AreEqual(2, intersections.Count);
        }

        [Test()]
        public void ConvertingAPointFromWorldToObjectSpace()
        {
            // Given
            Sphere sphere = new Sphere();
            sphere.Transform = Matrix.NewTranslationMatrix(5, 0, 0);

            Group group2 = new Group();
            group2.Transform = Matrix.NewScalingMatrix(2, 2, 2);
            group2.AddChild(sphere);

            Group group1 = new Group();
            group1.Transform = Matrix.NewRotationYMatrix(System.Math.PI / 2);
            group1.AddChild(group2);

            // When
            Point point = sphere.ConvertWorldPointToObjectPoint(new Point(-2, 0, -10));

            // Then
            Assert.IsTrue(point.NearlyEquals(new Point(0, 0, -1)));
        }

        [Test()]
        public void ConvertingANormalFromObjectToWorldSpace()
        {
            // Given
            Sphere sphere = new Sphere();
            sphere.Transform = Matrix.NewTranslationMatrix(5, 0, 0);

            Group group2 = new Group();
            group2.Transform = Matrix.NewScalingMatrix(1, 2, 3);
            group2.AddChild(sphere);

            Group group1 = new Group();
            group1.Transform = Matrix.NewRotationYMatrix(System.Math.PI / 2);
            group1.AddChild(group2);

            // When
            Vector normalVector = sphere.ConvertNormalVectorToWorldSpace(
                new Vector(
                    System.Math.Sqrt(3) / 3,
                    System.Math.Sqrt(3) / 3,
                    System.Math.Sqrt(3) / 3
                )
            );

            // Then
            Assert.IsTrue(normalVector.NearlyEquals(new Vector(0.2857, 0.4286, -0.8571)));
        }

        [Test()]
        public void FindingTheNormalOnAChildObject()
        {
            // Given
            Sphere sphere = new Sphere();
            sphere.Transform = Matrix.NewTranslationMatrix(5, 0, 0);

            Group group2 = new Group();
            group2.Transform = Matrix.NewScalingMatrix(1, 2, 3);
            group2.AddChild(sphere);

            Group group1 = new Group();
            group1.Transform = Matrix.NewRotationYMatrix(System.Math.PI / 2);
            group1.AddChild(group2);

            // When
            Vector normalVector = sphere.GetNormalAt(new Point(1.7321, 1.1547, -5.5774));

            // Then
            Assert.IsTrue(normalVector.NearlyEquals(new Vector(0.2857, 0.4286, -0.8571)));
        }
    }
}
