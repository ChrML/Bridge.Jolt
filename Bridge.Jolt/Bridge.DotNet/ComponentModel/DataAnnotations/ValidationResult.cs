using System;
using System.Collections.Generic;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Represents a container for the results of a validation request.
    /// <para>
    /// Use the static <see cref="ValidationResult.Success"/> to represent successful validation.
    /// </para>
    /// </summary>
    /// <seealso cref="ValidationAttribute.GetValidationResult"/>
    public class ValidationResult
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class by using an error message.
        /// </summary>
        /// <param name="errorMessage">The user-visible error message.  If null, <see cref="ValidationAttribute.GetValidationResult"/>
        /// will use <see cref="ValidationAttribute.FormatErrorMessage"/> for its error message.</param>
        public ValidationResult(string errorMessage): this(errorMessage, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class by using an error message and a list of members that have validation errors.
        /// </summary>
        /// <param name="errorMessage">The user-visible error message.  If null, <see cref="ValidationAttribute.GetValidationResult"/> 
        /// will use <see cref="ValidationAttribute.FormatErrorMessage"/> for its error message.</param>
        /// <param name="memberNames">The list of member names affected by this result.
        /// This list of member names is meant to be used by presentation layers to indicate which fields are in error.</param>
        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = memberNames ?? new string[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class by using a <see cref="ValidationResult"/> object (copy).
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="validationResult"/> is null.</exception>
        protected ValidationResult(ValidationResult validationResult)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException("validationResult");
            }

            this.ErrorMessage = validationResult.ErrorMessage;
            this.MemberNames = validationResult.MemberNames;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Represents the success of the validation (true if validation was successful; otherwise, false).
        /// </summary>
        /// <remarks>
        /// The <c>null</c> value is used to indicate success.  Consumers of <see cref="ValidationResult"/>s
        /// should compare the values to <see cref="ValidationResult.Success"/> rather than checking for null.
        /// </remarks>
        public static readonly ValidationResult Success;

        /// <summary>
        /// Gets the error message for the validation.
        /// </summary>
        /// <value>The error message for the validation. It may be null.</value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the collection of member names affected by this result.  The collection may be empty but will never be null.
        /// </summary>
        public IEnumerable<string> MemberNames { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the current validation result.
        /// </summary>
        /// <remarks>
        /// If the <see cref="ErrorMessage"/> is empty, it will still qualify as being specified, and therefore returned from <see cref="ToString"/>.
        /// </remarks>
        /// <returns>The <see cref="ErrorMessage"/> property value if specified, otherwise, the base <see cref="Object.ToString"/> result.</returns>
        public override string ToString()
        {
            return this.ErrorMessage ?? base.ToString();
        }

        #endregion Methods
    }
}
