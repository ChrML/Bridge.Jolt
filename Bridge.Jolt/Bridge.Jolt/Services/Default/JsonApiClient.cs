using Bridge.Jolt.Abstractions;
using Bridge.Jolt.Http;
using System;
using System.Threading.Tasks;

namespace Bridge.Jolt.Services.Default
{
    /// <summary>
    /// Provides an implementation of the <see cref="IApiClient"/> that uses the JSON- format.
    /// </summary>
    [Reflectable(true)]
    public class JsonApiClient: IApiClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
        /// </summary>
        public JsonApiClient()
        {
        }

        /// <inheritdoc/>
        public virtual Task SendAsync(HttpMethod method, string uri, object payload = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual Task<T> SendAsync<T>(HttpMethod method, string uri, object payload = null)
        {
            throw new NotImplementedException();
        }
    }
}
