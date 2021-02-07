namespace Jolt.Http
{
    /// <summary>
    /// Provides information about a HTTP- method used to call a web-API.
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// HTTP GET- request.
        /// </summary>
        Get,

        /// <summary>
        /// HTTP POST- request.
        /// </summary>
        Post,

        /// <summary>
        /// HTTP PUT- request.
        /// </summary>
        Put,

        /// <summary>
        /// HTTP PATCH- request.
        /// </summary>
        Patch,

        /// <summary>
        /// HTTP DELETE- request.
        /// </summary>
        Delete,

        /// <summary>
        /// HTTP OPTIONS- request.
        /// </summary>
        Options,

        /// <summary>
        /// HTTP HEAD- request.
        /// </summary>
        Head
    }
}
