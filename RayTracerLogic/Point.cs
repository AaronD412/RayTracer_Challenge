namespace RayTracerLogic
{
    /// <summary>
    /// Represents a 3-dimensional point.
    /// </summary>
    public class Point : Tuple
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the Point class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public Point(double x, double y, double z)
            : base(x, y, z, 1.0)
        {
            // Do nothing
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the current Point to a Vector, resulting in a new Point .
        /// </summary>
        /// <returns>The sum of the current <see cref="T:RayTracerLogic.Point"/> and the given <see cref="T:RayTracerLogic.Vector"/>.</returns>
        /// <param name="vector">The <see cref="T:RayTracerLogic.Vector"/> to add.</param>
        public Point Add(Vector vector)
        {
            return new Point(X + vector.X, Y + vector.Y, Z + vector.Z);
        }

        /// <summary>
        /// Subtracts the given from the current <see cref="T:RayTracerLogic.Point"/>, resulting in a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The difference between the current and given <see cref="T:RayTracerLogic.Point"/>.</returns>
        /// <param name="point">The <see cref="T:RayTracerLogic.Point"/> to subtract.</param>
        public Vector Subtract(Point point)
        {
            return new Vector(X - point.X, Y - point.Y, Z - point.Z);
        }

        /// <summary>
        /// Subtracts the given <see cref="T:RayTracerLogic.Vector"/> from the current <see cref="T:RayTracerLogic.Point"/>, resulting in a new <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <returns>The difference between the current <see cref="T:RayTracerLogic.Point"/> and the given <see cref="T:RayTracerLogic.Vector"/>.</returns>
        /// <param name="vector">The <see cref="T:RayTracerLogic.Vector"/> to subtract.</param>
        public Point Subtract(Vector vector)
        {
            return new Point(X - vector.X, Y - vector.Y, Z - vector.Z);
        }

        /// <summary>
        /// Negates this <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <returns>The negated <see cref="T:RayTracerLogic.Point"/>.</returns>
        public Point Negate()
        {
            return new Point(-X, -Y, -Z);
        }

        /// <summary>
        /// Multiples the current <see cref="T:RayTracerLogic.Point"/> with the given scalar.
        /// </summary>
        /// <returns>The product of the current <see cref="T:RayTracerLogic.Point"/> and the given scalar.</returns>
        /// <param name="scalar">The scalar.</param>
        public Point Multiply(double scalar)
        {
            return new Point(X * scalar, Y * scalar, Z * scalar);
        }

        /// <summary>
        /// Divides the current <see cref="T:RayTracerLogic.Point"/> by the given scalar.
        /// </summary>
        /// <returns>The resulting <see cref="T:RayTracerLogic.Point"/>.</returns>
        /// <param name="scalar">The scalar.</param>
        public Point Divide(double scalar)
        {
            return new Point(X / scalar, Y / scalar, Z / scalar);
        }

        /// <summary>
        /// Compares the current and the given points.
        /// </summary>
        /// <returns><c>true</c>, if the current and the given point are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="point">The point to compare.</param>
        public bool NearlyEquals(Point point)
        {
            return X.NearlyEquals(point.X) && Y.NearlyEquals(point.Y) && Z.NearlyEquals(point.Z) && W.NearlyEquals(point.W);
        }

        public Matrix GetViewTransform(Point to, Vector up)
        {
            Vector forward = (to - this).Normalize();
            Vector left = forward * up.Normalize();

            Vector trueUp = left * forward;

            Matrix orientation = new Matrix(
                new double[,]
                {
                    { left.X, left.Y, left.Z, 0 },
                    { trueUp.X, trueUp.Y, trueUp.Z, 0 },
                    { -forward.X, -forward.Y, -forward.Z, 0 },
                    { 0, 0, 0, 1 }
                }
            );

            return orientation * Matrix.NewTranslationMatrix(-X, -Y, -Z);
        }

        #endregion

        #region Public Operators

        /// <summary>
        /// Adds a <see cref="RayTracerLogic.Point"/> to a <see cref="RayTracerLogic.Vector"/>, yielding a new <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <param name="point">The first <see cref="RayTracerLogic.Point"/> to add.</param>
        /// <param name="vector">The second <see cref="RayTracerLogic.Vector"/> to add.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Point"/> that is the sum of the values of <c>point</c> and <c>vector</c>.</returns>
        public static Point operator +(Point point, Vector vector)
        {
            return point.Add(vector);
        }

        /// <summary>
        /// Subtracts a <see cref="RayTracerLogic.Point"/> from a <see cref="RayTracerLogic.Point"/>, yielding a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="point1">The <see cref="RayTracerLogic.Point"/> to subtract from (the minuend).</param>
        /// <param name="point2">The <see cref="RayTracerLogic.Point"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/> that is the <c>point1</c> minus <c>point2</c>.</returns>
        public static Vector operator -(Point point1, Point point2)
        {
            return point1.Subtract(point2);
        }

        /// <summary>
        /// Subtracts a <see cref="RayTracerLogic.Point"/> from a <see cref="RayTracerLogic.Vector"/>, yielding a new <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="RayTracerLogic.Point"/> to subtract from (the minuend).</param>
        /// <param name="vector">The <see cref="RayTracerLogic.Vector"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:RayTracerLogic.Point"/> that is the <c>point</c> minus <c>vector</c>.</returns>
        public static Point operator -(Point point, Vector vector)
        {
            return point.Subtract(vector);
        }

        /// <summary>
        /// Negates the given <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The negated <see cref="T:RayTracerLogic.Point"/>.</returns>
        public static Point operator -(Point point)
        {
            return point.Negate();
        }

        /// <summary>
        /// Computes the product of <c>point</c> and <c>scalar</c>, yielding a new <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="RayTracerLogic.Point"/> to multiply.</param>
        /// <param name="scalar">The <see cref="double"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Point"/> that is the <c>point</c> * <c>scalar</c>.</returns>
        public static Point operator *(Point point, double scalar)
        {
            return point.Multiply(scalar);
        }

        /// <summary>
        /// Computes the division of <c>point</c> and <c>scalar</c>, yielding a new <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="RayTracerLogic.Point"/> to divide (the divident).</param>
        /// <param name="scalar">The <see cref="double"/> to divide (the divisor).</param>
        /// <returns>The <see cref="T:RayTracerLogic.Point"/> that is the <c>point</c> / <c>scalar</c>.</returns>
        public static Point operator /(Point point, double scalar)
        {
            return point.Divide(scalar);
        }

        #endregion
    }
}
