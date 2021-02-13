using Jolt.Utilities;
using Retyped;
using System;

namespace Jolt.Controls
{
    /// <summary>
    /// Simple base class for HTML controls.
    /// </summary>
    public abstract class HtmlControl: IHtmlElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlControl"/> class.
        /// </summary>
        protected HtmlControl()
        {
            this.Dom = Html.NewDiv(this.GetType());

            if (this is ILifecycle lifecycle)
            {
                this.Dom["$Jolt$Lifecycle"] = lifecycle;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlControl"/> class with an root element.
        /// </summary>
        /// <param name="domElement">Existing element to use as root for this control.</param>
        protected HtmlControl(dom.HTMLElement domElement)
        {
            this.Dom = domElement ?? throw new ArgumentNullException(nameof(domElement));
            domElement.AddClass(Css.GetClass(this.GetType()));

            if (this is ILifecycle lifecycle)
            {
                domElement["$Jolt$Lifecycle"] = lifecycle;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an additional CSS class for this control.
        /// </summary>
        public string CssClass
        {
            get => this.Dom.GetSwitchCssClass();
            set => this.Dom.SwitchCssClass(value);
        }

        /// <summary>
        /// Gets the DOM- element that is the root of this control.
        /// </summary>
        protected dom.HTMLElement Dom { get; }

        /// <summary>
        /// Gets if this control currently is focused.
        /// </summary>
        public virtual bool IsFocused => dom.document.activeElement == this.Dom;
        
        /// <summary>
        /// Gets or sets the text of a tooltip to show when the user holds mouse over this control.
        /// </summary>
        public string Tooltip
        {
            get => this._tooltip;
            set
            {
                if (value != this._tooltip)
                {
                    this._tooltip = value;
                    this.Dom.title = value;
                }
            }
        }
        string _tooltip;

        /// <summary>
        /// Gets or sets if this control should be visible.
        /// </summary>
        public bool Visible
        {
            get => this._visible;
            set
            {
                if (value != this._visible)
                {
                    this.Dom.SetDisplay(value);
                    this._visible = value;
                }
            }
        }
        bool _visible = true;

        #endregion

        #region Methods

        /// <summary>
        /// Sets the browser's focus to this control.
        /// </summary>
        public virtual void Focus()
        {
            this.Dom.focus();
        }

        #endregion

        #region IHtmlElement- implementation

        /// <inheritdoc/>
        dom.HTMLElement IHtmlElement.DomElement => this.Dom;

        #endregion

        #region Privates

#pragma warning disable IDE0052 // Remove unread private members

        /// <summary>
        /// Track lifecycles here as a part of the HTML- control class because every control that needs to track lifecycle should use this as baseclass.
        /// </summary>
        static readonly LifecycleTracker lifecycles = new LifecycleTracker();

#pragma warning restore IDE0052 // Remove unread private members

        #endregion
    }
}
