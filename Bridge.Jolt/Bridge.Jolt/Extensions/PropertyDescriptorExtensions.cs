using System;
using System.ComponentModel;

namespace Jolt.Extensions
{
    /// <summary>
    /// Provides handy extension methods for the <see cref="PropertyDescriptor"/> class.
    /// </summary>
    public static class PropertyDescriptorExtensions
    {
        /// <summary>
        /// Gets a custom attribute declared for this property that implements the specified type if available.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
        /// <param name="property">The property to look for the attribute at.</param>
        /// <returns>Returns the attribute if found, otherwise <c>null</c>.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this PropertyDescriptor property) where TAttribute : Attribute
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            return property.Attributes[typeof(TAttribute)] as TAttribute;
        }
    }
}
