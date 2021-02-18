using Bridge;
using Jolt.Abstractions;
using Jolt.Http;
using Newtonsoft.Json;
using Retyped;
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
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
        /// </summary>
        public JsonApiClient()
        {
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public virtual Task SendAsync(HttpMethod method, string uri, object payload = null)
        {
            if (String.IsNullOrEmpty(uri)) throw new ArgumentNullException(nameof(uri));
            return SendAsync<object>(uri, payload, method, expectResponseData: false);
        }

        /// <inheritdoc/>
        public virtual Task<T> SendAsync<T>(HttpMethod method, string uri, object payload = null)
        {
            if (String.IsNullOrEmpty(uri)) throw new ArgumentNullException(nameof(uri));
            return SendAsync<T>(uri, payload, method, expectResponseData: true);
        }

        #endregion

        #region Privates

        /// <summary>
        /// Performs an web API request where the <paramref name="payload"/> is serialized to JSON and sent to the server.
        /// The server may or may not be expected to return data serializable to <typeparamref name="T"/> depending on <paramref name="expectResponseData"/>.
        /// </summary>
        static Task<T> SendAsync<T>(string uri, object payload, HttpMethod method, bool expectResponseData)
        {
            // Disable caching for GET- requests.
            if (method == HttpMethod.Get)
            {
                uri = HttpHelpers.GetUrlWithNoCache(uri);
            }

            // Configure request.
            string requestMethod = method.ToString().ToUpper();
            dom.XMLHttpRequest xhr = new dom.XMLHttpRequest();
            xhr.open(requestMethod, uri);
            xhr.responseType = dom.XMLHttpRequestResponseType.text;
            xhr.withCredentials = true;

            // Success responses.
            TaskCompletionSource<T> result = new TaskCompletionSource<T>();
            xhr.onload = (e) =>
            {
                if (HttpHelpers.IsSuccessCode(xhr.status))
                {
                    if (expectResponseData)
                    {
                        string json = xhr.responseText;

                        // Check if the client wants the string directly.
                        T objectData;
                        if (typeof(T) != typeof(string))
                        {
                            if (String.IsNullOrEmpty(json))
                            {
                                result.TrySetResult(default(T));
                            }
                            else
                            {
                                objectData = JsonConvert.DeserializeObject<T>(json);
                                result.TrySetResult(objectData);
                            }
                        }
                        else
                        {
                            objectData = (T)Convert.ChangeType(json, typeof(T));
                            result.TrySetResult(objectData);
                        }
                    }
                    else
                    {
                        result.TrySetResult(default(T));
                    }
                }
                else
                {
                    result.TrySetException(new HttpRequestException
                    (
                        message: HttpHelpers.GetStatusCodeDescription(xhr.status),
                        statusCode: xhr.status,
                        responsePayload: xhr.responseText
                    ));
                }
            };

            // Error responses.
            xhr.onerror = e =>
            {
                result.TrySetException(new HttpRequestException
                (
                    message: HttpHelpers.GetStatusCodeDescription(xhr.status),
                    statusCode: xhr.status,
                    responsePayload: xhr.responseText
                ));
            };

            // Abort responses.
            xhr.onabort = e =>
            {
                result.TrySetCanceled();
            };

            // Apply the correct content type.
            string contentType = GetContentType();
            if (contentType != null)
            {
                xhr.setRequestHeader("Content-Type", contentType);
            }

            string sendData = CreateJsonPayload(payload);
            if (sendData != null)
            {
                xhr.send(sendData);
            }
            else
            {
                xhr.send();
            }

            // Returns task completing when server responds with success/error/abort.
            return result.Task;
        }

        #endregion

        #region Privates

        /// <summary>
        /// Converts the input data and converts it into data we can submit as payload to the server.
        /// </summary>
        static string CreateJsonPayload(object input)
        {
            switch (input)
            {
                case string str:
                    return str;

                case null:
                    return null;

                default:
                    return JsonConvert.SerializeObject(input);
            }
        }

        /// <summary>
        /// Gets the content type to use for the submit.
        /// </summary>
        static string GetContentType()
        {
            return "application/json";
        }

        #endregion
    }
}
