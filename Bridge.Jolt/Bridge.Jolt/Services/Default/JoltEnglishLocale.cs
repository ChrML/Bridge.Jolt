using Bridge.Jolt.Abstractions;

namespace Bridge.Jolt.Services.Default
{
    /// <summary>
    /// Implements the locale interface (<see cref="IJoltLocale"/>) with the default English- language strings.
    /// </summary>
    [Reflectable(true)]
    public class JoltEnglishLocale : IJoltLocale
    {
        public string NothingToShow => "Nothing to show.";
    }
}
