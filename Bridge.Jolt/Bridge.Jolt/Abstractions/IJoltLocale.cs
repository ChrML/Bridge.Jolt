namespace Bridge.Jolt.Abstractions
{
    /// <summary>
    /// Represents a service that may provide default language strings used by Jolt.
    /// </summary>
    public interface IJoltLocale
    {
        string NothingToShow { get; }
    }
}
