namespace System.ComponentModel
{
    /// <summary>
    /// Represents the method that handles a cancelable event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="CancelEventArgs">CancelEventArgs</see> that contains the event data.</param>
    public delegate void CancelEventHandler(object sender, CancelEventArgs e);
}
