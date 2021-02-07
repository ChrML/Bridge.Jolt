using System;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Specifies the name of an additional type to associate with a data field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DataTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeAttribute"/> class by using the specified type name.
        /// </summary>
        /// <param name="dataType">The name of the type to associate with the data field.</param>
        public DataTypeAttribute(DataType dataType)
        {
            this.DataType = dataType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTypeAttribute"/> class by using the specified field template name.
        /// </summary>
        /// <param name="customDataType">The name of the custom field template to associate with the data field.</param>
        /// <exception cref="ArgumentException"><paramref name="customDataType"/> is <see langword="null"/> or an empty string ("").</exception>
        public DataTypeAttribute(string customDataType)
        {
            if (String.IsNullOrEmpty(customDataType)) throw new ArgumentException("Custom datatype is null or empty.", nameof(customDataType));
            this.CustomDataType = customDataType;
        }

        /// <summary>
        /// Gets the name of custom field template that is associated with the data field.
        /// </summary>
        /// <value>The name of the custom field template that is associated with the data field.</value>
        public string CustomDataType { get; }

        /// <summary>
        /// Gets the type that is associated with the data field.
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        /// Returns the name of the type that is associated with the data field.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDataTypeName()
        {
            this.EnsureValidDataType();

            if (this.DataType == DataType.Custom)
            {
                // If it's a custom type string, use it as the template name
                return this.CustomDataType;
            }
            else
            {
                // If it's an enum, turn it into a string
                // Use the cached array with enum string values instead of ToString() as the latter is too slow
                return _dataTypeStrings[(int)this.DataType];
            }
        }

        /// <summary>
        /// Checks that the value of the data field is valid.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns><see langword="true"/> always.</returns>
        /// <remarks>This method implements validation logic that is specific to the <see cref="DataTypeAttribute"/> attribute.</remarks>
        public override bool IsValid(object value)
        {
            this.EnsureValidDataType();
            return true;
        }


        /// <summary>
        /// Throws an exception if this attribute is not correctly formed
        /// </summary>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        void EnsureValidDataType()
        {
            if (this.DataType == DataType.Custom && String.IsNullOrEmpty(this.CustomDataType))
            {
                throw new InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Datatype is empty."));
            }
        }


        static readonly string[] _dataTypeStrings = Enum.GetNames(typeof(DataType));
    }
}
