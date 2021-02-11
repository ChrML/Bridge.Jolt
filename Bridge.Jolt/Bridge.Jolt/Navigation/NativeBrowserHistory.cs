using Bridge;
using Retyped;
using System;

namespace Jolt.Navigation
{
    /// <summary>
    /// Maps the browser interface to the actual native implementation.
    /// </summary>
    [Reflectable(true)]
    sealed class NativeBrowserHistory : IBrowserHistory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeBrowserHistory"/> class.
        /// </summary>
        public NativeBrowserHistory()
        {
            dom.window.onpopstate += ev => this.OnPopState(new BrowserHistoryPopEventArgs(ev.state));
        }

        /// <inheritdoc/>
        public int Length => Convert.ToInt32(dom.history.length);

        /// <inheritdoc/>
        public event EventHandler<BrowserHistoryPopEventArgs> PopState;

        /// <summary>
        /// Raises the <see cref="PopState"/> event.
        /// </summary>
        /// <param name="e"></param>
        void OnPopState(BrowserHistoryPopEventArgs e) => this.PopState?.Invoke(this, e);

        /// <inheritdoc/>
        public void Go(int delta)
        {
            dom.history.go(delta);
        }

        /// <inheritdoc/>
        public void PushState(object state, string title, string url)
        {
            dom.history.pushState(state, title, url);
        }

        /// <inheritdoc/>
        public void ReplaceState(object state, string title, string url)
        {
            dom.history.replaceState(state, title, url);
        }
    }
}
