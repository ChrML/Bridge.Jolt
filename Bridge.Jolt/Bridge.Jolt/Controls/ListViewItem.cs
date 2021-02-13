using Retyped;
using System;

namespace Jolt.Controls
{
    /// <summary>
    /// Implements a single item in a listview.
    /// </summary>
    public class ListViewItem : HtmlControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/> class.
        /// </summary>
        public ListViewItem()
        {
            this.Dom.onclick = this.DomClicked;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListViewItem"/> class with an initial text.
        /// </summary>
        /// <param name="text"></param>
        public ListViewItem(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the item is disabled. Disabled items cannot be clicked and may be rendered differently.
        /// </summary>
        public bool Disabled
        {
            get => this._disabled;
            set
            {
                if (this._disabled != value)
                {
                    this._disabled = value;
                    this.Dom.SetClass("Disabled", value);
                }
            }
        }
        bool _disabled;

        /// <summary>
        /// Gets or sets the text rendered in this item.
        /// </summary>
        public string Text
        {
            get => this._text;
            set => this.SetText(value);
        }
        string _text;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user clicks on this item.
        /// </summary>
        public event EventHandler<MouseEventArgs> Click;

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(MouseEventArgs e) => this.Click?.Invoke(this, e);

        #endregion

        #region Methods

        /// <summary>
        /// Sets custom element contents to render in this item.
        /// </summary>
        /// <param name="element"></param>
        public void SetElementContent(IHtmlElement element)
        {
            if (this.currentElement != null)
            {
                this.currentElement.Remove();
                this.currentElement = null;
            }

            this.currentElement = element?.DomElement;

            if (element != null)
            {
                this.Dom.Append(element);
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Called when the user clicks on this item.
        /// </summary>
        /// <param name="ev"></param>
        void DomClicked(dom.MouseEvent ev)
        {
            if (!this.Disabled)
            {
                this.OnClick(MouseEventArgs.FromNative(ev));
            }
        }

        /// <summary>
        /// Sets a text to render in this item.
        /// </summary>
        /// <param name="text"></param>
        void SetText(string text)
        {
            this._text = text;

            if (!this.currentElementIsText)
            {
                this.currentElement?.Remove();
                this.currentElement = Html.NewDiv<ListViewItem>("TextContent");
                this.Dom.Append(this.currentElement);
            }

            this.currentElement.SetNullableInnerText(text);
            this.currentElementIsText = true;
        }


        dom.HTMLElement currentElement = null;
        bool currentElementIsText = false;

        #endregion
    }
}
