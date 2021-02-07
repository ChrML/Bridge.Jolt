using System;
using System.Threading.Tasks;
using Jolt.Http;

namespace Jolt.Abstractions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IApiClient"/> service for common API- methods.
    /// </summary>
    public static class ApiClientExtensions
    {
        /// <summary>
        /// Sends a HTTP DELETE- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>.
        /// </summary>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished.</returns>
        public static Task DeleteAsync(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync(HttpMethod.Delete, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP DELETE- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>. <br/>
        /// Expects the server to return data compatible with the provided type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected result data type.</typeparam>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished with response data.</returns>
        public static Task<T> DeleteAsync<T>(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync<T>(HttpMethod.Delete, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP GET- request to the given <paramref name="uri"/>.
        /// Expects the server to return data compatible with the provided type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected result data type.</typeparam>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished with response data.</returns>
        public static Task<T> GetAsync<T>(this IApiClient client, string uri)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync<T>(HttpMethod.Get, uri);
        }

        /// <summary>
        /// Sends a HTTP PATCH- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>.
        /// </summary>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished.</returns>
        public static Task PatchAsync(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync(HttpMethod.Patch, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP PATCH- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>. <br/>
        /// Expects the server to return data compatible with the provided type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected result data type.</typeparam>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished with response data.</returns>
        public static Task<T> PatchAsync<T>(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync<T>(HttpMethod.Patch, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP POST- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>.
        /// </summary>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished.</returns>
        public static Task PostAsync(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync(HttpMethod.Post, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP POST- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>. <br/>
        /// Expects the server to return data compatible with the provided type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected result data type.</typeparam>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished with response data.</returns>
        public static Task<T> PostAsync<T>(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync<T>(HttpMethod.Post, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP PUT- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>.
        /// </summary>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished.</returns>
        public static Task PutAsync(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync(HttpMethod.Put, uri, payload);
        }

        /// <summary>
        /// Sends a HTTP PUT- request to the given <paramref name="uri"/> with an optional <paramref name="payload"/>. <br/>
        /// Expects the server to return data compatible with the provided type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected result data type.</typeparam>
        /// <param name="client">The API- client to use for sending the request.</param>
        /// <param name="uri">The relative or absolute URI to send the request to.</param>
        /// <param name="payload">Optional payload to send with the request.</param>
        /// <returns>Returns a <see cref="Task"/> that completes when the request has finished with response data.</returns>
        public static Task<T> PutAsync<T>(this IApiClient client, string uri, object payload = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            return client.SendAsync<T>(HttpMethod.Put, uri, payload);
        }
    }
}
