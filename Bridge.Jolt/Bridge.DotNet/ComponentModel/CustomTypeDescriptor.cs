namespace System.ComponentModel
{
    /// <summary>
    /// Provides a simple default implementation of the <see cref="ICustomTypeDescriptor"/> interface.
    /// </summary>
    public abstract class CustomTypeDescriptor : ICustomTypeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeDescriptor"/> class.
        /// </summary>
        protected CustomTypeDescriptor()
        {
        }

        /// <summary>
        /// Returns a collection of custom attributes for the type represented by this type descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual AttributeCollection GetAttributes() => AttributeCollection.Empty;

        /// <summary>
        /// Returns the fully qualified name of the class represented by this type descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual string GetClassName() => null;

        /// <summary>
        /// Returns the name of the class represented by this type descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual string GetComponentName() => null;

        /// <summary>
        /// Returns the property descriptor for the default property of the object represented by this type descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual PropertyDescriptor GetDefaultProperty() => null;

        /// <summary>
        /// Returns an editor of the specified type that is to be associated with the class represented by this type descriptor.
        /// </summary>
        /// <param name="editorBaseType"></param>
        /// <returns></returns>
        public virtual object GetEditor(Type editorBaseType) => null;

        /// <summary>
        /// Returns a collection of property descriptors for the object represented by this type descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual PropertyDescriptorCollection GetProperties() => PropertyDescriptorCollection.Empty;

        /// <summary>
        /// Returns a collection of property descriptors for the object represented by this type descriptor.
        /// </summary>
        /// <returns></returns>
        public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes) => PropertyDescriptorCollection.Empty;

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public virtual object GetPropertyOwner(PropertyDescriptor pd) => null;
    }
}
