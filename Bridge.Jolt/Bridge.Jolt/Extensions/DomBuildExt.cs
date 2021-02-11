using Bridge;
using Retyped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jolt
{

    /// <summary>
    /// Provides extension methods helpful for declaratively building a DOM- tree.
    /// </summary>
    [Reflectable(false)]
    public static class DomBuildExt
    {
        /// <summary>
        /// Creates a new DIV- element and wraps a new instance of the provided type into it. The new type is created using the default service provider.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static dom.HTMLElement New<TControl>(this dom.HTMLElement parent, string className, Action<TControl> config = null)
           where TControl : class, IHtmlElement
        {
            // Check sanity.
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            // Create using service provider.
            TControl instance = ActivatorUtilities.CreateInstance<TControl>(AppServices.Default);
            return parent.Wrap(instance, className, config);
        }

        /// <summary>
        /// Creates a new control of the provided type and appends it to the current element. The new type is created using the default service provider.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="parent"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static dom.HTMLElement New<TControl>(this dom.HTMLElement parent, Action<TControl> config = null)
           where TControl : class, IHtmlElement
        {
            // Check sanity.
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            // Create using service provider.
            TControl instance = ActivatorUtilities.CreateInstance<TControl>(AppServices.Default);
            parent.Append(instance);
            config?.Invoke(instance);
            return parent;
        }

        /// <summary>
        /// Creates a new DIV- element and wraps a new instance of the provided type into it. The new type is created using the default service provider.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        /// <param name="createInstance"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static dom.HTMLElement New<TControl>(this dom.HTMLElement parent, string className, Func<IServices, TControl> createInstance, Action<TControl> config = null)
            where TControl : class, IHtmlElement
        {
            // Check sanity.
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (createInstance == null) throw new ArgumentNullException(nameof(createInstance));

            // Append the provided instance.
            TControl instance = createInstance(AppServices.Default);
            return parent.Wrap(instance, className, config);
        }

        /// <summary>
        /// Creates a new DIV- element and wraps a new instance of the provided type into it. The new type is created using the default service provider.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="parent"></param>
        /// <param name="createInstance"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static dom.HTMLElement New<TControl>(this dom.HTMLElement parent, Func<IServices, TControl> createInstance, Action<TControl> config = null)
            where TControl : class, IHtmlElement
        {
            // Check sanity.
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (createInstance == null) throw new ArgumentNullException(nameof(createInstance));

            // Append the provided instance.
            TControl instance = createInstance(AppServices.Default);
            parent.Append(instance);
            config?.Invoke(instance);
            return parent;
        }

        /// <summary>
        /// Creates a new DIV- element and appends it to this element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="className"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static T NewDiv<T>(this T parent, string className, Action<dom.HTMLDivElement> callback = null)
            where T : dom.HTMLElement
        {
            // Check sanity.
            if (parent == null) throw new ArgumentNullException(nameof(className));

            // Apply the parent's classname plus our own.
            dom.HTMLDivElement div = new dom.HTMLDivElement();
            string parentClass = parent.className;
            if (parentClass != null && parentClass != "")
            {
                div.className = parent.className + "-" + className;
            }
            parent.appendChild(div);

            // Invoke the callback for configuring children.
            callback?.Invoke(div);
            return parent;
        }

        /// <summary>
        /// Creates a new DIV- element and appends it to this element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static T NewDiv<T>(this T parent, Action<dom.HTMLDivElement> callback = null)
            where T : dom.HTMLElement
        {
            return parent.NewDiv(className: null, callback: callback);
        }

        /// <summary>
        /// Wraps an existing control into a DIV- element.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="parent"></param>
        /// <param name="instance"></param>
        /// <param name="className"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static dom.HTMLElement Wrap<TControl>(this dom.HTMLElement parent, TControl instance, string className, Action<TControl> config = null)
            where TControl : class, IHtmlElement
        {
            // Check sanity.
            if (parent == null) throw new ArgumentNullException(nameof(className));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            // Append the provided instance.
            return parent.NewDiv(className, div =>
            {
                div.Append(instance);
                config?.Invoke(instance);
            });
        }
    }
}
