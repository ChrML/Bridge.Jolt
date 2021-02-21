using Bridge;
using Jolt.Controls;
using Retyped;
using System;
using System.Threading.Tasks;

namespace Jolt.Navigation
{
    /// <summary>
    /// Implements a simple navigator that will use the provided DOM- element to view the control navigated to.
    /// </summary>
    /// <remarks>
    /// You could use this class as a base class for your custom navigator integration.
    /// </remarks>
    [Reflectable(true)]
    public class SimpleNavigator: INavigator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleNavigator"/> class.
        /// </summary>
        /// <param name="domContainer"></param>
        public SimpleNavigator(dom.HTMLElement domContainer)
        {
            this.DomContainer = domContainer ?? throw new ArgumentNullException(nameof(domContainer));
            this.progress = new Spinner(domContainer);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the DOM- element displaying the navigated control.
        /// </summary>
        protected dom.HTMLElement DomContainer { get; }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public virtual Task BeginNavigateAsync(BeforeNavigateEventArgs args)
        {
            this.domCurrentControl?.Remove();
            this.domCurrentControl = null;

            this.progress.SetStatus(TaskStatus.InProgress);
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc/>
        public virtual Task EndNavigateAsync(AfterNavigateEventArgs args)
        {
            this.progress.SetStatus(TaskStatus.None);

            this.domCurrentControl = args.Control?.Dom;
            if (this.domCurrentControl != null)
            {
                this.DomContainer.appendChild(this.domCurrentControl);
            }

            return Task.FromResult<object>(null);
        }

        /// <inheritdoc/>
        public virtual void SetError(NavigateData data, string errorMessage, Exception error = null)
        {
            this.domCurrentControl?.Remove();
            this.domCurrentControl = null;

            string message = errorMessage;
            if (error != null)
            {
                message += ". " + error.Message;
            }

            this.progress.SetStatus(TaskStatus.Error, message);
        }

        #endregion

        #region Privates

        dom.HTMLElement domCurrentControl;
        readonly Spinner progress;

        #endregion
    }
}
