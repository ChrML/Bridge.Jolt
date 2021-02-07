using System;
using System.Globalization;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Used for specifying a range constraint
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RangeAttribute : ValidationAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class.
        /// </summary>
        RangeAttribute()
            : base(() => "Feltet må ha en verdi mellom {1} og {2}.")
        {
        }

        /// <summary>
        /// Constructor that takes double minimum and maximum values
        /// </summary>
        /// <param name="minimum">The minimum value, inclusive</param>
        /// <param name="maximum">The maximum value, inclusive</param>
        public RangeAttribute(double minimum, double maximum)
            : this()
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.OperandType = typeof(double);
        }

        /// <summary>
        /// Constructor that takes integer minimum and maximum values
        /// </summary>
        /// <param name="minimum">The minimum value, inclusive</param>
        /// <param name="maximum">The maximum value, inclusive</param>
        public RangeAttribute(int minimum, int maximum)
            : this()
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.OperandType = typeof(int);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the minimum value for the range
        /// </summary>
        public object Minimum { get; private set; }

        /// <summary>
        /// Gets the maximum value for the range
        /// </summary>
        public object Maximum { get; private set; }

        /// <summary>
        /// Gets the type of the <see cref="Minimum"/> and <see cref="Maximum"/> values (e.g. Int32, Double, or some custom type)
        /// </summary>
        public Type OperandType { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the value falls between min and max, inclusive.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> means the <paramref name="value"/> is valid</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override bool IsValid(object value)
        {
            if (this.OperandType == typeof(int))
            {
                return IsValidInt(Convert.ToInt32(value), (int)this.Minimum, (int)this.Maximum);
            }
            else if (this.OperandType == typeof(double))
            {
                return IsValidDouble(Convert.ToDouble(value), (double)this.Minimum, (double)this.Maximum);
            }
            else
            {
                throw new NotSupportedException("Validation type not supported.");
            }
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
        /// </summary>
        /// <remarks>This override exists to provide a formatted message describing the minimum and maximum values</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>A localized string describing the minimum and maximum values</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Minimum, this.Maximum);
        }

        #endregion

        #region Privates

        static bool IsValidDouble(double value, double minimum, double maximum)
        {
            return value >= minimum && value <= maximum;
        }

        static bool IsValidInt(int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }

        #endregion
    }
}
