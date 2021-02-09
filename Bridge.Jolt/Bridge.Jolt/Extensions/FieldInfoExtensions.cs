using System;
using System.Reflection;

namespace Jolt.Extensions
{
    /// <summary>
    /// Provides handy extension methods for the <see cref="FieldInfo"/> class.
    /// </summary>
    public static class FieldInfoExtensions
    {
        /// <summary>
        /// Gets a custom attribute declared for this type that implements the specified type if available.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
        /// <param name="field">The type to look for the attribute at.</param>
        /// <param name="inherit">Whether we should include inherited attributes.</param>
        /// <returns>Returns the attribute if found, otherwise <c>null</c>.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this FieldInfo field, bool inherit) where TAttribute : Attribute
        {
            // Check sanity.
            if (field == null) throw new ArgumentNullException(nameof(field));

            // Try to find it.
            object[] items = field.GetCustomAttributes(typeof(TAttribute), inherit);
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] is TAttribute attr)
                    {
                        return attr;
                    }
                }
            }

            return null;
        }
    }
}
