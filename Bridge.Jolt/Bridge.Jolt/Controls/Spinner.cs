using Bridge;
using Jolt.Abstractions;
using Retyped;
using System;

namespace Jolt.Controls
{
    /// <summary>
    /// Implements a control for indicating the progress of an ongoing action.
    /// </summary>
    [Reflectable(true)]
    public sealed class Spinner
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Spinner"/> class.
        /// </summary>
        /// <param name="parent">Container element for the spinner.</param>
        public Spinner(dom.HTMLElement parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current status indicated by this spinner.
        /// </summary>
        public TaskStatus Status { get; private set; } = TaskStatus.None;

        #endregion

        #region Methods

        /// <summary>
        /// Sets the current status to render in this spinner.
        /// </summary>
        /// <param name="status">Status to indicate.</param>
        /// <param name="details"></param>
        public void SetStatus(TaskStatus status, string details = null)
        {
            if (status == this.Status)
            {
                return;
            }

            this.Status = status;


            if (status == TaskStatus.None)
            {
                this.indicator?.Remove();
                this.indicator = null;
            }
            else
            {
                bool created =false;
                if (this.indicator == null)
                {
                    this.indicator = Html.NewImg<Spinner>("Indicator");
                    created = true;
                }

                switch (status)
                {
                    case TaskStatus.InProgress:
                        this.indicator.src = Service.Resolve<IJoltImageProvider>().InProgress;
                        break;

                    case TaskStatus.Completed:
                        this.indicator.src = Service.Resolve<IJoltImageProvider>().Completed;
                        break;

                    case TaskStatus.Error:
                        this.indicator.src = Service.Resolve<IJoltImageProvider>().Error;
                        break;

                    default:
                        this.indicator.Remove();
                        throw new NotSupportedException("Status " + status + " is not supported.");
                }

                this.indicator.title = details;

                if (created)
                {
                    this.parent.Append(this.indicator);
                }
            }
        }

        #endregion

        #region Privates

        dom.HTMLImageElement indicator;
        readonly dom.HTMLElement parent;

        #endregion
    }
}
