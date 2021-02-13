using System;

namespace Jolt.Abstractions
{
    /// <summary>
    /// Provides access to the most common global browser events, such as mouse or keyboard events.
    /// </summary>
    public interface IGlobalEvents
    {
        // TODO: Add the keyboard events.
        //event EventHandler<KeyboardEventArgs> KeyDown;
        //event EventHandler<KeyboardEventArgs> KeyPress;
        //event EventHandler<KeyboardEventArgs> KeyUp;

        /// <summary>
        /// Occurs when the user presses a mouse button down.
        /// </summary>
        event EventHandler<MouseEventArgs> MouseDown;

        /// <summary>
        /// Occurs when the user moves the mouse within the application area.
        /// </summary>
        event EventHandler<MouseEventArgs> MouseMove;

        /// <summary>
        /// Occurs when the user releases a mouse button up.
        /// </summary>
        event EventHandler<MouseEventArgs> MouseUp;

        /// <summary>
        /// Occurs when the window area this application is rendered in changes its size.
        /// </summary>
        event EventHandler WindowResize;
    }
}
