using Jolt.Abstractions;
using Retyped;
using System;
using System.Threading.Tasks;

namespace Jolt.Navigation
{
    /// <summary>
    /// Provides extension methods for the <see cref="INavigation"/> service.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Navigates back one step.
        /// </summary>
        /// <param name="navigation">Navigation instance to use.</param>
        public static void Back(this INavigation navigation)
        {
            if (navigation == null) throw new ArgumentNullException(nameof(navigation));
            navigation.Go(-1);
        }

        /// <summary>
        /// Navigates forward one step.
        /// </summary>
        /// <param name="navigation">Navigation instance to use.</param>
        public static void Forward(this INavigation navigation)
        {
            if (navigation == null) throw new ArgumentNullException(nameof(navigation));
            navigation.Go(1);
        }

        /// <summary>
        /// Navigates to a new control.
        /// </summary>
        /// <param name="navigation">Navigation instance to use.</param>
        /// <param name="title">The title of the new page.</param>
        /// <param name="createControl">Delegate for creating the control.</param>
        /// <param name="options">Optional options for the navigation.</param>
        /// <returns><see cref="Task"/> that completes when the new page is fully loaded.</returns>
        public static async void NavigateTo(this INavigation navigation, string title, Func<NavigateData, IHtmlElement> createControl, NavigateOptions options = null)
        {
            if (navigation == null) throw new ArgumentNullException(nameof(navigation));
            try
            {
                await navigation.NavigateToAsync(title, createControl, options);
            }
            catch (Exception exception)
            {
                AppServices.Default.Resolve<IErrorHandler>().OnError(exception, "Error during navigation.");
            }
        }

        /// <summary>
        /// Navigates to a new control that is created using dependency injection and the default service provider.
        /// </summary>
        /// <param name="navigation">Navigation instance to use.</param>
        /// <param name="title">The title of the new page.</param>
        /// <param name="state">Optional state that will be passed to the constructor of the control for create.</param>
        /// <param name="options">Optional options for the navigation.</param>
        /// <returns><see cref="Task"/> that completes when the new page is fully loaded.</returns>
        public static void NavigateTo<T>(this INavigation navigation, string title, object state = null, NavigateOptions options = null)
            where T : class, IHtmlElement
        {
            if (navigation == null) throw new ArgumentNullException(nameof(navigation));

            navigation.NavigateTo(title, data =>
                {
                    // TODO: We should also be able to inject NavigateData here.
                    T control = ActivatorUtilities.CreateInstance<T>(AppServices.Default, state);
                    return control;
                }, 
                options
            );
        }

        /// <summary>
        /// Navigates to a new control that is created using dependency injection and the default service provider.
        /// </summary>
        /// <param name="services">Services instance to use.</param>
        /// <param name="title">The title of the new page.</param>
        /// <param name="state">Optional state that will be passed to the constructor of the control for create.</param>
        /// <param name="options">Optional options for the navigation.</param>
        /// <returns><see cref="Task"/> that completes when the new page is fully loaded.</returns>
        public static void NavigateTo<T>(this IServices services, string title, object state = null, NavigateOptions options = null)
            where T : class, IHtmlElement
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            INavigation navigation = services.Resolve<INavigation>();
            navigation.NavigateTo<T>(title, state, options);
        }

        /// <summary>
        /// Refreshes the current page by recreating the current control.
        /// </summary>
        /// <param name="navigation">Navigation instance to use.</param>
        public static void Refresh(this INavigation navigation)
        {
            if (navigation == null) throw new ArgumentNullException(nameof(navigation));
            navigation.Go(0);
        }

        /// <summary>
        /// Sets the given type as the navigator service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection SetNavigator<T>(this IServiceCollection services) where T : class, INavigator
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.RemoveServices<INavigator>();
            return services.AddSingleton<INavigator, T>();
        }

        /// <summary>
        /// Sets the given instance as the navigator service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IServiceCollection SetNavigator<T>(this IServiceCollection services, T instance) where T : class, INavigator
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.RemoveServices<INavigator>();
            return services.AddSingleton<INavigator, T>(instance);
        }

        /// <summary>
        /// Sets the default navigator as the navigator in the given DOM container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="domContainer">DOM- element to use for navigation contents.</param>
        public static IServiceCollection SetSimpleNavigatorIn(this IServiceCollection services, dom.HTMLElement domContainer)
        {
            if (domContainer == null) throw new ArgumentNullException(nameof(domContainer));

            services.RemoveServices<INavigator>();
            return services.AddSingleton<INavigator, SimpleNavigator>(new SimpleNavigator(domContainer));
        }
    }
}
