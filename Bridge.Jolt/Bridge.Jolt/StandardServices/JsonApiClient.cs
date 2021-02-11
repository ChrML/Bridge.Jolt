using Bridge;
using Jolt.Abstractions;
using Jolt.Http;
using System;
using System.Threading.Tasks;

namespace Jolt.StandardServices.Default
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
