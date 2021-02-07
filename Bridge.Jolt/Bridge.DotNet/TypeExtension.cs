namespace System
{
    /// <summary>
    /// Polyfill for features missing in Bridge.NET for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Searches for the interface with the specified name.
        /// </summary>
        /// <param name="type">The type to get interface from.</param>
        /// <param name="name">The string containing the name of the interface to get. For generic interfaces, this is the mangled name.</param>
        /// <returns>
        /// An object representing the interface with the specified name, implemented or 
        /// inherited by the current <see cref="Type"/>, if found; otherwise, null.
        /// </returns>
        public static Type GetInterface(this Type type, string name)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            Type[] interfaces = type.GetInterfaces();
            int count = interfaces.Length;
            for (int i = 0; i < count; i++)
            {
                if (interfaces[i].Name == name)
                {
                    return interfaces[i];
                }
            }

            return null;
        }
    }
}
