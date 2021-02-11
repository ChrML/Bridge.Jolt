using System;
using System.Threading.Tasks;

namespace Jolt.Navigation
{
    /// <summary>
    /// Represents a navigator that shows the navigated contents.
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Called when the new page contents have started loading when the navigator should indicate loading has begun.
        /// </summary>
        /// <param name="args">Navigation arguments.</param>
        /// <returns><see cref="Task"/> that completes when eventual animations added by the navigator has completed.</returns>
        Task BeginNavigateAsync(BeforeNavigateEventArgs args);

        /// <summary>
        /// Called when the new page contents have finished loading and the navigator should display its contents.
        /// </summary>
        /// <param name="args">Navigation arguments.</param>
        /// <returns><see cref="Task"/> that completes when eventual animations added by the navigator has completed.</returns>
        Task EndNavigateAsync(AfterNavigateEventArgs args);

        /// <summary>
        /// Sets error details if an error occurs so that a page cannot be displayed.
        /// </summary>
        /// <param name="data">Navigation data.</param>
        /// <param name="errorMessage">Information about the error that occured.</param>
        /// <param name="error">Optional exception information..</param>
        void SetError(NavigateData data, string errorMessage, Exception error = null);
    }
}
