namespace Bridge.Jolt.Abstractions
{
    /// <summary>
    /// Represents a service that can provide image URL's for the standard Jolt controls.
    /// </summary>
    public interface IJoltImageProvider
    {
        string Completed { get; }

        string Error { get; }

        string InProgress { get; }
    }
}
