using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a two-dimensional matrix.
    /// </summary>
    public class Matrix
    {
        #region Private Members

        /// <summary>
        /// The two-dimensional array holding the matrix values.
        /// </summary>
        private double[,] values;

        /// <summary>
        /// ToDoBre16
        /// </summary>
        private bool precomputeInverse = false;

        /// <summary>
        /// ToDoBre16
        /// </summary>
        private Matrix precomputedInverse;

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracer.Matrix"/> class.
        /// </summary>
        /// <param name="values">The <see cref="T:RayTracer.Matrix"/> values as a two-dimensional array.</param>
        public Matrix(double[,] values)
        {
            this.values = values;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracer.Matrix"/> class.
        /// </summary>
        /// <remarks>
        /// All values are initialized with 0.0.
        /// </remarks>
        /// <param name="rowCount">The number of rows of the matrix.</param>
        /// <param name="columnCount">The number of columns of the matrix.</param>
        public Matrix(int rowCount, int columnCount)
        {
            values = new double[rowCount, columnCount];
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an identity <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="size">The number of rows and columns of the identity <see cref="T:RayTracer.Matrix"/>.</param>
        /// <returns>The identity <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewIdentityMatrix(int size)
        {
            Matrix identity = new Matrix(size, size);

            for (int index = 0; index < size; index++)
            {
                identity[index, index] = 1;
            }

            return identity;
        }

        /// <summary>
        /// Gets a translation <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="x">The number of units to translate in x direction.</param>
        /// <param name="y">The number of units to translate in y direction.</param>
        /// <param name="z">The number of units to translate in z direction.</param>
        /// <returns>The translation <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewTranslationMatrix(double x, double y, double z)
        {
            Matrix translationMatrix = Matrix.NewIdentityMatrix(4);

            translationMatrix[0, 3] = x;
            translationMatrix[1, 3] = y;
            translationMatrix[2, 3] = z;

            return translationMatrix;
        }

        /// <summary>
        /// Gets a scaling <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="x">The scaling factor in x direction.</param>
        /// <param name="y">The scaling factor in y direction.</param>
        /// <param name="z">The scaling factor in z direction.</param>
        /// <returns>The scaling <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewScalingMatrix(double x, double y, double z)
        {
            Matrix scalingMatrix = new Matrix(4, 4);

            scalingMatrix[0, 0] = x;
            scalingMatrix[1, 1] = y;
            scalingMatrix[2, 2] = z;
            scalingMatrix[3, 3] = 1;

            return scalingMatrix;
        }

        /// <summary>
        /// Gets a <see cref="T:RayTracer.Matrix"/> for rotating around the x axis.
        /// </summary>
        /// <param name="radians">The angle, measured in radians.</param>
        /// <returns>The rotation <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewRotationXMatrix(double radians)
        {
            Matrix rotationXMatrix = new Matrix(4, 4);

            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            rotationXMatrix[0, 0] = 1;
            rotationXMatrix[1, 1] = cos;
            rotationXMatrix[1, 2] = -sin;
            rotationXMatrix[2, 1] = sin;
            rotationXMatrix[2, 2] = cos;
            rotationXMatrix[3, 3] = 1;

            return rotationXMatrix;
        }

        /// <summary>
        /// Gets a <see cref="T:RayTracer.Matrix"/> for rotating around the y axis.
        /// </summary>
        /// <param name="radians">The angle, measured in radians.</param>
        /// <returns>The rotation <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewRotationYMatrix(double radians)
        {
            Matrix rotationYMatrix = new Matrix(4, 4);

            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            rotationYMatrix[0, 0] = cos;
            rotationYMatrix[0, 2] = sin;
            rotationYMatrix[1, 1] = 1;
            rotationYMatrix[2, 0] = -sin;
            rotationYMatrix[2, 2] = cos;
            rotationYMatrix[3, 3] = 1;

            return rotationYMatrix;
        }

        /// <summary>
        /// Gets a <see cref="T:RayTracer.Matrix"/> for rotating around the z axis.
        /// </summary>
        /// <param name="radians">The angle, measured in radians.</param>
        /// <returns>The rotation <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewRotationZMatrix(double radians)
        {
            Matrix rotationZMatrix = new Matrix(4, 4);

            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            rotationZMatrix[0, 0] = cos;
            rotationZMatrix[0, 1] = -sin;
            rotationZMatrix[1, 0] = sin;
            rotationZMatrix[1, 1] = cos;
            rotationZMatrix[2, 2] = 1;
            rotationZMatrix[3, 3] = 1;

            return rotationZMatrix;
        }

        /// <summary>
        /// Gets a shearing <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="xy">x shearing factor in proportion to y.</param>
        /// <param name="xz">x shearing factor in proportion to z.</param>
        /// <param name="yx">y shearing factor in proportion to x.</param>
        /// <param name="yz">y shearing factor in proportion to z.</param>
        /// <param name="zx">z shearing factor in proportion to x.</param>
        /// <param name="zy">z shearing factor in proportion to y.</param>
        /// <returns>The shearing <see cref="T:RayTracer.Matrix"/>.</returns>
        public static Matrix NewShearingMatrix(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            Matrix shearingMatrix = Matrix.NewIdentityMatrix(4);

            shearingMatrix[0, 1] = xy;
            shearingMatrix[0, 2] = xz;
            shearingMatrix[1, 0] = yx;
            shearingMatrix[1, 2] = yz;
            shearingMatrix[2, 0] = zx;
            shearingMatrix[2, 1] = zy;

            return shearingMatrix;
        }

        /// <summary>
        /// Multiplies the current and the given <see cref="T:RayTracer.Matrix"/> instances.
        /// </summary>
        /// <param name="matrix">The <see cref="T:RayTracer.Matrix"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracer.Matrix"/> that is the product of the current and the given <see cref="T:RayTracer.Matrix"/> instances.</returns>
        public Matrix Multiply(Matrix matrix)
        {
            if (ColumnCount != matrix.RowCount)
            {
                throw new ArgumentException("The two matrices must have a compatible size.");
            }

            Matrix result = new Matrix(RowCount, matrix.ColumnCount);

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < matrix.ColumnCount; columnIndex++)
                {
                    double value = 0;

                    for (int dotProductIndex = 0; dotProductIndex < ColumnCount; dotProductIndex++)
                    {
                        value += this[rowIndex, dotProductIndex] * matrix[dotProductIndex, columnIndex];
                    }

                    result[rowIndex, columnIndex] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplies the current <see cref="T:RayTracer.Matrix"/> by the given <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="point"The point to multiply.></param>
        /// <returns>The <see cref="T:RayTracer.Point"/> that is the product of the current <see cref="T:RayTracer.Matrix"/> and the given <see cref="T:RayTracer.Point"/>.</returns>
        public Point Multiply(Point point)
        {
            if (ColumnCount != 4)
            {
                throw new ArgumentException("The matrix must have a compatible size.");
            }

            double[] result = new double[4];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                double value = 0;

                for (int dotProductIndex = 0; dotProductIndex < ColumnCount; dotProductIndex++)
                {
                    value += this[rowIndex, dotProductIndex] * point[dotProductIndex];
                }

                result[rowIndex] = value;
            }

            return new Point(result[0], result[1], result[2]);
        }

        /// <summary>
        /// Multiplies the current <see cref="T:RayTracer.Matrix"/> by the given <see cref="T:RayTracer.Vector"/>.
        /// </summary>
        /// <param name="vector"The vector to multiply.></param>
        /// <returns>The <see cref="T:RayTracer.Vector"/> that is the product of the current <see cref="T:RayTracer.Matrix"/> and the given <see cref="T:RayTracer.Vector"/>.</returns>
        public Vector Multiply(Vector vector)
        {
            if (ColumnCount != 4)
            {
                throw new ArgumentException("The matrix must have a compatible size.");
            }

            double[] result = new double[4];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                double value = 0;

                for (int dotProductIndex = 0; dotProductIndex < ColumnCount; dotProductIndex++)
                {
                    value += this[rowIndex, dotProductIndex] * vector[dotProductIndex];
                }

                result[rowIndex] = value;
            }

            return new Vector(result[0], result[1], result[2]);
        }

        /// <summary>
        /// Compares the current and the given <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <returns><c>true</c>, if the current and the given <see cref="T:RayTracer.Matrix"/> are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="matrix">The <see cref="T:RayTracer.Matrix"/> to compare.</param>
        public bool NearlyEquals(Matrix matrix)
        {
            if (RowCount != matrix.RowCount ||
                ColumnCount != matrix.ColumnCount)
            {
                return false;
            }

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    if (!this[rowIndex, columnIndex].NearlyEquals(matrix[rowIndex, columnIndex]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Transposes the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <returns>The transposed <see cref="T:RayTracer.Matrix"/>.</returns>
        public Matrix Transpose()
        {
            double[,] transposed = new double[ColumnCount, RowCount];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    transposed[columnIndex, rowIndex] = this[rowIndex, columnIndex];
                }
            }

            return new Matrix(transposed);
        }

        /// <summary>
        /// Gets the determinant of the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <returns>The determinant.</returns>
        public double GetDeterminant()
        {
            if (RowCount == 2 &&
                ColumnCount == 2)
            {
                double determinant = this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

                return determinant;
            }
            else
            {
                double determinant = 0.0;

                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    determinant += this[0, columnIndex] * GetCofactor(0, columnIndex);
                }

                return determinant;
            }
        }

        /// <summary>
        /// Gets the submatrix of the current <see cref="T:RayTracer.Matrix"/>, with
        /// the given row and column not being part of the submatrix.
        /// </summary>
        /// <param name="rowIndexToOmit">The index of the row which should not be part of the submatrix.</param>
        /// <param name="columnIndexToOmit">The index of the column which should not be part of the submatrix.</param>
        /// <returns>The submatrix.</returns>
        public Matrix GetSubmatrix(int rowIndexToOmit, int columnIndexToOmit)
        {
            double[,] submatrix = new double[RowCount - 1, ColumnCount - 1];

            int rowIndexSubmatrix = 0;

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                if (rowIndex != rowIndexToOmit)
                {
                    int columnIndexSubmatrix = 0;

                    for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                    {
                        if (columnIndex != columnIndexToOmit)
                        {
                            submatrix[rowIndexSubmatrix, columnIndexSubmatrix] = this[rowIndex, columnIndex];

                            columnIndexSubmatrix++;
                        }
                    }

                    rowIndexSubmatrix++;
                }
            }

            return new Matrix(submatrix);
        }

        /// <summary>
        /// Gets the minor.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The minor.</returns>
        public double GetMinor(int rowIndex, int columnIndex)
        {
            Matrix submatrix = GetSubmatrix(rowIndex, columnIndex);

            return submatrix.GetDeterminant();
        }

        /// <summary>
        /// Gets the cofactor.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The cofactor.</returns>
        public double GetCofactor(int rowIndex, int columnIndex)
        {
            double minor = GetMinor(rowIndex, columnIndex);

            if ((rowIndex + columnIndex) % 2 == 1)
            {
                return -minor;
            }
            else
            {
                return minor;
            }
        }

        /// <summary>
        /// A flag indicating if the current <see cref="T:RayTracer.Matrix"/> is invertible.
        /// </summary>
        /// <returns><c>true</c>, if the current matrix is invertible, <c>false</c> otherwise.</returns>
        public bool IsInvertible()
        {
            return !GetDeterminant().NearlyEquals(0);
        }

        /// <summary>
        /// Gets the inverse of the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <returns>The inverse.</returns>
        public Matrix GetInverse()
        {
            if (precomputeInverse)
            {
                return precomputedInverse;
            }

            double determinant = GetDeterminant();
            double[,] cofactors = new double[RowCount, ColumnCount];

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    cofactors[rowIndex, columnIndex] = GetCofactor(rowIndex, columnIndex);
                }
            }

            Matrix cofactorMatrix = new Matrix(cofactors);
            Matrix transposedCofactorMatrix = cofactorMatrix.Transpose();

            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    transposedCofactorMatrix[rowIndex, columnIndex] /= determinant;
                }
            }

            return transposedCofactorMatrix;
        }

        // Fluent API for transformations from Chapter 4 of "The Ray Tracer Challenge"
        /// <summary>
        /// Translates the current <see cref="T:RayTracer.Matrix"/> by the given number of units.
        /// </summary>
        /// <param name="x">The number of units to translate in x direction.</param>
        /// <param name="y">The number of units to translate in y direction.</param>
        /// <param name="z">The number of units to translate in z direction.</param>
        /// <returns>The translated matrix.</returns>
        public Matrix Translate(double x, double y, double z)
        {
            return NewTranslationMatrix(x, y, z) * this;
        }

        /// <summary>
        /// Scales the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="x">The scaling factor in x direction.</param>
        /// <param name="y">The scaling factor in y direction.</param>
        /// <param name="z">The scaling factor in z direction.</param>
        /// <returns>The scaled matrix.</returns>
        public Matrix Scale(double x, double y, double z)
        {
            return NewScalingMatrix(x, y, z) * this;
        }

        /// <summary>
        /// Rotates the current <see cref="T:RayTracer.Matrix"/> around the x axis.
        /// </summary>
        /// <param name="radians">The angle, measured in radians.</param>
        /// <returns>The rotated matrix.</returns>
        public Matrix RotateX(double radians)
        {
            return NewRotationXMatrix(radians) * this;
        }

        /// <summary>
        /// Rotates the current <see cref="T:RayTracer.Matrix"/> around the y axis.
        /// </summary>
        /// <param name="radians">The angle, measured in radians.</param>
        /// <returns>The rotated matrix.</returns>
        public Matrix RotateY(double radians)
        {
            return NewRotationYMatrix(radians) * this;
        }

        /// <summary>
        /// Rotates the current <see cref="T:RayTracer.Matrix"/> around the z axis.
        /// </summary>
        /// <param name="radians">The angle, measured in radians.</param>
        /// <returns>The rotated matrix.</returns>
        public Matrix RotateZ(double radians)
        {
            return NewRotationZMatrix(radians) * this;
        }

        /// <summary>
        /// Shears the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="xy">x shearing factor in proportion to y.</param>
        /// <param name="xz">x shearing factor in proportion to z.</param>
        /// <param name="yx">y shearing factor in proportion to x.</param>
        /// <param name="yz">y shearing factor in proportion to z.</param>
        /// <param name="zx">z shearing factor in proportion to x.</param>
        /// <param name="zy">z shearing factor in proportion to y.</param>
        /// <returns></returns>
        public Matrix Shear(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            return NewShearingMatrix(xy, xz, yx, yz, zx, zy) * this;
        }

        #endregion

        #region Public Operators

        /// <summary>
        /// Computes the product of <c>matrix</c> and <c>matrixToMultiply</c>, yielding a new <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="RayTracer.Matrix"/> to multiply.</param>
        /// <param name="matrixToMultiply">The <see cref="RayTracer.Matrix"/> to multiply.</param>
        /// <returns>The <see cref="T:Matrix"/> that is the <c>matrix</c> * <c>matrixToMultiply</c>.</returns>
        public static Matrix operator *(Matrix matrix, Matrix matrixToMultiply)
        {
            return matrix.Multiply(matrixToMultiply);
        }

        /// <summary>
        /// Computes the product of <c>matrix</c> and <c>pointToMultiply</c>, yielding a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="RayTracer.Matrix"/> to multiply.</param>
        /// <param name="pointToMultiply">The <see cref="RayTracer.Point"/> to multiply.</param>
        /// <returns>The <see cref="T:Point"/> that is the <c>matrix</c> * <c>pointToMultiply</c>.</returns>
        public static Point operator *(Matrix matrix, Point pointToMultiply)
        {
            return matrix.Multiply(pointToMultiply);
        }

        /// <summary>
        /// Computes the product of <c>matrix</c> and <c>vectorToMultiply</c>, yielding a new <see cref="T:RayTracer.Vector"/>.
        /// </summary>
        /// <param name="matrix">The <see cref="RayTracer.Matrix"/> to multiply.</param>
        /// <param name="vectorToMultiply">The <see cref="RayTracer.Vector"/> to multiply.</param>
        /// <returns>The <see cref="T:Vector"/> that is the <c>matrix</c> * <c>vectorToMultiply</c>.</returns>
        public static Vector operator *(Matrix matrix, Vector vectorToMultiply)
        {
            return matrix.Multiply(vectorToMultiply);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of rows of the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        public int RowCount
        {
            get
            {
                return values.GetLength(0);
            }
        }

        /// <summary>
        /// Gets the number of columns of the current <see cref="T:RayTracer.Matrix"/>.
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return values.GetLength(1);
            }
        }

        /// <summary>
        /// Gets or sets the value at the specified position.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="columnIndex">The column index.</param>
        public double this[int rowIndex, int columnIndex]
        {
            get
            {
                return values[rowIndex, columnIndex];
            }
            private set
            {
                values[rowIndex, columnIndex] = value;

                if (precomputeInverse)
                {
                    precomputedInverse = GetInverse();
                }
                else
                {
                    precomputedInverse = null;
                }
            }
        }

        /// <summary>
        /// ToDoBre16
        /// </summary>
        public bool PrecomputeInverse
        {
            get
            {
                return precomputeInverse;
            }
            set
            {
                if (value)
                {
                    precomputedInverse = GetInverse();
                }
                else
                {
                    precomputedInverse = null;
                }

                precomputeInverse = value;
            }
        }

        #endregion
    }
}