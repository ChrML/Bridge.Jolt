using System;
using System.Threading.Tasks;

namespace Bridge.Jolt
{
    /// <summary>
    /// Represents any element or control that requires asynchronous load before display. Such as a control loading something from the server.
    /// </summary>
    public interface IRequiresAsyncLoad
    {
        /// <summary>
        /// Called when the contents of this element or control should be loaded.
        /// </summary>
        /// <returns>Returns a <see cref="Task"/> that completes when the load has completed.</returns>
        /// <exception cref="Exception">Any exception can occur if load fails.</exception>
        Task LoadAsync();
    }
}
