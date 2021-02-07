namespace System.ComponentModel
{
    /// <summary>
    /// Provides an interface that supplies dynamic custom type information for an object.
    /// </summary>
    public interface ICustomTypeDescriptor
    {
        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>An <see cref="AttributeCollection"/> containing the attributes for this object.</returns>
        AttributeCollection GetAttributes();

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>The class name of the object, or null if the class does not have a name.</returns>
        string GetClassName();

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>The name of the object, or null if the object does not have a name.</returns>
        string GetComponentName();

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="PropertyDescriptor"/> that represents the default property for this object, or null if this object does not have properties.</returns>
        PropertyDescriptor GetDefaultProperty();

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType"></param>
        /// <returns>A <see cref="Type"/> that represents the editor for this object.</returns>
        object GetEditor(Type editorBaseType);

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> that represents the properties for this component instance.</returns>
        PropertyDescriptorCollection GetProperties();

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="Attribute"/> that is used as a filter.</param>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> that represents the filtered properties for this component instance.</returns>
        PropertyDescriptorCollection GetProperties(Attribute[] attributes);

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A <see cref="PropertyDescriptor"/> that represents the property whose owner is to be found.</param>
        /// <returns>An <see cref="Object"/> that represents the owner of the specified property.</returns>
        object GetPropertyOwner(PropertyDescriptor pd);
    }
}
