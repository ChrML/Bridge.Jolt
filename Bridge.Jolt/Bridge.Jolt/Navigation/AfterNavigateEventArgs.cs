using System;

namespace Jolt.Navigation
{
    /// <summary>
    /// Provides data after a navigation occured.
    /// </summary>
    public sealed class AfterNavigateEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AfterNavigateEventArgs"/> class.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="control"></param>
        public AfterNavigateEventArgs(NavigateData data, IHtmlElement control)
        {
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            this.Control = control;
        }

        /// <summary>
        /// Gets the control that was navigated to.
        /// </summary>
        public IHtmlElement Control { get; }

        /// <summary>
        /// Gets the navigation data.
        /// </summary>
        public NavigateData Data { get; }
    }  
}
