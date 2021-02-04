using Bridge.Jolt.Abstractions;

namespace Bridge.Jolt.Services.Default
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IJoltImageProvider"/> service.
    /// </summary>
    class DefaultJoltImageProvider: IJoltImageProvider
    {
        public string Completed => "img/Completed.svg";

        public string Error => "img/Error.svg";

        public string InProgress => "img/InProgress.gif";
    }
}
