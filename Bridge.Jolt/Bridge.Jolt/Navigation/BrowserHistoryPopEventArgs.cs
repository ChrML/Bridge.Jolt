namespace Jolt.Navigation
{
    /// <summary>
    /// Provides event arguments for the browser's pop- event.
    /// </summary>
    public class BrowserHistoryPopEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserHistoryPopEventArgs"/> class.
        /// </summary>
        /// <param name="state"></param>
        public BrowserHistoryPopEventArgs(object state)
        {
            this.State = state;
        }

        /// <summary>
        /// Gets the history state object.
        /// </summary>
        public object State { get; }
    }
}
