using System;
using System.ComponentModel;

namespace Jolt.Extensions
{
    /// <summary>
    /// Provides handy extension methods for the <see cref="Enum"/> class.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets a custom attribute declared for this enum value that implements the specified type if available.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to get.</typeparam>
        /// <param name="value">The enum to look for the attribute at.</param>
        /// <returns>Returns the attribute if found, otherwise <c>null</c>.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            return type.GetField(name).GetCustomAttribute<TAttribute>(inherit: false);
        }

        /// <summary>
        /// Gets the description of an enum value if applied to the value using the <see cref="DescriptionAttribute"/>. <br/>
        /// If no such attribute exists, the declared name of the enum value will be used.
        /// </summary>
        /// <param name="value">The enum to look for the description for.</param>
        /// <returns>Returns the attribute if found, otherwise <c>null</c>.</returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            DescriptionAttribute attribute = value.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
