using System.Linq;
using System.Reflection;

namespace System.ComponentModel
{
    /// <summary>
    /// Provides information about the characteristics for a component, such as its attributes, properties, and events.
    /// </summary>
    public sealed class TypeDescriptor
    {
        #region Methods

        /// <summary>
        /// Creates and dynamically binds a property descriptor to a type, using the specified property name, type, and attribute array.
        /// </summary>
        /// <param name="componentType">The <see cref="Type"/> of the component that the property is a member of.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The <see cref="Type"/> of the property.</param>
        /// <param name="attributes">The new attributes for this property.</param>
        /// <returns>A <see cref="PropertyDescriptor"/> that is bound to the specified type and that has the specified metadata attributes merged with the existing metadata attributes.</returns>
        public static PropertyDescriptor CreateProperty(Type componentType, string name, Type type, params Attribute[] attributes)
        {
            return new ReflectPropertyDescriptor(componentType, name, type, attributes);
        }

        /// <summary>
        /// Returns a collection of attributes for the specified type of component.
        /// </summary>
        /// <param name="componentType">The <see cref="Type"/> of the target component.</param>
        /// <returns>An <see cref="AttributeCollection"/> with the attributes for the type of the component. If the component is null, this method returns an empty collection.</returns>
#pragma warning disable IDE0060 // Remove unused parameter
        public static AttributeCollection GetAttributes(Type componentType)
        {
            // Not implemented
            return AttributeCollection.Empty;
        }
#pragma warning restore IDE0060 // Remove unused parameter

        /// <summary>
        /// Returns the collection of properties for a specified component using the default type descriptor.
        /// </summary>
        /// <param name="component">A component to get the properties for.</param>
        /// <param name="noCustomTypeDesc">true to not consider custom type description information; otherwise, false.</param>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> with the properties for a specified component.</returns>
        public static PropertyDescriptorCollection GetProperties(object component, bool noCustomTypeDesc)
        {
            // Check sanity.
            if (component == null) throw new ArgumentNullException(nameof(component));
            Type componentType = component.GetType();
            if (componentType == null) throw new ArgumentException("Component has no type information.", nameof(component));

            // Use properties from custom type descriptor if any.
            if (!noCustomTypeDesc && component is ICustomTypeDescriptor componentCustomType)
            {
                PropertyDescriptorCollection propertiesCustom = componentCustomType.GetProperties();
                return propertiesCustom;
            }

            // If not, use the generic type.
            return GetProperties(componentType);
        }

        /// <summary>
        /// Returns the collection of properties for a specified component.
        /// </summary>
        /// <param name="component">A component to get the properties for.</param>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> with the properties for the specified component.</returns>
        public static PropertyDescriptorCollection GetProperties(object component) => GetProperties(component, false);

        /// <summary>
        /// Returns the collection of properties for a specified type of component.
        /// </summary>
        /// <param name="componentType">A <see cref="Type"/> that represents the component to get properties for.</param>
        /// <returns>A <see cref="PropertyDescriptorCollection"/> with the properties for a specified type of component.</returns>
        /// <remarks>Call this version of this method only when you do not have an instance of the object.</remarks>
        public static PropertyDescriptorCollection GetProperties(Type componentType)
        {
            const string cacheKey = "__internal_cachedPropertyDescriptors";

            // Check sanity.
            if (componentType == null) throw new ArgumentNullException(nameof(componentType));

            // Since we run in Javascript, we can cache the type descriptor collection inside the Type- object and hence prevent recreating it every time.
            // This will make our type-descriptor interface very efficient for databinding because we don't need to use reflection each iteration.
            PropertyDescriptorCollection cachedPropertyDescriptors = (PropertyDescriptorCollection)componentType[cacheKey];
            if (cachedPropertyDescriptors != null)
            {
                return cachedPropertyDescriptors;
            }

            // Create properties from reflection and store it in cache for next time.
            else
            {
                PropertyDescriptorCollection propertiesReflection = CreatePropertyDescriptorsUsingReflection(componentType);
                componentType[cacheKey] = propertiesReflection;
                return propertiesReflection;
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Default constructor to prevent initializing an instance of this class.
        /// </summary>
        TypeDescriptor()
        {
        }

        /// <summary>
        /// Create property descriptors for the given type using reflection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static PropertyDescriptorCollection CreatePropertyDescriptorsUsingReflection(Type type)
        {
            // Check arguments.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Reflect the properties of the type.
            BindingFlags bindingFlags = BindingFlags.Public |  BindingFlags.Instance;    // Same flags as CoreCLR uses.
            PropertyInfo[] props = type.GetProperties(bindingFlags);
            int length = props.Length;

            // Create one property descriptor for each property.
            PropertyDescriptor[] descriptors = new PropertyDescriptor[length];
            for (int i = 0; i < length; i++)
            {
                PropertyInfo info = props[i];
                Attribute[] attributes = info.GetCustomAttributes().OfType<Attribute>().ToArray();
                descriptors[i] = new ReflectPropertyDescriptor(type, info.Name, info.PropertyType, attributes);
            }
            return new PropertyDescriptorCollection(descriptors, true);
        }

        #endregion
    }
}
