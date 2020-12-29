namespace RayTracerLogic
{
    /// <summary>
    /// Represents a 3-dimensional point.
    /// </summary>
    public class Point : Tuple
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracer.Point"/> class.
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
        /// Adds the current <see cref="T:RayTracer.Point"/> and the given <see cref="T:RayTracer.Vector"/>, resulting in a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <returns>The sum of the current <see cref="T:RayTracer.Point"/> and the given <see cref="T:RayTracer.Vector"/>.</returns>
        /// <param name="vector">The <see cref="T:RayTracer.Vector"/> to add.</param>
        public Point Add(Vector vector)
        {
            return new Point(X + vector.X, Y + vector.Y, Z + vector.Z);
        }

        /// <summary>
        /// Subtracts the given from the current <see cref="T:RayTracer.Point"/>, resulting in a new <see cref="T:RayTracer.Vector"/>.
        /// </summary>
        /// <returns>The difference between the current and given <see cref="T:RayTracer.Point"/>.</returns>
        /// <param name="point">The <see cref="T:RayTracer.Point"/> to subtract.</param>
        public Vector Subtract(Point point)
        {
            return new Vector(X - point.X, Y - point.Y, Z - point.Z);
        }

        /// <summary>
        /// Subtracts the given <see cref="T:RayTracer.Vector"/> from the current <see cref="T:RayTracer.Point"/>, resulting in a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <returns>The difference between the current <see cref="T:RayTracer.Point"/> and the given <see cref="T:RayTracer.Vector"/>.</returns>
        /// <param name="vector">The <see cref="T:RayTracer.Vector"/> to subtract.</param>
        public Point Subtract(Vector vector)
        {
            return new Point(X - vector.X, Y - vector.Y, Z - vector.Z);
        }

        /// <summary>
        /// Negates this <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <returns>The negated <see cref="T:RayTracer.Point"/>.</returns>
        public Point Negate()
        {
            return new Point(-X, -Y, -Z);
        }

        /// <summary>
        /// Multiples the current <see cref="T:RayTracer.Point"/> with the given scalar.
        /// </summary>
        /// <returns>The product of the current <see cref="T:RayTracer.Point"/> and the given scalar.</returns>
        /// <param name="scalar">The scalar.</param>
        public Point Multiply(double scalar)
        {
            return new Point(X * scalar, Y * scalar, Z * scalar);
        }

        /// <summary>
        /// Divides the current <see cref="T:RayTracer.Point"/> by the given scalar.
        /// </summary>
        /// <returns>The resulting <see cref="T:RayTracer.Point"/>.</returns>
        /// <param name="scalar">The scalar.</param>
        public Point Divide(double scalar)
        {
            return new Point(X / scalar, Y / scalar, Z / scalar);
        }

        /// <summary>
        /// ToDoBre16
        /// </summary>
        /// <param name="to"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public Matrix ViewTransform(Point to, Vector up)
        {
            Vector forward = (to - this).Normalize();
            Vector normalizedUp = up.Normalize();
            Vector left = forward * normalizedUp;
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

        /// <summary>
        /// Compares the current and the given points.
        /// </summary>
        /// <returns><c>true</c>, if the current and the given point are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="point">The point to compare.</param>
        public bool NearlyEquals(Point point)
        {
            return X.NearlyEquals(point.X) && Y.NearlyEquals(point.Y) && Z.NearlyEquals(point.Z) && W.NearlyEquals(point.W);
        }

        #endregion

        #region Public Operators

        /// <summary>
        /// Adds a <see cref="RayTracer.Point"/> to a <see cref="RayTracer.Vector"/>, yielding a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="point">The first <see cref="RayTracer.Point"/> to add.</param>
        /// <param name="vector">The second <see cref="RayTracer.Vector"/> to add.</param>
        /// <returns>The <see cref="T:RayTracer.Point"/> that is the sum of the values of <c>point</c> and <c>vector</c>.</returns>
        public static Point operator +(Point point, Vector vector)
        {
            return point.Add(vector);
        }

        /// <summary>
        /// Subtracts a <see cref="RayTracer.Point"/> from a <see cref="RayTracer.Point"/>, yielding a new <see cref="T:RayTracer.Vector"/>.
        /// </summary>
        /// <param name="point1">The <see cref="RayTracer.Point"/> to subtract from (the minuend).</param>
        /// <param name="point2">The <see cref="RayTracer.Point"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:RayTracer.Vector"/> that is the <c>point1</c> minus <c>point2</c>.</returns>
        public static Vector operator -(Point point1, Point point2)
        {
            return point1.Subtract(point2);
        }

        /// <summary>
        /// Subtracts a <see cref="RayTracer.Point"/> from a <see cref="RayTracer.Vector"/>, yielding a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="RayTracer.Point"/> to subtract from (the minuend).</param>
        /// <param name="vector">The <see cref="RayTracer.Vector"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:RayTracer.Point"/> that is the <c>point</c> minus <c>vector</c>.</returns>
        public static Point operator -(Point point, Vector vector)
        {
            return point.Subtract(vector);
        }

        /// <summary>
        /// Negates the given <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The negated <see cref="T:RayTracer.Point"/>.</returns>
        public static Point operator -(Point point)
        {
            return point.Negate();
        }

        /// <summary>
        /// Computes the product of <c>point</c> and <c>scalar</c>, yielding a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="RayTracer.Point"/> to multiply.</param>
        /// <param name="scalar">The <see cref="double"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracer.Point"/> that is the <c>point</c> * <c>scalar</c>.</returns>
        public static Point operator *(Point point, double scalar)
        {
            return point.Multiply(scalar);
        }

        /// <summary>
        /// Computes the division of <c>point</c> and <c>scalar</c>, yielding a new <see cref="T:RayTracer.Point"/>.
        /// </summary>
        /// <param name="point">The <see cref="RayTracer.Point"/> to divide (the divident).</param>
        /// <param name="scalar">The <see cref="double"/> to divide (the divisor).</param>
        /// <returns>The <see cref="T:RayTracer.Point"/> that is the <c>point</c> / <c>scalar</c>.</returns>
        public static Point operator /(Point point, double scalar)
        {
            return point.Divide(scalar);
        }

        #endregion
    }
}
