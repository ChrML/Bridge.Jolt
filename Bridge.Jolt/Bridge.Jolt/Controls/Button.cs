using Bridge;
using Jolt.Abstractions;
using Retyped;
using System;
using System.Threading.Tasks;

namespace Jolt.Controls
{
    /// <summary>
    /// Implements a button control that may perform both synchronous or asynchronos operations.
    /// </summary>
    [Reflectable(true)]
    public class Button: HtmlControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
            this.spinner = new Spinner(this.DomElement);
            this.Text = "";
            this.DomElement.onclick = (e) => this.RunClick(MouseEventArgs.FromNative(e));
            this.DomElement.appendChild(this.domText);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class with the given text.
        /// </summary>
        public Button(string text)
            : this()
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the button is disabled. Disabled buttons cannot be clicked and are rendered differently.
        /// </summary>
        public bool Disabled
        {
            get => this._disabled;
            set
            {
                if (this._disabled != value)
                {
                    this._disabled = value;
                    this.DomElement.SetClass("Disabled", value);
                }
            }
        }
        bool _disabled;

        /// <summary>
        /// Gets or sets the image to render inside this button.
        /// </summary>
        public string Image
        {
            get => this._image;
            set => this.SetImage(value);
        }
        string _image;

        /// <summary>
        /// Gets if an asynchronous action is currently in progress.
        /// </summary>
        public bool InProgress { get; private set; }

        /// <summary>
        /// Gets or sets the text to render inside this button.
        /// </summary>
        public string Text
        {
            get => this._text;
            set
            {
                if (value != this._text)
                {
                    this._text = value;
                    this.domText.SetNullableInnerText(value);
                }
            }
        }
        string _text;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when user has clicked this button.
        /// </summary>
        public event EventHandler<MouseEventArgs> Click;

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick(MouseEventArgs e) => Click?.Invoke(this, e);

        /// <summary>
        /// Occurs when user has clicked the button. The handler of this event should return a task that will be awaited by this control.
        /// </summary>
        public EventHandlerAsync<MouseEventArgs> ClickAsync { get; set; }

        /// <summary>
        /// Raises the <see cref="ClickAsync"/> handler.
        /// </summary>
        protected async virtual Task OnClickAsync(MouseEventArgs e)
        {
            if (this.ClickAsync != null)
            {
                await this.ClickAsync(this, e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called if the button's action failed due to an exception.
        /// </summary>
        /// <param name="exception"></param>
        protected virtual void OnActionException(Exception exception)
        {
            this.SetInProgress(false);
            Service.Resolve<IErrorHandler>().OnError(exception);
        }

        /// <summary>
        /// Called after the button's action has successfully completed.
        /// </summary>
        protected virtual void OnAfterAction()
        {
            this.SetInProgress(false);
        }

        /// <summary>
        /// Called before the button's action is invoked.
        /// </summary>
        protected virtual void OnBeforeAction()
        {
            this.SetInProgress(true);
        }

        /// <summary>
        /// Invokes the handlers of this button's click- events as if the user clicked the button.
        /// </summary>
        /// <param name="e">Mouse event arguments to provide with the click.</param>
        public void RunClick(MouseEventArgs e)
        {
            _ = this.RunClickAsync(e);
        }

        /// <summary>
        /// Invokes the handlers of this button's click events as if the user clicked the button. <br/>
        /// This method can be awaited until the task has completed.
        /// </summary>
        /// <returns></returns>
        /// <param name="e"></param>
        public async Task RunClickAsync(MouseEventArgs e)
        {
            if (this.Disabled || this.InProgress)
            {
                return;
            }

            try
            {
                // Show the spinner.
                this.InProgress = true;
                this.OnBeforeAction();

                // Invoke the handlers.
                this.OnClick(e);
                await this.OnClickAsync(e);

                // Hide the spinner.
                this.InProgress = false;
                this.OnAfterAction();
            }
            catch (Exception exception)
            {
                this.InProgress = false;
                this.OnActionException(exception);
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Toggles the button's in-progress indicator.
        /// </summary>
        /// <param name="inProgress"></param>
        protected virtual void SetInProgress(bool inProgress)
        {
            if (this.spinning)
            {
                if (!inProgress)
                {
                    this.spinner.SetStatus(TaskStatus.None);

                    if (this.domImage != null)
                    {
                        this.DomElement.Append(this.domImage);
                    }
                    this.DomElement.Append(this.domText);

                    this.DomElement.SetClass("InProgress", false);
                    this.UnlockSize();
                    this.spinning = false;
                }
            }
            else
            {
                if (inProgress)
                {
                    this.LockSize();
                    this.DomElement.SetClass("InProgress", true);
                    this.domImage?.Remove();
                    this.domText.Remove();
                    this.spinner.SetStatus(TaskStatus.InProgress);
                    this.spinning = true;
                }
            }
        }

        /// <summary>
        /// Locks the current actual size of the button.
        /// </summary>
        void LockSize()
        {
            if (!this.sizeFixed)
            {
                int actualWidth = this.DomElement.offsetWidth;
                int actualHeight = this.DomElement.offsetHeight;
                this.DomElement.style.width = actualWidth.ToString() + "px";
                this.DomElement.style.height = actualHeight.ToString() + "px";
                this.sizeFixed = true;
            }
        }

        /// <summary>
        /// Sets the image displayed by this button.
        /// </summary>
        /// <param name="src"></param>
        void SetImage(string src)
        {
            this._image = src;

            if (String.IsNullOrEmpty(src))
            {
                this.domImage?.Remove();
            }
            else
            {
                if (this.domImage == null)
                {
                    this.domImage = Html.NewImg<Button>("Image");
                    this.domImage.src = src;

                    if (!this.InProgress)
                    {
                        this.DomElement.Insert(0, this.domImage);
                    }
                }
            }
        }

        /// <summary>
        /// Unlocks the current size of the button so that it may flow freely according to the contents.
        /// </summary>
        void UnlockSize()
        {
            if (this.sizeFixed)
            {
                this.DomElement.style.width = null;
                this.DomElement.style.height = null;
                this.sizeFixed = false;
            }
        }


        bool sizeFixed = false;
        bool spinning = false;

        dom.HTMLImageElement domImage = null;
        readonly dom.HTMLDivElement domText = Html.NewDiv<Button>("Text");
        readonly Spinner spinner;

        #endregion
    }
}
