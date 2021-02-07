using System;
using System.ComponentModel;
using System.Globalization;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Serves as the base class for all validation attributes.
    /// </summary>
    /// <remarks>
    /// This class enforces validation, based on the metadata that is associated with the data table.
    /// You can override this class to create custom validation attributes.
    /// </remarks>
    public abstract class ValidationAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttribute"/> class.
        /// </summary>
        protected ValidationAttribute() : 
            this(() => "Validation failed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttribute"/> class by using the error message to associate with a validation control.
        /// </summary>
        /// <param name="errorMessage"></param>
        protected ValidationAttribute(string errorMessage) 
            : this(() => errorMessage)
        {
        }

        /// <summary>
        /// Allows for providing a resource accessor function that will be used by the <see cref="ErrorMessageString"/>
        /// property to retrieve the error message.  An example would be to have something like
        /// CustomAttribute() : base( () =&gt; MyResources.MyErrorMessage ) {}.
        /// </summary>
        /// <param name="errorMessageAccessor">The <see cref="Func{T}"/> that will return an error message.</param>
        protected ValidationAttribute(Func<string> errorMessageAccessor)
        {
            // If null, will later be exposed as lack of error message to be able to construct accessor
            this._errorMessageResourceAccessor = errorMessageAccessor;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an error message to associate with a validation control if validation fails.
        /// </summary>
        /// <remarks>
        /// This property is the error message that you associate with the validation control. It is used for non-localized error messages.
        /// </remarks>
        public string ErrorMessage
        {
            get =>
                // If _errorMessage is not set, return the default. This is done to preserve behavior prior to the fix where ErrorMessage showed the non-null message to use.
                this._errorMessage ?? this._defaultErrorMessage;
            set
            {
                this._errorMessage = value;
                this._errorMessageResourceAccessor = null;
                this.CustomErrorMessageSet = true;

                // Explicitly setting ErrorMessage also sets DefaultErrorMessage if null. This prevents subsequent read of ErrorMessage from returning default.
                if (value == null)
                {
                    this._defaultErrorMessage = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the error message resource name to use in order to look up the <see cref="ErrorMessageResourceType"/> property value if validation fails.
        /// </summary>
        /// <value>The error message resource that is associated with a validation control.</value>
        public string ErrorMessageResourceName
        {
            get => this._errorMessageResourceName;
            set
            {
                this._errorMessageResourceName = value;
                this._errorMessageResourceAccessor = null;
                this.CustomErrorMessageSet = true;
            }
        }

        /// <summary>
        /// Gets or sets the resource type to use for error-message lookup if validation fails.
        /// </summary>
        /// <value>The type of error message that is associated with a validation control.</value>
        public Type ErrorMessageResourceType
        {
            get => this._errorMessageResourceType;
            set
            {
                this._errorMessageResourceType = value;
                this._errorMessageResourceAccessor = null;
                this.CustomErrorMessageSet = true;
            }
        }

        /// <summary>
        /// Gets the localized validation error message.
        /// </summary>
        /// <value>The localized validation error message.</value>
        /// <remarks>
        /// The error message string is obtained by evaluating the ErrorMessage property or by evaluating the <see cref="ErrorMessageResourceType"/> and
        /// <see cref="ErrorMessageResourceName"/> properties. The two cases are mutually exclusive. The second case is used if you want to display a
        /// localized error message.
        /// </remarks>
        protected string ErrorMessageString
        {
            get
            {
                this.SetupResourceAccessor();
                return this._errorMessageResourceAccessor();
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        public virtual bool RequiresValidationContext => false;

        #endregion

        #region Methods

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>An instance of the formatted error message.</returns>
        /// <remarks>
        /// This method formats an error message by using the <see cref="ErrorMessageString"/> property.
        /// This method appends the name of the data field that triggered the error to the formatted error message.
        /// You can customize how the error message is formatted by creating a derived class that overrides this method.
        /// </remarks>
        public virtual string FormatErrorMessage(string name)
        {
            return this.ErrorMessageString + " (" + name + ")";
        }

        /// <summary>
        /// Checks whether the specified value is valid with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="ValidationResult"/> class.</returns>
        /// <remarks>The <see cref="GetValidationResult"/> method checks validity without throwing an exception. </remarks>
        public ValidationResult GetValidationResult(object value, ValidationContext validationContext)
        {
            // Check sanity.
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            // Perform the validation.
            ValidationResult result = this.IsValid(value, validationContext);

            // If validation fails, we want to ensure we have a ValidationResult that guarantees it has an ErrorMessage
            if (result != null)
            {
                bool hasErrorMessage = (result != null) && !String.IsNullOrEmpty(result.ErrorMessage);
                if (!hasErrorMessage)
                {
                    string errorMessage = this.FormatErrorMessage(validationContext.DisplayName);
                    result = new ValidationResult(errorMessage, result.MemberNames);
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        /// <remarks>If you inherit from the <see cref="ValidationAttribute"/> class, you must implement the validation logic in this method.</remarks>
        public virtual bool IsValid(object value)
        {
            // Track that this method overload has not been overridden.
            if (!this._hasBaseIsValid)
            {
                this._hasBaseIsValid = true;
            }

            // Call overridden method.
            return this.IsValid(value, null) == null;
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns></returns>
        protected virtual ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // This means neither of the IsValid methods has been overriden, throw.
            if (this._hasBaseIsValid)
            {
                throw new NotImplementedException("IsValid is not implemented by derived class");
            }

            // Call overridden method.
            ValidationResult result = ValidationResult.Success;
            if (!this.IsValid(value))
            {
                string[] memberNames = validationContext.MemberName != null ? new string[] { validationContext.MemberName } : null;
                result = new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
            }
            return result;
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> and throws <see cref="ValidationException"/> if it is not.
        /// <para>
        /// The overloaded <see cref="Validate(Object, ValidationContext)"/> is the recommended entry point as it
        /// can provide additional context to the <see cref="ValidationAttribute"/> being validated.
        /// </para>
        /// </summary>
        /// <remarks>This base method invokes the <see cref="IsValid(Object)"/> method to determine whether or not the
        /// <paramref name="value"/> is acceptable.  If <see cref="IsValid(Object)"/> returns <c>false</c>, this base
        /// method will invoke the <see cref="FormatErrorMessage"/> to obtain a localized message describing
        /// the problem, and it will throw a <see cref="ValidationException"/>
        /// </remarks>
        /// <param name="value">The value to validate</param>
        /// <param name="name">The string to be included in the validation error message if <paramref name="value"/> is not valid</param>
        /// <exception cref="ValidationException"> is thrown if <see cref="IsValid(Object)"/> returns <c>false</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        public void Validate(object value, string name)
        {
            if (!this.IsValid(value))
            {
                throw new ValidationException(this.FormatErrorMessage(name), this, value);
            }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> and throws <see cref="ValidationException"/> if it is not.
        /// </summary>
        /// <remarks>This method invokes the <see cref="IsValid(Object, ValidationContext)"/> method 
        /// to determine whether or not the <paramref name="value"/> is acceptable given the <paramref name="validationContext"/>.
        /// If that method doesn't return <see cref="ValidationResult.Success"/>, this base method will throw
        /// a <see cref="ValidationException"/> containing the <see cref="ValidationResult"/> describing the problem.
        /// </remarks>
        /// <param name="value">The value to validate</param>
        /// <param name="validationContext">Additional context that may be used for validation.  It cannot be null.</param>
        /// <exception cref="ValidationException"> is thrown if <see cref="IsValid(Object, ValidationContext)"/> 
        /// doesn't return <see cref="ValidationResult.Success"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        /// <exception cref="NotImplementedException"> is thrown when <see cref="IsValid(Object, ValidationContext)" />
        /// has not been implemented by a derived class.
        /// </exception>
        public void Validate(object value, ValidationContext validationContext)
        {
            // Check sanity.
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            // Throw exception if not valid.
            ValidationResult result = this.GetValidationResult(value, validationContext);
            if (result != null)
            {
                throw new ValidationException(result, this, value);     // Convenience -- if implementation did not fill in an error message,
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Gets or sets and the default error message string. This message will be used if the user has not set <see cref="ErrorMessage"/>
        /// or the <see cref="ErrorMessageResourceType"/> and <see cref="ErrorMessageResourceName"/> pair.
        /// This property was added after the public contract for DataAnnotations was created.
        /// </summary>
        internal string DefaultErrorMessage
        {
            get => this._defaultErrorMessage;
            set
            {
                this._defaultErrorMessage = value;
                this._errorMessageResourceAccessor = null;
                this.CustomErrorMessageSet = true;
            }
        }

        /// <summary>
        /// A flag indicating whether a developer has customized the attribute's error message by setting any one of 
        /// <see cref="ErrorMessage"/>, <see cref="ErrorMessageResourceName"/>, <see cref="ErrorMessageResourceType"/> or <see cref="DefaultErrorMessage"/>.
        /// </summary>
        internal bool CustomErrorMessageSet { get; private set; }

        /// <summary>
        /// Validates the configuration of this attribute and sets up the appropriate error string accessor.
        /// This method bypasses all verification once the ResourceAccessor has been set.
        /// </summary>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        void SetupResourceAccessor()
        {
            if (this._errorMessageResourceAccessor == null)
            {
                string localErrorMessage = this.ErrorMessage;
                bool resourceNameSet = !String.IsNullOrEmpty(this._errorMessageResourceName);
                bool errorMessageSet = !String.IsNullOrEmpty(this._errorMessage);
                bool resourceTypeSet = this._errorMessageResourceType != null;
                bool defaultMessageSet = !String.IsNullOrEmpty(this._defaultErrorMessage);

                // The following combinations are illegal and throw InvalidOperationException:
                //   1) Both ErrorMessage and ErrorMessageResourceName are set, or
                //   2) None of ErrorMessage, ErrorMessageReourceName, and DefaultErrorMessage are set.
                if ((resourceNameSet && errorMessageSet) || !(resourceNameSet || errorMessageSet || defaultMessageSet))
                {
                    throw new InvalidOperationException("Cannot set both ErrorMessage and resource.");
                }

                // Must set both or neither of ErrorMessageResourceType and ErrorMessageResourceName
                if (resourceTypeSet != resourceNameSet)
                {
                    throw new InvalidOperationException("Need both ResourceType and ResourceName.");
                }

                // If set resource type (and we know resource name too), then go setup the accessor
                if (resourceNameSet)
                {
                    this.SetResourceAccessorByPropertyLookup();
                }
                else
                {
                    // Here if not using resource type/name -- the accessor is just the error message string,
                    // which we know is not empty to have gotten this far.
                    this._errorMessageResourceAccessor = delegate 
                    {
                        // We captured error message to local in case it changes before accessor runs
                        return localErrorMessage;
                    };
                }
            }
        }

        /// <summary>
        /// Configures so the errormessage comes from a property lookup.
        /// </summary>
        void SetResourceAccessorByPropertyLookup()
        {
            // Check that we have the required settings.
            if (this._errorMessageResourceType != null && !String.IsNullOrEmpty(this._errorMessageResourceName))
            {
                // Get the property requested.
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this._errorMessageResourceType);
                PropertyDescriptor property = properties.Find(this._errorMessageResourceName, true);

                // Property was not found.
                if (property == null)
                {
                    throw new InvalidOperationException(
                        String.Format(
                        CultureInfo.CurrentCulture,
                        "Resource type does not have property",
                        this._errorMessageResourceType.FullName,
                        this._errorMessageResourceName));
                }

                // The property was not a string.
                if (property.PropertyType != typeof(string))
                {
                    throw new InvalidOperationException(
                        String.Format(
                        CultureInfo.CurrentCulture,
                        "Resource property not a string type",
                        property.Name,
                        this._errorMessageResourceType.FullName));
                }

                // Use this property as the accessor.
                this._errorMessageResourceAccessor = delegate
                {
                    return (string)property.GetValue(null);
                };
            }
            else
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Need both ResourceType and ResourceName"));
            }
        }

        string _errorMessage;
        string _defaultErrorMessage;
        string _errorMessageResourceName;
        Type _errorMessageResourceType;
        Func<string> _errorMessageResourceAccessor;
        volatile bool _hasBaseIsValid;

        #endregion
    }
}
