namespace System.ComponentModel
{
    /// <summary>
    /// Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control set to Categorized mode.
    /// </summary>
    /// <remarks>
    /// A CategoryAttribute indicates the category to associate the associated property or event with, when listing properties or events in a PropertyGrid control set to Categorized mode.
    /// If a CategoryAttribute has not been applied to a property or event, the PropertyGrid associates it with the Misc category.
    /// A new category can be created for any name by specifying the name of the category in the constructor for the CategoryAttribute. 
    /// </remarks>
    [AttributeUsage(AttributeTargets.All)]
    public class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the CategoryAttribute class using the category name Default.
        /// </summary>
        public CategoryAttribute()
        {
            this.Category = "Default";
        }

        /// <summary>
        /// Initializes a new instance of the CategoryAttribute class using the specified category name.
        /// </summary>
        /// <param name="category">The name of the category</param>
        public CategoryAttribute(string category)
        {
            this.Category = category;
        }

        /// <summary>
        /// Gets the name of the category for the property or event that this attribute is applied to.
        /// </summary>
        public string Category { get; private set; }
    }
}
