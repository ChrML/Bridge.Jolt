using System;

namespace Jolt.Navigation
{
    /// <summary>
    /// Represents the browser's history.
    /// </summary>
    public interface IBrowserHistory
    {
        /// <summary>
        /// Gets the length of the browser history.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Occurs when an item pops from the browser's history.
        /// </summary>
        event EventHandler<BrowserHistoryPopEventArgs> PopState;

        /// <summary>
        /// Submits a Go- command to the browser's history.
        /// </summary>
        /// <param name="delta"></param>
        void Go(int delta);

        /// <summary>
        /// Submits a new state to the browser history.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        void PushState(object state, string title, string url);

        /// <summary>
        /// Replaces the current state to the browser history.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        void ReplaceState(object state, string title, string url);
    }
}
