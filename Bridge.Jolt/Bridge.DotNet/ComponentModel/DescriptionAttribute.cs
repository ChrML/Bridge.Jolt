namespace System.ComponentModel
{
    /// <summary>
    /// Specifies a description for a property or event.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.All)]
    public class DescriptionAttribute: Attribute
    {
        /// <summary>
        /// Initializes a new instance of the DescriptionAttribute class with no parameters.
        /// </summary>
        public DescriptionAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DescriptionAttribute class with a description.
        /// </summary>
        /// <param name="description"></param>
        public DescriptionAttribute(string description)
        {
            this.DescriptionValue = description;
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        public virtual string Description => this.DescriptionValue;

        /// <summary>
        /// Gets or sets the string stored as the description.
        /// </summary>
        protected string DescriptionValue { get; set; }
    }
}
