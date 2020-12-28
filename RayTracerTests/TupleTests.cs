using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class TupleTests
    {
        [Test()]
        public void TupleWithWEqualToZeroIsAPoint()
        {
            // Given
            Tuple tuple = new Point(4.3, -4.2, 3.1);

            // Then
            Assert.IsTrue(tuple.X.NearlyEquals(4.3));
            Assert.IsTrue(tuple.Y.NearlyEquals(-4.2));
            Assert.IsTrue(tuple.Z.NearlyEquals(3.1));
            Assert.IsTrue(tuple.W.NearlyEquals(1.0));

            Assert.IsInstanceOf<Point>(tuple);
            Assert.IsNotInstanceOf<Vector>(tuple);
        }

        [Test()]
        public void TupleWithWEqualToZeroIsAVector()
        {
            // Given
            Tuple tuple = new Vector(4.3, -4.2, 3.1);

            // Then
            Assert.IsTrue(tuple.X.NearlyEquals(4.3));
            Assert.IsTrue(tuple.Y.NearlyEquals(-4.2));
            Assert.IsTrue(tuple.Z.NearlyEquals(3.1));
            Assert.IsTrue(tuple.W.NearlyEquals(0.0));

            Assert.IsInstanceOf<Vector>(tuple);
            Assert.IsNotInstanceOf<Point>(tuple);
        }

        [Test()]
        public void PointDescribesTuplesWithWEqualToOne()
        {
            // Given
            Point point = new Point(4, -4, 3);

            // Then
            Assert.IsInstanceOf<Tuple>(point);

            Assert.IsTrue(point.X.NearlyEquals(4));
            Assert.IsTrue(point.Y.NearlyEquals(-4));
            Assert.IsTrue(point.Z.NearlyEquals(3));
            Assert.IsTrue(point.W.NearlyEquals(1));
        }

        [Test()]
        public void VectorDecribesTuplesWithWEqualToZero()
        {
            // Given
            Vector vector = new Vector(4, -4, 3);

            // Then
            Assert.IsInstanceOf<Tuple>(vector);

            Assert.IsTrue(vector.X.NearlyEquals(4));
            Assert.IsTrue(vector.Y.NearlyEquals(-4));
            Assert.IsTrue(vector.Z.NearlyEquals(3));
            Assert.IsTrue(vector.W.NearlyEquals(0));
        }

        [Test()]
        public void AddingTwoTuples()
        {
            // Given
            Point point = new Point(3, -2, 5);
            Vector vector = new Vector(-2, 3, 1);

            // Then
            Point result = point + vector;

            Assert.IsTrue(result.NearlyEquals(new Point(1, 1, 6)));
        }

        [Test()]
        public void SubtractingTwoPoints()
        {
            // Given
            Point point1 = new Point(3, 2, 1);
            Point point2 = new Point(5, 6, 7);

            // Then
            Vector result = point1 - point2;

            Assert.IsTrue(result.NearlyEquals(new Vector(-2, -4, -6)));
        }

        [Test()]
        public void SubtractingAVectorFromAPoint()
        {
            // Given
            Point point = new Point(3, 2, 1);
            Vector vector = new Vector(5, 6, 7);

            // Then
            Point result = point - vector;

            Assert.IsTrue(result.NearlyEquals(new Point(-2, -4, -6)));
        }

        [Test()]
        public void SubtractingTwoVectors()
        {
            // Given
            Vector vector1 = new Vector(3, 2, 1);
            Vector vector2 = new Vector(5, 6, 7);

            // Then
            Vector result = vector1 - vector2;

            Assert.IsTrue(result.NearlyEquals(new Vector(-2, -4, -6)));
        }

        [Test()]
        public void SubtractingAVectorFromTheZeroVector()
        {
            // Given
            Vector zero = new Vector(0, 0, 0);
            Vector vector = new Vector(1, -2, -3);

            // Then
            Vector result = zero - vector;

            Assert.IsTrue(result.NearlyEquals(new Vector(-1, 2, 3)));
        }

        [Test()]
        public void NegatingAVector()
        {
            // Given
            Vector vector = new Vector(1, -2, 3);

            // Then
            Vector result = -vector;

            Assert.IsTrue(result.NearlyEquals(new Vector(-1, 2, -3)));
        }

        [Test()]
        public void NegatingAPoint()
        {
            // Given
            Point point = new Point(1, -2, 3);

            // Then
            Point result = -point;

            Assert.IsTrue(result.NearlyEquals(new Point(-1, 2, -3)));
        }

        [Test()]
        public void MultiplyingAVectorByAScalar()
        {
            // Given
            Vector vector = new Vector(1, -2, 3);

            // Then
            Vector result = vector * 3.5;

            Assert.IsTrue(result.NearlyEquals(new Vector(3.5, -7, 10.5)));
        }

        [Test()]
        public void MultiplyingAPointByAScalar()
        {
            // Given
            Point point = new Point(1, -2, 3);

            // Then
            Point result = point * 3.5;

            Assert.IsTrue(result.NearlyEquals(new Point(3.5, -7, 10.5)));
        }

        [Test()]
        public void MultiplyingAVectorByAFraction()
        {
            // Given
            Vector vector = new Vector(1, -2, 3);

            // Then
            Vector result = vector * 0.5;

            Assert.IsTrue(result.NearlyEquals(new Vector(0.5, -1, 1.5)));
        }

        [Test()]
        public void MultiplyingAPointByAFraction()
        {
            // Given
            Point point = new Point(1, -2, 3);

            // Then
            Point result = point * 0.5;

            Assert.IsTrue(result.NearlyEquals(new Point(0.5, -1, 1.5)));
        }

        [Test()]
        public void DividingAVectorByAScalar()
        {
            // Given
            Vector vector = new Vector(1, -2, 3);

            // Then
            Vector result = vector / 2;

            Assert.IsTrue(result.NearlyEquals(new Vector(0.5, -1, 1.5)));
        }

        [Test()]
        public void DividingAPointByAScalar()
        {
            // Given
            Point point = new Point(1, -2, 3);

            // Then
            Point result = point / 2;

            Assert.IsTrue(result.NearlyEquals(new Point(0.5, -1, 1.5)));
        }

        [Test()]
        public void MagnitudeOfVectorOneZeroZero()
        {
            // Given
            Vector vector = new Vector(1, 0, 0);

            // Then
            double result = vector.GetMagnitude();

            Assert.IsTrue(result.NearlyEquals(1));
        }

        [Test()]
        public void MagnitudeOfVectorZeroOneZero()
        {
            // Given
            Vector vector = new Vector(0, 1, 0);

            // Then
            double result = vector.GetMagnitude();

            Assert.IsTrue(result.NearlyEquals(1));
        }

        [Test()]
        public void MagnitudeOfVectorZeroZeroOne()
        {
            // Given
            Vector vector = new Vector(0, 0, 1);

            // Then
            double result = vector.GetMagnitude();

            Assert.IsTrue(result.NearlyEquals(1));
        }

        [Test()]
        public void MagnitudeOfVectorOneTwoThree()
        {
            // Given
            Vector vector = new Vector(1, 2, 3);

            // Then
            double result = vector.GetMagnitude();

            Assert.IsTrue(result.NearlyEquals(System.Math.Sqrt(14)));
        }

        [Test()]
        public void MagnitudeOfVectorMinusOneMinusTwoMinusThree()
        {
            // Given
            Vector vector = new Vector(-1, -2, -3);

            // Then
            double result = vector.GetMagnitude();

            Assert.IsTrue(result.NearlyEquals(System.Math.Sqrt(14)));
        }

        [Test()]
        public void NormalizingVectorFourZeroZeroGivesOneZeroZero()
        {
            // Given
            Vector vector = new Vector(4, 0, 0);

            // Then
            Vector result = vector.Normalize();

            Assert.IsTrue(result.NearlyEquals(new Vector(1, 0, 0)));
        }

        [Test()]
        public void NormalizingVectorOneTwoThree()
        {
            // Given
            Vector vector = new Vector(1, 2, 3);

            // Then
            Vector result = vector.Normalize();

            Assert.IsTrue(result.NearlyEquals(new Vector(0.26726, 0.53452, 0.80178)));
        }

        [Test()]
        public void TheMagnitudeOfANormalizedVector()
        {
            // Given
            Vector vector = new Vector(1, 2, 3);

            // Then
            double magnitude = vector.Normalize().GetMagnitude();

            Assert.IsTrue(magnitude.NearlyEquals(1));
        }

        [Test()]
        public void TheDotProductOfTwoVectors()
        {
            // Given
            Vector vector1 = new Vector(1, 2, 3);
            Vector vector2 = new Vector(2, 3, 4);

            // Then
            double result = vector1.Dot(vector2);

            Assert.IsTrue(result.NearlyEquals(20));
        }

        [Test()]
        public void CrossProductOfTwoVectors()
        {
            // Given
            Vector vector1 = new Vector(1, 2, 3);
            Vector vector2 = new Vector(2, 3, 4);

            // Then
            Vector vector1CrossVector2 = vector1 * vector2;
            Vector vector2CrossVector1 = vector2 * vector1;

            Assert.IsTrue(vector1CrossVector2.NearlyEquals(new Vector(-1, 2, -1)));
            Assert.IsTrue(vector2CrossVector1.NearlyEquals(new Vector(1, -2, 1)));
        }
    }
}
