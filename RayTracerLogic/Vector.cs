using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents a 3-dimensional vector.
    /// </summary>
    public class Vector : Tuple
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RayTracerLogic.Vector"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public Vector(double x, double y, double z)
            : base(x, y, z, 0.0)
        {
            // Do nothing
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the current and the given <see cref="T:RayTracerLogic.Vector"/>, resulting in a new <see cref="T:RayTracerLogic.Point"/>.
        /// </summary>
        /// <returns>The sum of the current and the given <see cref="T:RayTracerLogic.Vector"/>.</returns>
        /// <param name="vector">The <see cref="T:RayTracerLogic.Vector"/> to add.</param>
        public Vector Add(Vector vector)
        {
            return new Vector(X + vector.X, Y + vector.Y, Z + vector.Z);
        }

        /// <summary>
        /// Subtracts the given from the current <see cref="T:RayTracerLogic.Vector"/>, resulting in a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The difference between the current and given <see cref="T:RayTracerLogic.Vector"/>.</returns>
        /// <param name="vector">The <see cref="T:RayTracerLogic.Vector"/> to subtract.</param>
        public Vector Subtract(Vector vector)
        {
            return new Vector(X - vector.X, Y - vector.Y, Z - vector.Z);
        }

        /// <summary>
        /// Negates this <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The negated <see cref="T:RayTracerLogic.Vector"/>.</returns>
        public Vector Negate()
        {
            return new Vector(-X, -Y, -Z);
        }

        /// <summary>
        /// Multiples the current <see cref="T:RayTracerLogic.Vector"/> with the given scalar.
        /// </summary>
        /// <returns>The product of the current <see cref="T:RayTracerLogic.Vector"/> and the given scalar.</returns>
        /// <param name="scalar">The scalar.</param>
        public Vector Multiply(double scalar)
        {
            return new Vector(X * scalar, Y * scalar, Z * scalar);
        }

        /// <summary>
        /// Divides the current <see cref="T:RayTracerLogic.Vector"/> by the given scalar.
        /// </summary>
        /// <returns>The resulting <see cref="T:RayTracerLogic.Vector"/>.</returns>
        /// <param name="scalar">The scalar.</param>
        public Vector Divide(double scalar)
        {
            return new Vector(X / scalar, Y / scalar, Z / scalar);
        }

        /// <summary>
        /// Calculates and returns the magnitude of this <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The magnitude.</returns>
        public double GetMagnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// Normalizes the current <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The normalized <see cref="T:RayTracerLogic.Vector"/>.</returns>
        public Vector Normalize()
        {
            double magnitude = GetMagnitude();

            return new Vector(X / magnitude, Y / magnitude, Z / magnitude);
        }

        /// <summary>
        /// Calculates the dot product between the current and the given <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The dot product.</returns>
        /// <param name="vector">The vector to multiply.</param>
        public double Dot(Vector vector)
        {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        /// <summary>
        /// Calculates the cross product between the current and the given <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <returns>The cross product.</returns>
        /// <param name="vector">The vector to multiply.</param>
        public Vector Cross(Vector vector)
        {
            return new Vector(Y * vector.Z - Z * vector.Y, Z * vector.X - X * vector.Z, X * vector.Y - Y * vector.X);
        }

        /// <summary>
        /// Gets the reflection vector.
        /// </summary>
        /// <param name="normal">The normal vector.</param>
        /// <returns>The reflection vector.</returns>
        public Vector GetReflect(Vector normal)
        {
            return this - normal * 2 * Dot(normal);
        }

        /// <summary>
        /// Compares the current and the given vectors.
        /// </summary>
        /// <returns><c>true</c>, if the current and the given vectors are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="vector">The vector to compare.</param>
        public bool NearlyEquals(Vector vector)
        {
            return X.NearlyEquals(vector.X) && Y.NearlyEquals(vector.Y) && Z.NearlyEquals(vector.Z) && W.NearlyEquals(vector.W);
        }

        #endregion

        #region Public Operators

        /// <summary>
        /// Adds a <see cref="RayTracerLogic.Vector"/> to a <see cref="RayTracerLogic.Vector"/>, yielding a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="vector1">The first <see cref="RayTracerLogic.Vector"/> to add.</param>
        /// <param name="vector2">The second <see cref="RayTracerLogic.Vector"/> to add.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/> that is the sum of the values of <c>vector1</c> and <c>vector2</c>.</returns>
        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return vector1.Add(vector2);
        }

        /// <summary>
        /// Subtracts a <see cref="RayTracerLogic.Vector"/> from a <see cref="RayTracerLogic.Vector"/>, yielding a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="vector1">The <see cref="RayTracerLogic.Vector"/> to subtract from (the minuend).</param>
        /// <param name="vector2">The <see cref="RayTracerLogic.Vector"/> to subtract (the subtrahend).</param>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/> that is the <c>vector1</c> minus <c>vector2</c>.</returns>
        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return vector1.Subtract(vector2);
        }

        /// <summary>
        /// Negates the given <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The negated <see cref="T:RayTracerLogic.Vector"/>.</returns>
        public static Vector operator -(Vector vector)
        {
            return vector.Negate();
        }

        /// <summary>
        /// Computes the scalar product of <c>vector</c> and <c>scalar</c>, yielding a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="vector">The <see cref="RayTracerLogic.Vector"/> to multiply.</param>
        /// <param name="scalar">The <see cref="double"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/> that is the <c>vector</c> * <c>scalar</c>.</returns>
        public static Vector operator *(Vector vector, double scalar)
        {
            return vector.Multiply(scalar);
        }

        /// <summary>
        /// Computes the cross product of <c>vector1</c> and <c>vector2</c>, yielding a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="vector1">The <see cref="RayTracerLogic.Vector"/> to multiply.</param>
        /// <param name="vector2">The <see cref="RayTracerLogic.Vector"/> to multiply.</param>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/> that is the <c>vector1</c> * <c>vector2</c>.</returns>
        public static Vector operator *(Vector vector1, Vector vector2)
        {
            return vector1.Cross(vector2);
        }

        /// <summary>
        /// Computes the division of <c>vector</c> and <c>scalar</c>, yielding a new <see cref="T:RayTracerLogic.Vector"/>.
        /// </summary>
        /// <param name="vector">The <see cref="RayTracerLogic.Vector"/> to divide (the divident).</param>
        /// <param name="scalar">The <see cref="double"/> to divide (the divisor).</param>
        /// <returns>The <see cref="T:RayTracerLogic.Vector"/> that is the <c>vector</c> / <c>scalar</c>.</returns>
        public static Vector operator /(Vector vector, double scalar)
        {
            return vector.Divide(scalar);
        }

        #endregion
    }
}
