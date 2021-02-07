namespace System.ComponentModel
{
    /// <summary>
    /// Represents the method that will handle the <see cref="IBindingList.ListChanged"/> event of the <see cref="IBindingList"/> interface.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="ListChangedEventArgs"/> that contains the event data.</param>
    public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs e);
}
