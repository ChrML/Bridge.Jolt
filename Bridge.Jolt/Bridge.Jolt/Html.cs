using Retyped;
using System;

namespace Bridge.Jolt
{
    /// <summary>
    /// Provides features for common HTML- operations.
    /// </summary>
    public static class Html
    {
        #region Methods

        /// <summary>
        /// Gets an element from the document by its ID.
        /// </summary>
        /// <typeparam name="T">The type of element to get.</typeparam>
        /// <param name="id">The ID to get.</param>
        /// <returns>Returns the element if found.</returns>
        public static T GetById<T>(string id) where T : dom.HTMLElement
        {
            if (String.IsNullOrEmpty(id)) throw new ArgumentException("Id was null or empty.", nameof(id));
            return (T)dom.document.getElementById(id);
        }

        /// <summary>
        /// Gets an element from the document by its ID which is required to exist.
        /// </summary>
        /// <typeparam name="T">The type of element to get.</typeparam>
        /// <param name="id">The ID to get.</param>
        /// <returns>Returns the element.</returns>
        public static T GetByIdRequired<T>(string id) where T : dom.HTMLElement
        {
            if (String.IsNullOrEmpty(id)) throw new ArgumentException("Id was null or empty.", nameof(id));
            return (T)dom.document.getElementById(id) ?? throw new InvalidOperationException($"Required element with ID \"{id}\" was not found in the document.");
        }

        /// <summary>
        /// Creates a HTML div- element with an automatic CSS- class set based on the full type name of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type to base the CSS class name on.</typeparam>
        /// <param name="append">Optional additional string to append on the CSS.</param>
        /// <returns>Returns a div- element preconfigured with the given CSS- classname.</returns>
        public static dom.HTMLDivElement NewDiv<T>(string append = null)
        {
            return new dom.HTMLDivElement
            {
                className = Css.GetClass(typeof(T), append)
            };
        }

        /// <summary>
        /// Creates a HTML div- element with an automatic CSS- class set based on the full type name of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type to base the CSS class name on.</param>
        /// <param name="append">Optional additional string to append on the CSS.</param>
        /// <returns>Returns a div- element preconfigured with the given CSS- classname.</returns>
        public static dom.HTMLDivElement NewDiv(Type type, string append = null)
        {
            return new dom.HTMLDivElement
            {
                className = Css.GetClass(type, append)
            };
        }

        /// <summary>
        /// Creates a HTML img- element with an automatic CSS- class set based on the full type name of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type to base the CSS class name on.</typeparam>
        /// <param name="append">Optional additional string to append on the CSS.</param>
        /// <returns>Returns a img- element preconfigured with the given CSS- classname.</returns>
        public static dom.HTMLImageElement NewImg<T>(string append = null)
        {
            return new dom.HTMLImageElement
            {
                className = Css.GetClass(typeof(T), append)
            };
        }

        /// <summary>
        /// Creates a HTML img- element with an automatic CSS- class set based on the full type name of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type to base the CSS class name on.</param>
        /// <param name="append">Optional additional string to append on the CSS.</param>
        /// <returns>Returns a img- element preconfigured with the given CSS- classname.</returns>
        public static dom.HTMLImageElement NewImg(Type type, string append = null)
        {
            return new dom.HTMLImageElement
            {
                className = Css.GetClass(type, append)
            };
        }

        #endregion
    }
}
