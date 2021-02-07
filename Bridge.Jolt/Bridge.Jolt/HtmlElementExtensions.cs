using Retyped;
using System;

namespace Jolt
{
    /// <summary>
    /// Provides helpful extensions methods that may be used on any HTML- element.
    /// </summary>
    public static class HtmlElementExtensions
    {
        /// <summary>
        /// Makes the given element fill all the available width/height.
        /// </summary>
        /// <param name="element">The element to make fit.</param>
        /// <returns>Returns the element.</returns>
        public static IHtmlElement FullWidthAndHeight(this IHtmlElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            dom.HTMLElement dom = element.DomElement;
            if (dom != null)
            {
                dom.style.height = "100%";
                dom.style.width = "100%";
                return element;
            }
            else
            {
                throw new InvalidOperationException("The HTML- element does not have a root.");
            }
        }

        /// <summary>
        /// Removes the HTML- element from the DOM- tree.
        /// </summary>
        /// <param name="element">The element to remove from the DOM- tree.</param>
        public static void Remove(this IHtmlElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            dom.HTMLElement dom = element.DomElement ?? throw new InvalidOperationException("The HTML- element does not have a root.");
            dom.Remove();
        }
    }
}
