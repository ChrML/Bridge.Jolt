using System;
using System.Threading.Tasks;

namespace Jolt.Navigation
{
    /// <summary>
    /// Represents the framework's primary navigation system.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Occurs before the user has navigated to a new control.
        /// </summary>
        event EventHandler<BeforeNavigateEventArgs> BeforeNavigate;

        /// <summary>
        /// Occurs after the user has navigated to a new control.
        /// </summary>
        event EventHandler<AfterNavigateEventArgs> AfterNavigate;

        /// <summary>
        /// Gets the control or page that we have currently navigated to.
        /// </summary>
        IHtmlElement Current { get; }

        /// <summary>
        /// Navigates back or forward in the history. Positive value means forward, and a negative value means backwards.
        /// </summary>
        /// <param name="delta"></param>
        void Go(int delta);

        /// <summary>
        /// Navigates to a new control and waits for the target control to have been fully loaded before completing.
        /// </summary>
        /// <param name="title">The title of the new page.</param>
        /// <param name="createControl">Delegate for creating the control.</param>
        /// <param name="options">Optional options for the navigation.</param>
        /// <returns><see cref="Task"/> that completes when the new page is fully loaded.</returns>
        Task NavigateToAsync(string title, Func<NavigateData, IHtmlElement> createControl, NavigateOptions options = null);
    }
}
