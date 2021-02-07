namespace System.ComponentModel
{
    /// <summary>
    /// Provides event arguments for events whose effects can be cancelled.
    /// </summary>
    public class CancelEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelEventArgs"/> class.
        /// </summary>
        public CancelEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelEventArgs"/> class with the <see cref="Cancel"/> property set to the given value.
        /// </summary>
        /// <param name="cancel"></param>
        public CancelEventArgs(bool cancel)
        {
            this.Cancel = cancel;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the event should be canceled.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
