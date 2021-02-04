using System.Threading.Tasks;

namespace Bridge.Jolt
{
    /// <summary>
    /// Delegate that represents an asynchronous event handler.
    /// </summary>
    /// <typeparam name="T">Type of event arguments.</typeparam>
    /// <param name="sender">Class that sent the event.</param>
    /// <param name="e">Event arguments related to this event.</param>
    /// <returns></returns>
    public delegate Task EventHandlerAsync<T>(object sender, T e);
}
