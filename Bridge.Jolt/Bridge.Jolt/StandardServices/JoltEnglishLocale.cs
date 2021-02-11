using Bridge;
using Jolt.Abstractions;

namespace Jolt.StandardServices.Default
{
    /// <summary>
    /// Implements the locale interface (<see cref="IJoltLocale"/>) with the default English- language strings.
    /// </summary>
    [Reflectable(true)]
    public class JoltEnglishLocale : IJoltLocale
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public string NothingToShow => "Nothing to show.";

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
