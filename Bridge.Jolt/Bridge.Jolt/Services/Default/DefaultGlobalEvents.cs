using Bridge.Jolt.Abstractions;
using Retyped;
using System;

namespace Bridge.Jolt.Services.Default
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IGlobalEvents"/> service that connects to the browser's events.
    /// </summary>
    [Reflectable(true)]
    class DefaultGlobalEvents: IGlobalEvents
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultGlobalEvents"/> class.
        /// </summary>
        public DefaultGlobalEvents()
        {
            dom.document.onmousedown += e => this.MouseDown?.Invoke(this, MouseEventArgs.FromNative(e));
            dom.document.onmousemove += e => this.MouseMove?.Invoke(this, MouseEventArgs.FromNative(e));
            dom.document.onmouseup += e => this.MouseUp?.Invoke(this, MouseEventArgs.FromNative(e));
            dom.window.onresize += e => this.WindowResize?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public event EventHandler<MouseEventArgs> MouseDown;

        /// <inheritdoc/>
        public event EventHandler<MouseEventArgs> MouseMove;

        /// <inheritdoc/>
        public event EventHandler<MouseEventArgs> MouseUp;

        /// <inheritdoc/>
        public event EventHandler WindowResize;
    }
}
