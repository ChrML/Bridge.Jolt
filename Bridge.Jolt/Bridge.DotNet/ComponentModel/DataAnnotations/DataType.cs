namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Represents an enumeration of the data types associated with data fields and parameters.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Represents a credit card number.
        /// </summary>
        CreditCard = 14,

        /// <summary>
        /// Represents a currency value.
        /// </summary>
        Currency = 6,

        /// <summary>
        /// Represents a custom data type.
        /// </summary>
        Custom = 0,

        /// <summary>
        /// Represents a date value.
        /// </summary>
        Date = 2,

        /// <summary>
        /// Represents an instant in time, expressed as a date and time of day.
        /// </summary>
        DateTime = 1,

        /// <summary>
        /// Represents a continuous time during which an object exists.
        /// </summary>
        Duration = 4,

        /// <summary>
        /// Represents an email address.
        /// </summary>
        EmailAddress = 10,

        /// <summary>
        /// Represents an HTML file.
        /// </summary>
        Html = 8,

        /// <summary>
        /// Represents a URL to an image.
        /// </summary>
        ImageUrl = 13,

        /// <summary>
        /// Represents multi-line text.
        /// </summary>
        MultilineText = 9,

        /// <summary>
        /// Represent a password value.
        /// </summary>
        Password = 11,

        /// <summary>
        /// Represents a phone number value.
        /// </summary>
        PhoneNumber = 5,

        /// <summary>
        /// Represents a postal code.
        /// </summary>
        PostalCode = 15,

        /// <summary>
        /// Represents text that is displayed.
        /// </summary>
        Text = 7,

        /// <summary>
        /// Represents a time value.
        /// </summary>
        Time = 3,

        /// <summary>
        /// Represents file upload data type.
        /// </summary>
        Upload = 16,

        /// <summary>
        /// Represents a URL value.
        /// </summary>
        Url = 12
    }
}
