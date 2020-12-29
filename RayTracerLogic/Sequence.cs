using System;

namespace RayTracerLogic
{
    public class Sequence
    {
        #region Private Members

        private readonly double[] numbers;
        private int index;

        #endregion

        #region Public Constructors

        public Sequence(params double[] numbers)
        {
            this.numbers = numbers;
        }

        #endregion

        #region Public Properties

        public double Next
        {
            get
            {
                index = index % numbers.Length;

                return numbers[index++];
            }
        }

        #endregion
    }
}
