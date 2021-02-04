using Retyped;
using System;
using System.ComponentModel;

namespace Bridge.Jolt
{
    /// <summary>
    /// Provides event arguments for mouse related events.
    /// </summary>
    public class MouseEventArgs: CancelEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="mouseEvent"></param>
        MouseEventArgs(dom.MouseEvent mouseEvent)
        {
            this.NativeEvent = mouseEvent ?? throw new ArgumentNullException(nameof(mouseEvent));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class based on a native browser event.
        /// </summary>
        /// <param name="mouseEvent"></param>
        /// <returns></returns>
        public static MouseEventArgs FromNative(dom.MouseEvent mouseEvent)
        {
            return new MouseEventArgs(mouseEvent);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the mouse button that triggered the event.
        /// </summary>
        public MouseButton Button
        {
            get
            {
                switch (this.NativeEvent.button)
                {
                    case 0: return MouseButton.Left;
                    default: return MouseButton.Other;
                }
            }
        }

        /// <summary>
        /// Gets if the CTRL- key was held while the event occured.
        /// </summary>
        public bool CtrlKey => this.NativeEvent.ctrlKey;

        /// <summary>
        /// Gets a default instance of mouse event arguments.
        /// </summary>
        public new static MouseEventArgs Empty { get; } = new MouseEventArgs(new dom.MouseEvent("click"));

        /// <summary>
        /// Gets the inner native event.
        /// </summary>
        public dom.MouseEvent NativeEvent { get; }

        /// <summary>
        /// Gets the target element that this event was raised for.
        /// </summary>
        public dom.HTMLElement Target => this.NativeEvent.target as dom.HTMLElement;

        #endregion

        #region Methods

        /// <summary>
        /// Cancels this event as well as telling the browser to prevent default behaviour and event propagation.
        /// </summary>
        public void CancelAll()
        {
            this.StopPropagation();
            this.PreventDefault();
            this.Cancel = true;
        }

        /// <summary>
        /// Tells the browser to prevent the default behaviour this event would cause.
        /// </summary>
        public void PreventDefault() => this.NativeEvent.preventDefault();

        /// <summary>
        /// Tells the browser to stop propagating this event.
        /// </summary>
        public void StopPropagation() => this.NativeEvent.stopPropagation();

        #endregion
    }
}
