using Retyped;
using System;

namespace Jolt
{
    /// <summary>
    /// Provides features for common CSS- operations.
    /// </summary>
    public static class Css
    {
        #region Methods

        /// <summary>
        /// Gets a CSS class name based on the full typename of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type to base the CSS- class name on.</typeparam>
        /// <param name="append">Optional additional string to append on the CSS class name.</param>
        /// <returns>Returns a CSS class name.</returns>
        public static string GetClass<T>(string append = null)
        {
            return GetClass(typeof(T), append);
        }

        /// <summary>
        /// Gets a CSS class name based on the full typename of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type to base the CSS- class name on.</param>
        /// <param name="append">Optional additional string to append on the CSS class name.</param>
        /// <returns>Returns a CSS class name.</returns>
        public static string GetClass(Type type, string append = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            // This is a hot-path, we should avoid reflecting too often. Therefore, cache the type-names!
            dynamic dyn = type;
            string result = (string)dyn._css_cache;
            if (result == null)
            {
                result = GenerateClass(type);
                dyn._css_cache = result;
            }

            if (append != null)
            {
                return result + "-" + append;
            }
            else
            {
                return result;
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Generates a new class name by reflecting the input type.
        /// </summary>
        static string GenerateClass(Type type)
        {
            string result = type.Namespace.Replace('.', '-') + "-" + type.Name;
            int genericIndex = result.IndexOf('`');
            if (genericIndex >= 0)
            {
                result = result.Substring(0, genericIndex);
            }
            return result;
        }

        #endregion
    }
}
