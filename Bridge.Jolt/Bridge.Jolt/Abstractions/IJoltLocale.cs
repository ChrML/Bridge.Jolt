namespace Jolt.Abstractions
{
    /// <summary>
    /// Represents a service that may provide default language strings used by Jolt.
    /// </summary>
    public interface IJoltLocale
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        string NothingToShow { get; }

        string UnableToConnect { get; }

        /// <summary>
        /// Gets the HTTP- status code description in this locale.
        /// </summary>
        /// <param name="code">The HTTP error code.</param>
        /// <returns>Returns the description in this locale, or <c>null</c> if not found.</returns>
        string GetHttpStatusCodeDescription(int code);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
