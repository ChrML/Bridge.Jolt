using Bridge.Jolt.Http;
using System.Threading.Tasks;

namespace Bridge.Jolt.Abstractions
{
    /// <summary>
    /// Represents a client that can call an API. This abstraction has no meaning of which dataformat will be used which is up to the implementating service.
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Executes an API- call to the given endpoint when no result data is expected.
        /// </summary>
        /// <param name="method">Which HTTP- method to use.</param>
        /// <param name="uri">URI- to the endpoint to call.</param>
        /// <param name="payload">Optional payload to submit.</param>
        /// <returns>Returns a task that completes when the request has completed.</returns>
        Task SendAsync(HttpMethod method, string uri, object payload = null);

        /// <summary>
        /// Executes an API- call to the given endpoint where result data is expected.
        /// </summary>
        /// <typeparam name="T">Datatype expected as result value.</typeparam>
        /// <param name="method">Which HTTP- method to use.</param>
        /// <param name="uri">URI- to the endpoint to call.</param>
        /// <param name="payload">Optional payload to submit.</param>
        /// <returns>Returns the result data.</returns>
        Task<T> SendAsync<T>(HttpMethod method, string uri, object payload = null);
    }
}
