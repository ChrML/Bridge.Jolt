using System;

namespace Jolt.Extensions
{
    /// <summary>
    /// Provides handy extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a custom attribute declared for this type that implements the specified type if available.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
        /// <param name="type">The type to look for the attribute at.</param>
        /// <param name="inherit">Whether we should include inherited attributes.</param>
        /// <returns>Returns the attribute if found, otherwise <c>null</c>.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit) where TAttribute : Attribute
        {
            // Check sanity.
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Try to find it.
            object[] items = type.GetCustomAttributes(typeof(TAttribute), inherit);
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
