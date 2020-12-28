using NUnit.Framework;
using RayTracerLogic;

namespace RayTracerTests
{
    [TestFixture()]
    public class MatrixTests
    {
        [Test()]
        public void ConstructingAndInspectingA4x4Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 1, 2, 3, 4 },
                    { 5.5, 6.5, 7.5, 8.5 },
                    { 9, 10, 11, 12 },
                    { 13.5, 14.5, 15.5, 16.5 }
                }
            );

            // Then
            Assert.IsTrue(matrix[0, 0].NearlyEquals(1));
            Assert.IsTrue(matrix[0, 3].NearlyEquals(4));
            Assert.IsTrue(matrix[1, 0].NearlyEquals(5.5));
            Assert.IsTrue(matrix[1, 2].NearlyEquals(7.5));
            Assert.IsTrue(matrix[2, 2].NearlyEquals(11));
            Assert.IsTrue(matrix[3, 0].NearlyEquals(13.5));
            Assert.IsTrue(matrix[3, 2].NearlyEquals(15.5));
        }

        [Test()]
        public void A2x2MatrixOughtToBeRepresentable()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { -3, 5 },
                    { 1, -2 }
                }
            );

            // Then
            Assert.IsTrue(matrix[0, 0].NearlyEquals(-3));
            Assert.IsTrue(matrix[0, 1].NearlyEquals(5));
            Assert.IsTrue(matrix[1, 0].NearlyEquals(1));
            Assert.IsTrue(matrix[1, 1].NearlyEquals(-2));
        }

        [Test()]
        public void A3x3MatrixOughtToBeRepresentable()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { -3, 5, 0 },
                    { 1, -2, -7 },
                    { 0, 1, 1 }
                }
            );

            // Then
            Assert.IsTrue(matrix[0, 0].NearlyEquals(-3));
            Assert.IsTrue(matrix[1, 1].NearlyEquals(-2));
            Assert.IsTrue(matrix[2, 2].NearlyEquals(1));
        }

        [Test()]
        public void MatrixEqualityWithIdenticalMatrices()
        {
            // Given
            Matrix matrixOne = new Matrix(
                new double[,]
                {
                    { 1, 2, 3, 4 },
                    { 5, 6, 7, 8 },
                    { 9, 8, 7, 6 },
                    { 5, 4, 3, 2 }
                }
            );

            Matrix matrixTwo = new Matrix(
                new double[,]
                {
                    { 1, 2, 3, 4 },
                    { 5, 6, 7, 8 },
                    { 9, 8, 7, 6 },
                    { 5, 4, 3, 2 }
                }
            );

            // Then
            Assert.IsTrue(matrixOne.NearlyEquals(matrixTwo));
        }

        [Test()]
        public void MatrixEqualityWithDifferentMatrices()
        {
            // Given
            Matrix matrixOne = new Matrix(
                new double[,]
                {
                    { 1, 2, 3, 4 },
                    { 5, 6, 7, 8 },
                    { 9, 8, 7, 6 },
                    { 5, 4, 3, 2 }
                }
            );

            Matrix matrixTwo = new Matrix(
                new double[,]
                {
                    { 2, 3, 4, 5 },
                    { 6, 7, 8, 9 },
                    { 8, 7, 6, 5 },
                    { 4, 3, 2, 1 }
                }
            );

            // Then
            Assert.IsFalse(matrixOne.NearlyEquals(matrixTwo));
        }

        [Test()]
        public void MultiplyingTwoMatrices()
        {
            // Given
            Matrix matrixA = new Matrix(
                new double[,]
                {
                    { 1, 2, 3, 4 },
                    { 5, 6, 7, 8 },
                    { 9, 8, 7, 6 },
                    { 5, 4, 3, 2 }
                }
            );

            Matrix matrixB = new Matrix(
                new double[,]
                {
                    { -2, 1, 2, 3 },
                    { 3, 2, 1, -1 },
                    { 4, 3, 6, 5 },
                    { 1, 2, 7, 8 }
                }
            );

            // Then
            Assert.IsTrue((matrixA * matrixB).NearlyEquals(new Matrix(
                new double[,]
                {
                    { 20, 22, 50, 48 },
                    { 44, 54, 114, 108 },
                    { 40, 58, 110, 102 },
                    { 16, 26, 46, 42 }
                }
            )));
        }

        [Test()]
        public void MultiplyingTwoDifferentMatrices()
        {
            // Given
            Matrix matrixA = new Matrix(
                new double[,]
                {
                    { 1, 2, 3 },
                    { 5, 6, 7 },
                    { 9, 8, 7 },
                    { 5, 4, 3 },
                    { -3, -2, -1 }
                }
            );

            Matrix matrixB = new Matrix(
                new double[,]
                {
                    { -2, 1, 2, 3 },
                    { 3, 2, 1, -1 },
                    { 1, 2, 3, 4 }
                }
            );

            // Then
            Assert.IsTrue((matrixA * matrixB).NearlyEquals(new Matrix(
                new double[,]
                {
                    { 7, 11, 13, 13 },
                    { 15, 31, 37, 37 },
                    { 13, 39, 47, 47 },
                    { 5, 19, 23, 23 },
                    { -1, -9, -11, -11 }
                }
            )));
        }

        [Test()]
        public void MultiplyingIncompatibleMatrices()
        {
            // Given
            Matrix matrixA = new Matrix(
                new double[,]
                {
                    { 1 },
                    { 5 },
                    { 9 },
                }
            );

            Matrix matrixB = new Matrix(
                new double[,]
                {
                    { -2, 1, 2, 3 },
                    { 3, 2, 1, -1 },
                }
            );

            Matrix matrixC;

            // Throws exception
            Assert.Throws<System.ArgumentException>(() => matrixC = matrixA * matrixB);
        }

        [Test()]
        public void AMatrixMultipliedByATuple()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 1, 2, 3, 4 },
                    { 2, 4, 4, 2 },
                    { 8, 6, 4, 1 },
                    { 0, 0, 0, 1 }
                }
            );

            // Then
            Point tuple = new Point(1, 2, 3);
            Assert.IsTrue((matrix * tuple).NearlyEquals(new Point(18, 24, 33)));
        }

        [Test()]
        public void MultiplyingAMatrixByTheIdentityMatrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 0, 1, 2, 4 },
                    { 1, 2, 4, 8 },
                    { 2, 4, 8, 16 },
                    { 4, 8, 16, 32 }
                }
            );

            // Then
            Assert.IsTrue((matrix * Matrix.NewIdentityMatrix(4)).NearlyEquals(matrix));
        }

        [Test()]
        public void TransposingAMatrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 0, 9, 3, 0 },
                    { 9, 8, 0, 8 },
                    { 1, 8, 5, 3 },
                    { 0, 0, 5, 8 }
                }
            );

            // Then
            Assert.IsTrue(matrix.Transpose().NearlyEquals(new Matrix(
                new double[,]
                {
                    { 0, 9, 1, 0 },
                    { 9, 8, 8, 0 },
                    { 3, 0, 5, 5 },
                    { 0, 8, 3, 8 }
                }
            )));
        }

        [Test()]
        public void TransposingTheIdentityMatrix()
        {
            // Given
            Matrix matrix = Matrix.NewIdentityMatrix(4);

            // Then
            Assert.IsTrue(matrix.Transpose().NearlyEquals(matrix));
        }

        [Test()]
        public void CalculatingTheDeterminantOfA2x2Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 1, 5 },
                    { -3, 2 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetDeterminant().NearlyEquals(17));
        }

        [Test()]
        public void ASubmatrixOfA3x3MatrixIsA2x2Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 1, 5, 0 },
                    { -3, 2, 7 },
                    { 0, 6, -3 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetSubmatrix(0, 2).NearlyEquals(new Matrix(
                new double[,]
                {
                    { -3, 2 },
                    { 0, 6 }
                }
            )));
        }

        [Test()]
        public void ASubmatrixOfA4x4MatrixIsA3x3Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { -6, 1, 1, 6 },
                    { -8, 5, 8, 6 },
                    { -1, 0, 8, 2 },
                    { -7, 1, -1, 1 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetSubmatrix(2, 1).NearlyEquals(new Matrix(
                new double[,]
                {
                    { -6, 1, 6 },
                    { -8, 8, 6 },
                    { -7, -1, 1 }
                }
            )));
        }

        [Test()]
        public void CalculatingAMinorOfA3x3Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 3, 5, 0 },
                    { 2, -1, -7 },
                    { 6, -1, 5 }
                }
            );

            Matrix submatrix = matrix.GetSubmatrix(1, 0);

            // Then
            Assert.IsTrue(submatrix.GetDeterminant().NearlyEquals(25));
            Assert.IsTrue(matrix.GetMinor(1, 0).NearlyEquals(25));
        }

        [Test()]
        public void CalculatingACofactorOfA3x3Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 3, 5, 0 },
                    { 2, -1, -7 },
                    { 6, -1, 5 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetMinor(0, 0).NearlyEquals(-12));
            Assert.IsTrue(matrix.GetCofactor(0, 0).NearlyEquals(-12));
            Assert.IsTrue(matrix.GetMinor(1, 0).NearlyEquals(25));
            Assert.IsTrue(matrix.GetCofactor(1, 0).NearlyEquals(-25));
        }

        [Test()]
        public void CalculatingTheDeterminantOfA3x3Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 1, 2, 6 },
                    { -5, 8, -4 },
                    { 2, 6, 4 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetCofactor(0, 0).NearlyEquals(56));
            Assert.IsTrue(matrix.GetCofactor(0, 1).NearlyEquals(12));
            Assert.IsTrue(matrix.GetCofactor(0, 2).NearlyEquals(-46));
            Assert.IsTrue(matrix.GetDeterminant().NearlyEquals(-196));
        }

        [Test()]
        public void CalculatingTheDeterminantOfA4x4Matrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                   { -2, -8, 3, 5 },
                   { -3, 1, 7, 3 },
                   { 1, 2, -9, 6 },
                   { -6, 7, 7, -9 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetCofactor(0, 0).NearlyEquals(690));
            Assert.IsTrue(matrix.GetCofactor(0, 1).NearlyEquals(447));
            Assert.IsTrue(matrix.GetCofactor(0, 2).NearlyEquals(210));
            Assert.IsTrue(matrix.GetCofactor(0, 3).NearlyEquals(51));
            Assert.IsTrue(matrix.GetDeterminant().NearlyEquals(-4071));
        }

        [Test()]
        public void TestingAnInvertibleMatrixForInvertibilty()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 6, 4, 4, 4 },
                    { 5, 5, 7, 6 },
                    { 4, -9, 3, -7 },
                    { 9, 1, 7, -6 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetDeterminant().NearlyEquals(-2120));
            Assert.IsTrue(matrix.IsInvertible());
        }

        [Test()]
        public void TestingAnNonInvertibleMatrixForInvertibilty()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { -4, 2, -2, -3 },
                    { 9, 6, 2, 6},
                    { 0, -5, 1, -5 },
                    { 0, 0, 0, 0 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetDeterminant().NearlyEquals(0));
            Assert.IsFalse(matrix.IsInvertible());
        }

        [Test()]
        public void CalculatingTheInverseOfAMatrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { -5, 2, 6, -8 },
                    { 1, -5, 1, 8 },
                    { 7, 7, -6, -7 },
                    { 1, -3, 7, 4 }
                }
            );

            Matrix inversedMatrix = matrix.GetInverse();

            // Then
            Assert.IsTrue(matrix.GetDeterminant().NearlyEquals(532));
            Assert.IsTrue(matrix.GetCofactor(2, 3).NearlyEquals(-160));
            Assert.IsTrue(inversedMatrix[3, 2].NearlyEquals(-160.0 / 532));
            Assert.IsTrue(matrix.GetCofactor(3, 2).NearlyEquals(105));
            Assert.IsTrue(inversedMatrix[2, 3].NearlyEquals(105.0 / 532));
            Assert.IsTrue(inversedMatrix.NearlyEquals(new Matrix(
                new double[,]
                {
                    { 0.21805, 0.45113, 0.24060, -0.04511 },
                    { -0.80827, -1.45677, -0.44361, 0.52068 },
                    { -0.07895, -0.22368, -0.05263, 0.19737 },
                    { -0.52256, -0.81391, -0.30075, 0.30639 }
                }
            )));

        }

        [Test()]
        public void CalculatingTheInverseOfAnotherMatrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 8, -5, 9, 2 },
                    { 7, 5, 6, 1 },
                    { -6, 0, 9, 6 },
                    { -3, 0, -9, -4 }
                }    
            );

            // Then
            Assert.IsTrue(matrix.GetInverse().NearlyEquals(new Matrix(
                new double [,]
                {
                    { -0.15385, -0.15385, -0.28205, -0.53846 },
                    { -0.07692, 0.12308, 0.02564, 0.03077 },
                    { 0.35897, 0.35897, 0.43590, 0.92308 },
                    { -0.69231, -0.69231, -0.76923, -1.92308 }
                }
            )));
        }

        [Test()]
        public void CalculatingTheInverseOfThirdMatrix()
        {
            // Given
            Matrix matrix = new Matrix(
                new double[,]
                {
                    { 9, 3, 0, 9 },
                    { -5, -2, -6, -3 },
                    { -4, 9, 6, 4 },
                    { -7, 6, 6, 2 }
                }
            );

            // Then
            Assert.IsTrue(matrix.GetInverse().NearlyEquals(new Matrix(
                new double[,]
                {
                    { -0.04074, -0.07778, 0.14444, -0.22222 },
                    { -0.07778, 0.03333, 0.36667, -0.33333 },
                    { -0.02901, -0.14630, -0.10926, 0.12963 },
                    { 0.17778, 0.06667, -0.26667, 0.33333 }
                }
            )));
        }

        [Test()]
        public void MultiplyingAPrductByItsInverse()
        {
            // Given
            Matrix matrixA = new Matrix(
                new double[,]
                {
                    { 3, -9, 7, 3 },
                    { 3, -8, 2, -9 },
                    { -4, 4, 4, 1 },
                    { -6, 5, -1, 1 }
                }
            );

            Matrix matrixB = new Matrix(
                new double[,]
                {
                    { 8, 2, 2, 2 },
                    { 3, -1, 7, 0 },
                    { 7, 0, 5, 4 },
                    { 6, -2, 0, 5 }
                }
            );

            Matrix matrixC = matrixA * matrixB;

            // Then
            Assert.IsTrue((matrixC * matrixB.GetInverse()).NearlyEquals(matrixA));
        }
    }
}
