using System;
using System.Globalization;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Specifies the minimum and maximum length of characters that are allowed in a data field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class StringLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringLengthAttribute"/> class.
        /// </summary>
        /// <param name="maximumLength">The maximum length, inclusive.  It may not be negative.</param>
        public StringLengthAttribute(int maximumLength) 
            : base(() => "Invalid length.")
        {
            this.MaximumLength = maximumLength;
        }

        /// <summary>
        /// Gets the maximum acceptable length of the string
        /// </summary>
        public int MaximumLength { get; }

        /// <summary>
        /// Gets or sets the minimum acceptable length of the string
        /// </summary>
        public int MinimumLength { get; set; }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(Object)"/>
        /// </summary>
        /// <remarks>This method returns <c>true</c> if the <paramref name="value"/> is null.  
        /// It is assumed the <see cref="RequiredAttribute"/> is used if the value may not be null.</remarks>
        /// <param name="value">The value to test.</param>
        /// <returns><c>true</c> if the value is null or less than or equal to the set maximum length</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override bool IsValid(object value)
        {
            // Check the lengths for legality
            this.EnsureLegalLengths();

            // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
            // We expect a cast exception if a non-string was passed in.
            int length = value == null ? 0 : ((string)value).Length;
            return value == null || (length >= this.MinimumLength && length <= this.MaximumLength);
        }

        /// <summary>
        /// Override of <see cref="ValidationAttribute.FormatErrorMessage"/>
        /// </summary>
        /// <param name="name">The name to include in the formatted string</param>
        /// <returns>A localized string to describe the maximum acceptable length</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override string FormatErrorMessage(string name)
        {
            this.EnsureLegalLengths();

            bool useErrorMessageWithMinimum = this.MinimumLength != 0 && !this.CustomErrorMessageSet;

            string errorMessage = useErrorMessageWithMinimum ? "Ugyldig lengde" : this.ErrorMessageString;

            // it's ok to pass in the minLength even for the error message without a {2} param since String.Format will just ignore extra arguments.
            return String.Format(CultureInfo.CurrentCulture, errorMessage, name, this.MaximumLength, this.MinimumLength);
        }

        /// <summary>
        /// Checks that MinimumLength and MaximumLength have legal values. Throws InvalidOperationException if not.
        /// </summary>
        void EnsureLegalLengths()
        {
            if (this.MaximumLength < 0)
            {
                throw new InvalidOperationException("Invalid max length");
            }

            if (this.MaximumLength < this.MinimumLength)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Min length is greater than max length", this.MaximumLength, this.MinimumLength));
            }
        }
    }
}
