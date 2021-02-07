using System;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Specifies the minimum length of array or string data allowed in a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
    public class MinLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinLengthAttribute"/> class.
        /// </summary>
        /// <param name="length">The length of the array or string data.</param>
        public MinLengthAttribute(int length)
        {
            this.Length = length;
        }

        /// <summary>
        /// Gets or sets the minimum allowable length of the array or string data.
        /// </summary>
        /// <value>The minimum allowable length of the array or string data.</value>
        public int Length { get; }

        /// <summary>
        /// Applies formatting to a specified error message.
        /// </summary>
        /// <param name="name">The name to include in the formatted string.</param>
        /// <returns>A localized string to describe the minimum acceptable length.</returns>
        public override string FormatErrorMessage(string name)
        {
            // An error occurred, so we know the value is less than the minimum
            return String.Format(System.Globalization.CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Length);
        }

        /// <summary>
        /// Determines whether a specified object is valid.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        /// <returns>true if the specified object is valid; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            // Check the lengths for legality
            this.EnsureLegalLengths();

            int length;

            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            if (value == null)
            {
                return true;
            }
            else
            {
                if (value is string str)
                {
                    length = str.Length;
                }
                else if (value is Array arr)
                {
                    // We expect a cast exception if a non-{string|array} property was passed in.
                    length = arr.Length;
                }
                else
                {
                    throw new InvalidOperationException("MinLengthAttribute cannot be used on properties that are not String or Array");
                }
            }

            return length >= this.Length;
        }

        #region Private

        /// <summary>
        /// Checks that Length has a legal value.
        /// </summary>
        /// <exception cref="InvalidOperationException">Length is less than zero.</exception>
        void EnsureLegalLengths()
        {
            if (this.Length < 0)
            {
                throw new InvalidOperationException("Invalid minimum length in data annotation.");
            }
        }

        #endregion
    }
}
