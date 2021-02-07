namespace System.ComponentModel
{
    /// <summary>
    /// Specifies the display name for a property, event, or public void method which takes no arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisplayNameAttribute: Attribute
    {
        /// <summary>
        /// Initializes a new instance of the DisplayNameAttribute class.
        /// </summary>
        public DisplayNameAttribute() { }

        /// <summary>
        /// Initializes a new instance of the DisplayNameAttribute class using the display name.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public DisplayNameAttribute(string displayName)
        {
            this.DisplayNameValue = displayName;
        }

        /// <summary>
        /// Gets the display name for a property, event, or public void method that takes no arguments stored in this attribute.
        /// </summary>
        public virtual string DisplayName => this.DisplayNameValue;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        protected string DisplayNameValue { get; set; }
    }
}
