using Retyped;
using System;
using System.Collections.Generic;

namespace Jolt
{
    /// <summary>
    /// Provides extension methods for <see cref="dom.HTMLElement"/>.
    /// </summary>
    public static class DomExt
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
        /// Appends a native HTML- element to the current element.
        /// </summary>
        /// <param name="element">The element to append another control to.</param>
        /// <param name="html">The HTML- code to append.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement Append(this dom.HTMLElement element, string html)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            dynamic template = dom.document.createElement("template");
            template.innerHTML = html;
            element.appendChild(template);
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
            dom.HTMLElement item = append.Dom ?? throw new InvalidOperationException("The HTML- element from the appended item was null.");
            element.appendChild(item);
            return element;
        }

        /// <summary>
        /// Appends a class that contains a HTML- element using dependency-injection and the current service provider.
        /// </summary>
        /// <typeparam name="T">Control kind to append.</typeparam>
        /// <param name="element">The element to append another control to.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement Append<T>(this dom.HTMLElement element) where T : class, IHtmlElement
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            T child = ActivatorUtilities.CreateInstance<T>(AppServices.Default);
            dom.HTMLElement item = child.Dom ?? throw new InvalidOperationException("The HTML- element in the created item was null.");

            element.appendChild(item);
            return element;
        }

        /// <summary>
        /// Appends an array of HTML- elements to the current element.
        /// </summary>
        /// <param name="element">The element to append another control to.</param>
        /// <param name="append">The element to append.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement AppendRange(this dom.HTMLElement element, params IHtmlElement[] append)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (append != null)
            {
                int length = append.Length;
                for (int i = 0; i < length; i++)
                {
                    element.Append(append[i]);
                }
            }

            return element;
        }

        /// <summary>
        /// Appends an array of HTML- elements to the current element.
        /// </summary>
        /// <param name="element">The element to append another control to.</param>
        /// <param name="append">The element to append.</param>
        /// <returns>Returns the value of <paramref name="element"/>.</returns>
        public static dom.HTMLElement AppendRange(this dom.HTMLElement element, IEnumerable<IHtmlElement> append)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (append != null)
            {
                foreach (IHtmlElement item in append)
                {
                    element.Append(item);
                }
            }

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
        /// Inserts a a child element at the given index of the parent element.
        /// </summary>
        /// <param name="element">The item to insert a new child for.</param>
        /// <param name="index">Index to insert item to.</param>
        /// <param name="child">The child element to insert.</param>
        public static dom.Element Insert(this dom.Element element, int index, dom.Element child)
        {
            // Check sanity.
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (child == null) throw new ArgumentNullException(nameof(child));

            // Validate the range of the argument.
            int count = Convert.ToInt32(element.children.length);
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be zero or a positive value.");
            }
            else if (index > count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be equal to or less than the number of elements in the parent.");
            }

            // Insert it the position requested.
            if (index == count)
            {
                element.appendChild(child);
            }
            else
            {
                uint i = Convert.ToUInt32(index);
                element.insertBefore(child, element.children[i]);
            }

            return element;
        }

        /// <summary>
        /// Inserts a a child control at the given index of the parent element.
        /// </summary>
        /// <param name="element">The item to insert a new child for.</param>
        /// <param name="index">Index to insert item to.</param>
        /// <param name="child">The child element to insert.</param>
        public static dom.HTMLElement Insert(this dom.HTMLElement element, int index, IHtmlElement child)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (child == null) throw new ArgumentNullException(nameof(child));

            element.Insert(index, child.Dom);
            return element;
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
        /// Replaces an existing child element with a new.
        /// </summary>
        /// <param name="element">The parent element to replace a child for.</param>
        /// <param name="oldItem">The old item to replace.</param>
        /// <param name="newItem">The new item to put at this position.</param>
        public static void ReplaceChild(this dom.HTMLElement element, dom.HTMLElement oldItem, dom.HTMLElement newItem)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (oldItem == null) throw new ArgumentNullException(nameof(oldItem));
            if (newItem == null) throw new ArgumentNullException(nameof(newItem));

            element.replaceChild(oldItem, newItem);
        }

        /// <summary>
        /// Removes all the child elements of the current element from the HTML dom tree.
        /// </summary>
        /// <param name="element">The element to remove all children from.</param>
        public static void RemoveChildren(this dom.HTMLElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            while (element.children.length > 0)
            {
                element.children[0].remove();
            }
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
        /// Sets the innerText- property of the current element. If the string is null or empty, we apply a placeholder.
        /// </summary>
        /// <param name="element">The element to apply a new text to.</param>
        /// <param name="text">The new text to apply.</param>
        public static void SetNullableInnerText(this dom.HTMLElement element, string text)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (String.IsNullOrEmpty(text))
            {
                element.innerHTML = "&nbsp;";
            }
            else
            {
                element.innerText = text;
            }
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
