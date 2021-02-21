namespace Jolt
{
    /// <summary>
    /// Indicates that a control has a state that could be restored at a later point.
    /// </summary>
    public interface IControlState
    {
        /// <summary>
        /// Gets the current state of this control as an anonymous object. 
        /// </summary>
        /// <returns>The current state of this control.</returns>
        /// <remarks>
        /// The data stored in the state must be serializable.
        /// </remarks>
        object GetState();
    }
}
