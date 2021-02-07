using System;

namespace Jolt.Poly.DataAnnotations
{
    /// <summary>
    /// Indicates whether a data field is editable.
    /// </summary>
    /// <remarks>
    /// The presence of the <see cref="EditableAttribute"/> attribute on a data field indicates whether the user should be able to change the field's value.
    /// This class neither enforces nor guarantees that a field is editable.
    /// The underlying data store might allow the field to be changed regardless of the presence of this attribute.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EditableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableAttribute"/> class. 
        /// </summary>
        /// <param name="allowEdit">true to specify that field is editable; otherwise, false.</param>
        public EditableAttribute(bool allowEdit)
        {
            this.AllowEdit = allowEdit;
        }

        /// <summary>
        /// Gets a value that indicates whether a field is editable.
        /// </summary>
        public bool AllowEdit { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether an initial value is enabled.
        /// </summary>
        public bool AllowInitialValue { get; set; }
    }
}
