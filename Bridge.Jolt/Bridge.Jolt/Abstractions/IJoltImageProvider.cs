namespace Bridge.Jolt.Abstractions
{
    /// <summary>
    /// Represents a service that can provide image URL's for the standard Jolt controls.
    /// </summary>
    public interface IJoltImageProvider
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        string Completed { get; }

        string Error { get; }

        string InProgress { get; }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
