namespace System.ComponentModel
{
    /// <summary>
    /// Indicates whether a class converts property change events to ListChanged events.
    /// </summary>
    public interface IRaiseItemChangedEvents
    {
        /// <summary>
        /// Gets a value indicating whether the IRaiseItemChangedEvents object raises ListChanged events.
        /// </summary>
        bool RaisesItemChangedEvents { get; }
    }
}
