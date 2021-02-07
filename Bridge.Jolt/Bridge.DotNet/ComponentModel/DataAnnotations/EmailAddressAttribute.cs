using System;
using System.Text.RegularExpressions;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Validates an email address.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class EmailAddressAttribute : DataTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddressAttribute"/> class.
        /// </summary>
        public EmailAddressAttribute() : base(DataType.EmailAddress)
        {
            this.DefaultErrorMessage = "Ikke gyldig epost- adresse.";
        }

        /// <summary>
        /// Determines whether the specified value matches the pattern of a valid email address.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;

            // Use RegEx implementation if it has been created, otherwise use a non RegEx version.
            if (_regex != null)
            {
                return valueAsString != null && _regex.Match(valueAsString).Length > 0;
            }
            else
            {
                int atCount = 0;

                foreach (char c in valueAsString)
                {
                    if (c == '@')
                    {
                        atCount++;
                    }
                }

                return 
                    valueAsString != null &&
                    atCount == 1 &&
                    valueAsString[0] != '@' &&
                    valueAsString[valueAsString.Length - 1] != '@';
            }
        }

        static Regex CreateRegEx()
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            const RegexOptions options = /*RegexOptions.Compiled | */ RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            // Set explicit regex match timeout, sufficient enough for email parsing.
            TimeSpan matchTimeout = TimeSpan.FromSeconds(2);
            return new Regex(pattern, options, matchTimeout);
        }

        static readonly Regex _regex = CreateRegEx();
    }
}
