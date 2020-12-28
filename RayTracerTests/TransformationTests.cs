using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class TransformationTests
    {
        [Test()]
        public void MultiplyingByATranslationMatrix()
        {
            // Given
            Matrix transform = Matrix.NewTranslationMatrix(5, -3, 2);
            Point point = new Point(-3, 4, 5);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(2, 1, 7)));
        }

        [Test()]
        public void MultiplyingByTheInverseOfATranslationMatrix()
        {
            // Given
            Matrix transform = Matrix.NewTranslationMatrix(5, -3, 2);
            Matrix inverse = transform.GetInverse();
            Point point = new Point(-3, 4, 5);

            // Then
            Assert.IsTrue((inverse * point).NearlyEquals(new Point(-8, 7, 3)));
        }

        [Test()]
        public void TranslationDoesNotAffectVectors()
        {
            // Given
            Matrix transform = Matrix.NewTranslationMatrix(5, -3, 2);
            Vector vector = new Vector(-3, 4, 5);

            // Then
            Assert.IsTrue((transform * vector).NearlyEquals(vector));
        }

        [Test()]
        public void AScalingMatrixAppliedToAPoint()
        {
            // Given
            Matrix scaling = Matrix.NewScalingMatrix(2, 3, 4);
            Point point = new Point(-4, 6, 8);

            // Then
            Assert.IsTrue((scaling * point).NearlyEquals(new Point(-8, 18, 32)));
        }

        [Test()]
        public void AScalingMatrixAppliedToAVector()
        {
            // Given
            Matrix scaling = Matrix.NewScalingMatrix(2, 3, 4);
            Vector vector = new Vector(-4, 6, 8);

            // Then
            Assert.IsTrue((scaling * vector).NearlyEquals(new Vector(-8, 18, 32)));
        }

        [Test()]
        public void MultiplyingByTheInverseOfAScalingMatrix()
        {
            // Given
            Matrix scaling = Matrix.NewScalingMatrix(2, 3, 4);
            Matrix inverse = scaling.GetInverse();
            Vector vector = new Vector(-4, 6, 8);

            // Then
            Assert.IsTrue((inverse * vector).NearlyEquals(new Vector(-2, 2, 2)));
        }

        [Test()]
        public void RefelctionIsScalingByANegativValue()
        {
            // Given
            Matrix scaling = Matrix.NewScalingMatrix(-1, 1, 1);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((scaling * point).NearlyEquals(new Point(-2, 3, 4)));
        }

        [Test()]
        public void RotatingAPointAroundTheXAxis()
        {
            // Given
            Point point = new Point(0, 1, 0);

            Matrix halfQuarter = Matrix.NewRotationXMatrix(System.Math.PI / 4);
            Matrix fullQuarter = Matrix.NewRotationXMatrix(System.Math.PI / 2);

            // Then
            Assert.IsTrue(halfQuarter.Multiply(point).NearlyEquals(new Point(0, System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2)));
            Assert.IsTrue(fullQuarter.Multiply(point).NearlyEquals(new Point(0, 0 ,1)));
        }

        [Test()]
        public void TheInverseOfAnXRotationRotatesInTheOppositeDirection()
        {
            // Given
            Point point = new Point(0, 1, 0);

            Matrix halfQuarter = Matrix.NewRotationXMatrix(System.Math.PI / 4);
            Matrix inverse = halfQuarter.GetInverse();

            // Then
            Assert.IsTrue((inverse * point).NearlyEquals(new Point(0, System.Math.Sqrt(2) / 2, -System.Math.Sqrt(2) / 2)));
        }

        [Test()]
        public void RotatingAPointAroundTheYAxis()
        {
            // Given
            Point point = new Point(0, 0, 1);

            Matrix halfQuarter = Matrix.NewRotationYMatrix(System.Math.PI / 4);
            Matrix fullQuarter = Matrix.NewRotationYMatrix(System.Math.PI / 2);

            // Then
            Assert.IsTrue((halfQuarter * point).NearlyEquals(new Point(System.Math.Sqrt(2) / 2, 0, System.Math.Sqrt(2) / 2)));
            Assert.IsTrue((fullQuarter * point).NearlyEquals(new Point(1, 0, 0)));
        }

        [Test()]
        public void RotatingAPointAroundTheZAxis()
        {
            // Given
            Point point = new Point(0, 1, 0);

            Matrix halfQuarter = Matrix.NewRotationZMatrix(System.Math.PI / 4);
            Matrix fullQuarter = Matrix.NewRotationZMatrix(System.Math.PI / 2);

            // Then
            Assert.IsTrue((halfQuarter * point).NearlyEquals(new Point(-System.Math.Sqrt(2) / 2, System.Math.Sqrt(2) / 2, 0)));
            Assert.IsTrue((fullQuarter * point).NearlyEquals(new Point(-1, 0, 0)));
        } 

        [Test()]
        public void AShearingTransformationMovesXInProportionToY()
        {
            // Given
            Matrix transform = Matrix.NewShearingMatrix(1, 0, 0, 0, 0, 0);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(5, 3, 4)));
        }

        [Test()]
        public void AShearingTransformationMovesXInProportionToZ()
        {
            // Given
            Matrix transform = Matrix.NewShearingMatrix(0, 1, 0, 0, 0, 0);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(6, 3, 4)));
        }

        [Test()]
        public void AShearingTransformationMovesYInProportionToX()
        {
            // Given
            Matrix transform = Matrix.NewShearingMatrix(0, 0, 1, 0, 0, 0);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(2, 5, 4)));
        }

        [Test()]
        public void AShearingTransformationMovesYInProportionToZ()
        {
            // Given
            Matrix transform = Matrix.NewShearingMatrix(0, 0, 0, 1, 0, 0);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(2, 7, 4)));
        }

        [Test()]
        public void AShearingTransformationMovesZInProportionToX()
        {
            // Given
            Matrix transform = Matrix.NewShearingMatrix(0, 0, 0, 0, 1, 0);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(2, 3, 6)));
        }

        [Test()]
        public void AShearingTransformationMovesZInProportionToY()
        {
            // Given
            Matrix transform = Matrix.NewShearingMatrix(0, 0, 0, 0, 0, 1);
            Point point = new Point(2, 3, 4);

            // Then
            Assert.IsTrue((transform * point).NearlyEquals(new Point(2, 3, 7)));
        }

        [Test()]
        public void IndividualTransformationsAreAppliedInSequence()
        {
            // Given
            Point point = new Point(1, 0, 1);

            Matrix rotation = Matrix.NewRotationXMatrix(System.Math.PI / 2);
            Matrix scaling = Matrix.NewScalingMatrix(5, 5, 5);
            Matrix translation = Matrix.NewTranslationMatrix(10, 5, 7);

            // Apply rotation first
            // When
            Point point2 = rotation * point;
            
            // Then
            Assert.IsTrue(point2.NearlyEquals(new Point(1, -1, 0)));

            // Then apply scaling
            // When
            Point point3 = scaling * point2;

            // Then
            Assert.IsTrue(point3.NearlyEquals(new Point(5, -5, 0)));

            // Then apply translation
            // When
            Point point4 = translation * point3; 

            // Then
            Assert.IsTrue(point4.NearlyEquals(new Point(15, 0, 7)));
        }

        [Test()]
        public void ChainedTransformationsMustBeAppliedInReverseOrder()
        {
            Point point = new Point(1, 0, 1);

            Matrix rotation = Matrix.NewRotationXMatrix(System.Math.PI / 2);
            Matrix scaling = Matrix.NewScalingMatrix(5, 5, 5);
            Matrix translation = Matrix.NewTranslationMatrix(10, 5, 7);

            Matrix transform = translation * scaling * rotation;

            Assert.IsTrue((transform * point).NearlyEquals(new Point(15, 0, 7)));
        }

        [Test()]
        public void ChainedTransformationsMustBeAppliedInReverseOrderFluentApi()
        {
            Point point = new Point(1, 0, 1);

            Matrix transform = Matrix.NewIdentityMatrix(4).RotateX(System.Math.PI / 2).Scale(5, 5, 5).Translate(10, 5, 7);

            Assert.IsTrue((transform * point).NearlyEquals(new Point(15, 0, 7)));
        }
    }
}
