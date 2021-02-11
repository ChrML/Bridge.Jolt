using System;

namespace Jolt.Navigation
{
    /// <summary>
    /// Provides data about a navigation.
    /// </summary>
    public sealed class NavigateData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateData"/> class.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="title"></param>
        public NavigateData(IServices services, string title)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
            this.Title = title;
        }

        /// <summary>
        /// Gets the service container.
        /// </summary>
        public IServices Services { get; }

        /// <summary>
        /// Gets the title used when navigating.
        /// </summary>
        public string Title { get; }
    }
}
