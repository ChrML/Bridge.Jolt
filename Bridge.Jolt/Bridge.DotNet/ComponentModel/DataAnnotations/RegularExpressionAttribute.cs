using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Specifies that a data field value in ASP.NET Dynamic Data must match the specified regular expression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RegularExpressionAttribute : ValidationAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularExpressionAttribute"/> class.
        /// </summary>
        /// <param name="pattern">The regular expression that is used to validate the data field value.</param>
        /// <exception cref="ArgumentNullException">pattern is null</exception>
        public RegularExpressionAttribute(string pattern): base(() => "Validering feilet")
        {
            this.Pattern = pattern;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the amount of time in milliseconds to execute a single matching operation before the operation times out.
        /// </summary>
        /// <value>The amount of time in milliseconds to execute a single matching operation.</value>
        public int MatchTimeoutInMilliseconds
        {
            get => this._matchTimeoutInMilliseconds;
            set
            {
                this._matchTimeoutInMilliseconds = value;
                this._matchTimeoutSet = true;
            }
        }

        /// <summary>
        /// Gets the regular expression pattern to use.
        /// </summary>
        public string Pattern { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Formats the error message to display if the regular expression validation fails.
        /// </summary>
        /// <remarks>This override provide a formatted error message describing the pattern</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>The localized message to present to the user</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern"/> is not a valid regular expression.</exception>
        public override string FormatErrorMessage(string name)
        {
            this.SetupRegex();
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Pattern);
        }

        /// <summary>
        /// Checks whether the value entered by the user matches the regular expression pattern.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> if the given value matches the current regular expression pattern</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern"/> is not a valid regular expression.</exception>
        public override bool IsValid(object value)
        {
            this.SetupRegex();

            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            string stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);
            if (String.IsNullOrEmpty(stringValue))
            {
                return true;
            }

            // We are looking for an exact match, not just a search hit. This matches what the RegularExpressionValidator control does.
            Match m = this.Regex.Match(stringValue);
            return m.Success && m.Index == 0 && m.Length == stringValue.Length;
        }

        #endregion

        #region Private

        /// <summary>
        /// Sets up the <see cref="Regex"/> property from the <see cref="Pattern"/> property.
        /// </summary>
        /// <exception cref="ArgumentException"> is thrown if the current <see cref="Pattern"/> cannot be parsed</exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> thrown if <see cref="MatchTimeoutInMilliseconds" /> is negative (except -1),
        /// zero or greater than approximately 24 days </exception>
        void SetupRegex()
        {
            if (this.Regex == null)
            {
                if (String.IsNullOrEmpty(this.Pattern))
                {
                    throw new InvalidOperationException("Empty regex pattern");
                }

                if (!this._matchTimeoutSet)
                {
                    this.MatchTimeoutInMilliseconds = GetDefaultTimeout();
                }

                this.Regex = this.MatchTimeoutInMilliseconds == -1
                    ? new Regex(this.Pattern)
                    : this.Regex = new Regex(this.Pattern, default(RegexOptions), TimeSpan.FromMilliseconds((double)this.MatchTimeoutInMilliseconds));
            }
        }


        static int GetDefaultTimeout() =>  2000;
        Regex Regex { get; set; }

        int _matchTimeoutInMilliseconds;
        bool _matchTimeoutSet;
        
        #endregion
    }
}
