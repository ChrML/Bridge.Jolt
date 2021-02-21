namespace Jolt.Navigation
{
    /// <summary>
    /// Provides options used for navigation.
    /// </summary>
    public sealed class NavigateOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateOptions"/> class.
        /// </summary>
        public NavigateOptions()
        {
        }

        /// <summary>
        /// Gets the default navigate options.
        /// </summary>
        public static NavigateOptions Default { get; } = new NavigateOptions();
    }
}
