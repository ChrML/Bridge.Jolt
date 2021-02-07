namespace System.ComponentModel
{
    /// <summary>
    /// Specifies the editor to use to change a property. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public sealed class EditorAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorAttribute"/> class with the default editor, which is no editor.
        /// </summary>
        public EditorAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorAttribute"/> class with the type name and base type name of the editor.
        /// </summary>
        /// <param name="typeName">The fully qualified type name of the editor.</param>
        /// <param name="baseTypeName">The fully qualified type name of the base class or interface to use as a lookup key for the editor.</param>
        public EditorAttribute(string typeName, string baseTypeName)
        {
            this.EditorTypeName = typeName;
            this.EditorBaseTypeName = baseTypeName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorAttribute"/> class with the type name and the base type.
        /// </summary>
        /// <param name="typeName">The fully qualified type name of the editor.</param>
        /// <param name="baseType">The <see cref="Type"/> of the base class or interface to use as a lookup key for the editor.</param>
        public EditorAttribute(string typeName, Type baseType)
        {
            this.EditorTypeName = typeName;
            this.EditorBaseTypeName = baseType?.AssemblyQualifiedName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorAttribute"/> class with the type and the base type.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> that represents the type of the editor.</param>
        /// <param name="baseType">The <see cref="Type"/> of the base class or interface to use as a lookup key for the editor.</param>
        public EditorAttribute(Type type, Type baseType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            this.EditorTypeName = type.AssemblyQualifiedName;
            this.EditorBaseTypeName = baseType?.AssemblyQualifiedName;
        }

        /// <summary>
        /// Gets the name of the base class or interface serving as a lookup key for this editor.
        /// </summary>
        public string EditorBaseTypeName { get; } = String.Empty;

        /// <summary>
        /// Gets the name of the editor class in the <see cref="Type.AssemblyQualifiedName"/>  format.
        /// </summary>
        public string EditorTypeName { get; } = String.Empty;
    }
}
