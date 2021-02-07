namespace System.ComponentModel
{
    /// <summary>
    /// Provides data for the AddingNew event.
    /// </summary>
    public class AddingNewEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddingNewEventArgs"/> class using no parameters.
        /// </summary>
        public AddingNewEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddingNewEventArgs"/> class using the specified object as the new item.
        /// </summary>
        /// <param name="newObject"></param>
        public AddingNewEventArgs(object newObject)
        {
            this.NewObject = newObject;
        }

        /// <summary>
        /// Gets or sets the object to be added to the binding list.
        /// </summary>
        public object NewObject { get; set; }
    }
}
