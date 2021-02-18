using Jolt.Abstractions;
using System;

namespace Jolt.Http
{
    /// <summary>
    /// Provides HTTP helper methods.
    /// </summary>
    public static class HttpHelpers
    {
        /// <summary>
        /// Gets a HTTP status code description in the current locale.
        /// </summary>
        /// <param name="code">HTTP status code to parse.</param>
        /// <returns>Returns a string for that HTTP status code.</returns>
        public static string GetStatusCodeDescription(int code)
        {
            IJoltLocale locale = AppServices.Default.Resolve<IJoltLocale>();

            if (code != 0)
            {
                string description = locale.GetHttpStatusCodeDescription(code);
                if (description != null)
                {
                    return description + ". (HTTP " + code + ")";
                }
                else
                {
                    return "HTTP " + code;
                }
            }
            else
            {
                return locale.UnableToConnect;
            }
        }

        /// <summary>
        /// Gets the input URL with a timestamp appended to ensure no cache.
        /// </summary>
        /// <param name="url">Input URL.</param>
        /// <returns>Modified output URL.</returns>
        public static string GetUrlWithNoCache(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            long ticks = DateTime.Now.Ticks;
            if (url.Contains("?"))
            {
                return url + "&_=" + ticks;
            }
            else
            {
                return url + "?_=" + ticks;
            }
        }

        /// <summary>
        /// Gets if the given HTTP response code means that the request succeeded.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSuccessCode(int code)
        {
            return code >= 200 && code <= 300;
        }
    }
}
