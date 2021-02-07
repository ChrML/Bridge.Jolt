namespace System.ComponentModel
{
    /// <summary>
    /// Specifies whether the property this attribute is bound to is read-only or read/write. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ReadOnlyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyAttribute"/> class.
        /// </summary>
        /// <param name="isReadOnly"></param>
        public ReadOnlyAttribute(bool isReadOnly)
        {
            this.IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Gets a value indicating whether the property this attribute is bound to is read-only.
        /// </summary>
        /// <value>true if the property this attribute is bound to is read-only; false if the property is read/write.</value>
        public bool IsReadOnly { get; }

        /// <summary>
        /// Specifies the default value for the ReadOnlyAttribute, which is No
        /// (that is, the property this attribute is bound to is read/write).
        /// This static field is read-only.
        /// </summary>
        public static readonly ReadOnlyAttribute Default = No;

        /// <summary>
        /// Specifies that the property this attribute is bound to is read/write and can be modified.
        /// This static field is read-only.
        /// </summary>
        public static readonly ReadOnlyAttribute No = new ReadOnlyAttribute(false);

        /// <summary>
        /// Specifies that the property this attribute is bound to is read-only and cannot be modified in the server explorer.
        /// This static field is read-only.
        /// </summary>
        public static readonly ReadOnlyAttribute Yes = new ReadOnlyAttribute(true);
    }
}
