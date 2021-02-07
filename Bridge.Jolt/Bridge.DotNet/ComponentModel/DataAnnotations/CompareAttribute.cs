using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Provides an attribute that compares two properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CompareAttribute : ValidationAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareAttribute"/> class.
        /// </summary>
        /// <param name="otherProperty"></param>
        public CompareAttribute(string otherProperty)
            : base("The fields must be equal.")
        {
            this.OtherProperty = otherProperty ?? throw new ArgumentNullException(nameof(otherProperty));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the property to compare with the current property.
        /// </summary>
        public string OtherProperty { get; private set; }

        /// <summary>
        /// Gets the display name of the other property.
        /// </summary>
        public string OtherPropertyDisplayName { get; internal set; }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        public override bool RequiresValidationContext => true;

        #endregion

        #region Methods

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name of the field that caused the validation failure.</param>
        /// <returns>The formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.OtherPropertyDisplayName ?? this.OtherProperty);
        }

        /// <summary>
        /// Determines whether a specified object is valid.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        /// <param name="validationContext">An object that contains information about the validation request.</param>
        /// <returns>true if value is valid; otherwise, false.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check sanity.
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            // Get the property that we should compare with.
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(validationContext.ObjectType);
            PropertyDescriptor otherPropertyInfo = properties.Find(this.OtherProperty, true);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Unknown property to compare to", this.OtherProperty));
            }

            // Get the other property value and compare them.
            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
            if (!Equals(value, otherPropertyValue))
            {
                if (this.OtherPropertyDisplayName == null)
                {
                    this.OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, this.OtherProperty);
                }
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        #endregion

        #region Private

        /// <summary>
        /// Gets the display name for a given property in a given type.
        /// </summary>
        /// <param name="containerType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            // Check sanity.
            if (containerType == null) throw new ArgumentNullException(nameof(containerType));
            if (String.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            // Find the other property.
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(containerType);
            PropertyDescriptor property = properties.Find(propertyName, true);
            if (property == null)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "Property not found", containerType.FullName, propertyName));
            }

            // Attempt to find a Display or DisplayName- attribute.
            IEnumerable<Attribute> attributes = property.Attributes.OfType<Attribute>();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.GetName();
            }
            DisplayNameAttribute displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            return propertyName;
        }

        #endregion
    }
}
