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
        /// Gets if this element is currently mounted to the DOM- tree.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsMounted(this IHtmlElement element)
        {
            // Check sanity.
            if (element == null) throw new ArgumentNullException(nameof(element));
            dom.Node inner = element.DomElement;
            if (inner == null) throw new InvalidOperationException("The HTML- element does not have a root.");

            // Rooted if we find the document node.
            do
            {
                if (inner == dom.document)
                {
                    return true;
                }

                inner = inner.parentNode;
            }
            while (inner != null);

            return false;
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
