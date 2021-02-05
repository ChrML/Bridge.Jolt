using System;

namespace Bridge.Jolt.Controls
{
    /// <summary>
    /// Provides event arguments when items in a <see cref="ListView"/> has been clicked by the user.
    /// </summary>
    public sealed class ListViewItemEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItemEventArgs"/> class.
        /// </summary>
        /// <param name="item"></param>
        public ListViewItemEventArgs(ListViewItem item, MouseEventArgs mouseEvent)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
            this.MouseEvent = mouseEvent ?? throw new ArgumentNullException(nameof(mouseEvent));
        }

        /// <summary>
        /// Gets the item that the user clicked on.
        /// </summary>
        public ListViewItem Item { get; }

        /// <summary>
        /// Gets the mouse event arguments that raised this event.
        /// </summary>
        public MouseEventArgs MouseEvent { get; }
    }
}
