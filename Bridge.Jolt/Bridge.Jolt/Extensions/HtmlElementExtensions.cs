using Retyped;
using System;

namespace Bridge.Jolt
{
    /// <summary>
    /// Provides extension methods for <see cref="dom.HTMLElement"/>.
    /// </summary>
    public static class HtmlElementExtensions
    {
        /// <summary>
        /// Adds a CSS- class to the current element.
        /// </summary>
        /// <param name="element">The element to add a CSS- class to.</param>
        /// <param name="className">The CSS class name to add.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement AddClass(this dom.HTMLElement element, string className)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (String.IsNullOrEmpty(className)) throw new ArgumentException("Class was null or empty.", nameof(className));

            element.classList.add(className);
            return element;
        }

        /// <summary>
        /// Appends a native HTML- element to the current element.
        /// </summary>
        /// <param name="element">The element to append another control to.</param>
        /// <param name="append">The element to append.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement Append(this dom.HTMLElement element, dom.HTMLElement append)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (append == null) throw new ArgumentNullException(nameof(append));
            element.appendChild(append);
            return element;
        }

        /// <summary>
        /// Appends a class that contains a HTML- element to the current element.
        /// </summary>
        /// <param name="element">The element to append another control to.</param>
        /// <param name="append">The element to append.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement Append(this dom.HTMLElement element, IHtmlElement append)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (append == null) throw new ArgumentNullException(nameof(append));
            dom.HTMLElement item = append.DomElement ?? throw new InvalidOperationException("The HTML- element from the appended item was null.");
            element.appendChild(item);
            return element;
        }

        /// <summary>
        /// Gets the CSS class used by <see cref="SwitchCssClass(dom.HTMLElement, String, String)"/> in the given context.
        /// </summary>
        /// <param name="element">The element to get CSS class from.</param>
        /// <param name="context">The context to get CSS class from.</param>
        /// <returns></returns>
        public static string GetSwitchCssClass(this dom.HTMLElement element, string context = null)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (context != null)
            {
                context = "_css_switch_" + context;
            }
            else
            {
                context = "_css_switch";
            }

            return element[context] as string;
        }

        /// <summary>
        /// Removes the current element from the HTML dom tree.
        /// </summary>
        /// <param name="element"></param>
        public static void Remove(this dom.HTMLElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            element.remove();
        }

        /// <summary>
        /// Removes a CSS class from the current element.
        /// </summary>
        /// <param name="element">The element to remove CSS class from.</param>
        /// <param name="className">The CSS class to remove.</param>
        public static void RemoveClass(this dom.HTMLElement element, string className)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            element.classList.remove(className);
        }

        /// <summary>
        /// Adds or removes a CSS- class to the current element.
        /// </summary>
        /// <param name="element">The element to add or remove a CSS- class for.</param>
        /// <param name="className">The CSS class name to add or remove.</param>
        /// <param name="active">If the class should be set, or removed.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement SetClass(this dom.HTMLElement element, string className, bool active)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (active)
            {
                element.AddClass(className);
            }
            else
            {
                element.RemoveClass(className);
            }

            return element;
        }

        /// <summary>
        /// Sets if the given HTML- element should be displayed or not.
        /// </summary>
        /// <param name="element">The element to change display of.</param>
        /// <param name="display"><c>true</c> will show the element.</param>
        /// <remarks>
        /// Setting <paramref name="display"/> to false will set the "display" style to "none".
        /// </remarks>
        public static void SetDisplay(this dom.HTMLElement element, bool display)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            element.style.display = display ? null : "none";
        }

        /// <summary>
        /// Switches CSS class for an element by replacing the current class set by this method with a new provided class.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="newClass"></param>
        /// <param name="context">Optional context if multiple CSS classes are used.</param>
        public static void SwitchCssClass(this dom.HTMLElement element, string newClass, string context = null)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            // Skip if nothing to do.
            string current = element.GetSwitchCssClass();
            if (newClass == current)
            {
                return;
            }

            // Remove the old class.
            if (!String.IsNullOrEmpty(current))
            {
                element.RemoveClass(current);
            }

            // Apply the new class.
            if (!String.IsNullOrEmpty(newClass))
            {
                element.AddClass(newClass);
            }
            element[context] = newClass;
        }
    }
}
