using System;

namespace RayTracerLogic
{
    /// <summary>
    /// Represents the Extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Public Methods

        /// <summary>
        /// Checks if the current and the given double values are nearly equal.
        /// </summary>
        /// <returns><c>true</c>, if the current and the given <see cref="double"/> value are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="currentValue">The current <see cref="double"/> value.</param>
        /// <param name="value">The <see cref="double"/> value to compare.</param>
        public static bool NearlyEquals(this double currentValue, double value)
        {
            return Math.Abs(currentValue - value) < Constants.Epsilon;
        }

        #endregion
    }
}
