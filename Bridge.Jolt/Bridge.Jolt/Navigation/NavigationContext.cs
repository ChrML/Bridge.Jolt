using System;
using System.Threading.Tasks;

namespace Jolt.Navigation
{
    /// <summary>
    /// Implements wrapper that for the navigation that allows us to navigate with different default options.
    /// </summary>
    public sealed class NavigationContext : INavigation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationContext"/> class.
        /// </summary>
        /// <param name="inner">Navigation to use for navigating.</param>
        /// <param name="defaultOptions">Default options to use for navigation if not specified by the navigation.</param>
        public NavigationContext(INavigation inner, NavigateOptions defaultOptions = null)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.options = defaultOptions;
        }

        /// <inheritdoc/>
        public IHtmlElement Current => this.inner.Current;

        /// <inheritdoc/>
        public event EventHandler<AfterNavigateEventArgs> AfterNavigate
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public event EventHandler<BeforeNavigateEventArgs> BeforeNavigate
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Go(int delta)
        {
            this.inner.Go(delta);
        }

        /// <inheritdoc/>
        public Task NavigateToAsync(string title, Func<NavigateData, IHtmlElement> createControl, NavigateOptions options = null)
        {
            return this.inner.NavigateToAsync(title, createControl, options ?? this.options);
        }


        readonly NavigateOptions options;
        readonly INavigation inner;
    }
}
