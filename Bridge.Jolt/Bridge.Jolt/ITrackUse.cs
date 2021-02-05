namespace Bridge.Jolt
{
    /// <summary>
    /// Interface that should be implemented by elements or controls that need a notification when added or removed to the DOM- tree.
    /// </summary>
    /// <remarks>
    /// Examples of usage: <br/>
    /// * Control that needs to subscribe and unsubscribe to global events.<br/>
    /// * Control that opens a context menu that needs to be cleaned up when the control is removed.<br/>
    /// * Control that render resources needing cleanup such as subscriptions, websockets or charts.
    /// </remarks>
    public interface ITrackUse
    {
        /// <summary>
        /// Called by the framework when this element implementing this interface has been added to the DOM- tree.
        /// </summary>
        void AddedToDom();

        /// <summary>
        /// Called by the framework when this element implementing this interface has been removed from the DOM- tree.
        /// </summary>
        void RemovedFromDom();
    }
}
