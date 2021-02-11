using Bridge;
using Retyped;
using System;

namespace Jolt.Controls
{
    /// <summary>
    /// Implements an editable textbox control.
    /// </summary>
    [Reflectable(true)]
    public class TextBox : HtmlControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        public TextBox()
        {
            this.EnsureInputKind();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class with an initial text configured.
        /// </summary>
        /// <param name="text">The initial text.</param>
        public TextBox(string text)
            : this()
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if this control is disabled for new input.
        /// </summary>
        public bool Disabled
        {
            get => this._disabled;
            set
            {
                if (value != this._disabled)
                {
                    this._disabled = value;
                    this.SetStyle();
                }
            }
        }
        bool _disabled = false;

        /// <summary>
        /// Gets if a validation error has been set for this control.
        /// </summary>
        public bool HasValidationError { get; private set; }

        /// <summary>
        /// Gets or sets what kind of textbox this is.
        /// </summary>
        public TextBoxKind Kind
        {
            get => this._kind;
            set => this.SetKind(value);
        }
        TextBoxKind _kind = TextBoxKind.Standard;

        /// <summary>
        /// Gets or sets the placeholder text to use if the textbox contents are empty.
        /// </summary>
        public string Placeholder
        {
            get => this._placeholder;
            set
            {
                this._placeholder = value ?? String.Empty;
                this.SetStyle();
            }
        }
        string _placeholder = "";

        /// <summary>
        /// Gets or sets if the contents of this textbox should be read-only.
        /// </summary>
        public bool ReadOnly
        {
            get => this._readOnly;
            set
            {
                if (value != this._readOnly)
                {
                    this._readOnly = value;
                    this.SetStyle();
                }
            }
        }
        bool _readOnly = false;

        /// <summary>
        /// Gets or sets the text value in this textbox.
        /// </summary>
        public string Text
        {
            get => this.GetText();
            set => this.SetText(value);
        }

        /// <summary>
        /// Gets the current validation error message if <see cref="HasValidationError"/> is <see langword="true"/>.
        /// </summary>
        public string ValidationErrorMessage { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user puts focus on this control.
        /// </summary>
        public event EventHandler GotFocus;

        /// <summary>
        /// Raises the <see cref="GotFocus"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGotFocus(EventArgs e) => this.GotFocus?.Invoke(this, e);

        /// <summary>
        /// Occurs when the user takes the focus away from this control.
        /// </summary>
        public event EventHandler LostFocus;

        /// <summary>
        /// Raises the <see cref="LostFocus"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnLostFocus(EventArgs e) => LostFocus?.Invoke(this, e);

        /// <summary>
        /// Occurs when the <see cref="Text"/> property of this control changes when the user changes focus away from it (e.g. done typing).
        /// </summary>
        public event EventHandler TextChange;

        /// <summary>
        /// Raises the <see cref="TextChange"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChange(EventArgs e) => this.TextChange?.Invoke(this, e);

        /// <summary>
        /// Occurs when the <see cref="Text"/> property of this control changes as the user is typing.
        /// </summary>
        public event EventHandler TextInput;

        /// <summary>
        /// Raises the <see cref="TextInput"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextInput(EventArgs e) => TextInput?.Invoke(this, e);

        #endregion

        #region Methods

        /// <summary>
        /// Clears a previously set validate error.
        /// </summary>
        public void ClearValidateError()
        {
            if (this.HasValidationError)
            {
                this.HasValidationError = false;
                this.ValidationErrorMessage = null;

                this.DomElement.RemoveClass("ValidateError");
                this.validation?.Remove();
                this.validation = null;
            }
        }

        /// <summary>
        /// Sets a validate error to render for this control.
        /// </summary>
        /// <param name="message">Optional validation error message to show.</param>
        public void SetValidateError(string message = null)
        {
            // Change the style.
            if (!this.HasValidationError)
            {
                this.HasValidationError = true;
                this.DomElement.AddClass("ValidateError");
            }

            // Show the validation message.
            if (String.IsNullOrEmpty(message))
            {
                this.validation?.Remove();
                this.validation = null;
            }
            else
            {
                if (this.validation == null)
                {
                    this.validation = new Label
                    {
                        CssClass = Css.GetClass<TextBox>("ValidateErrorLabel")
                    };
                    this.DomElement.Append(this.validation);
                }
                this.validation.Text = message;
                this.ValidationErrorMessage = message;
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Adds the events required for this textbox to work properly independent of HTML- kind.
        /// </summary>
        void AddEvents()
        {
            this.any.onblur = e => this.OnLostFocus(EventArgs.Empty);
            this.any.onchange = e => this.OnTextChange(EventArgs.Empty);
            this.any.onfocus = e => this.OnGotFocus(EventArgs.Empty);
            this.any.oninput = e => this.OnTextInput(EventArgs.Empty);
        }

        /// <summary>
        /// Clears the current HTML- textbox from the DOM.
        /// </summary>
        void ClearHtml()
        {
            if (this.any != null)
            {
                this.any.Remove();
                this.any.onchange = null;
                this.any.oninput = null;
                this.any.onfocus = null;
                this.any.onblur = null;
                this.any = null;
            }
            this.input = null;
            this.area = null;
        }

        /// <summary>
        /// Ensures that we have an HTML Input- kind of DOM- element.
        /// </summary>
        dom.HTMLInputElement EnsureInputKind()
        {
            if (this.input != null)
            {
                return this.input;
            }
            else
            {
                string oldText = this.GetText();
                this.ClearHtml();

                this.input = new dom.HTMLInputElement();
                this.any = this.input;

                this.AddEvents();
                this.SetStyle();
                this.SetText(oldText);

                this.DomElement.Append(this.any);
                return this.input;
            }
        }

        /// <summary>
        /// Ensures that we have an HTML TextArea- kind of DOM- element.
        /// </summary>
        dom.HTMLTextAreaElement EnsureTextAreaKind()
        {
            if (this.area != null)
            {
                return this.area;
            }
            else
            {
                string oldText = this.GetText();
                this.ClearHtml();

                this.area = new dom.HTMLTextAreaElement();
                this.any = this.area;

                this.AddEvents();
                this.SetStyle();
                this.SetText(oldText);

                this.DomElement.Append(this.any);
                return this.area;
            }
        }

        /// <summary>
        /// Gets the current user-input text of this control.
        /// </summary>
        /// <returns></returns>
        string GetText()
        {
            if (this.input != null)
            {
                return this.input.value;
            }
            else if (this.area != null)
            {
                return this.area.value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the kind of textbox we have.
        /// </summary>
        /// <param name="value"></param>
        void SetKind(TextBoxKind value)
        {
            if (value != this._kind)
            {
                this._kind = value;

                switch (value)
                {
                    case TextBoxKind.Standard:
                    case TextBoxKind.Password:
                        dom.HTMLInputElement input = this.EnsureInputKind();
                        input.type = value == TextBoxKind.Password ? "password" : "text";
                        break;

                    case TextBoxKind.MultiLine:
                        this.EnsureTextAreaKind();
                        break;
                }
            }
        }

        /// <summary>
        /// Applies the current style to our current DOM- element.
        /// </summary>
        void SetStyle()
        {
            if (this.input != null)
            {
                this.input.disabled = this._disabled;
                this.input.placeholder = this._placeholder;
                this.input.readOnly = this._readOnly;
                this.input.type = this._kind == TextBoxKind.Password ? "password" : "text";
            }
            else if (this.area != null)
            {
                this.area.disabled = this._disabled;
                this.area.placeholder = this._placeholder;
                this.area.readOnly = this._readOnly;
            }
        }

        /// <summary>
        /// Sets the text in the DOM- element that the user can change.
        /// </summary>
        void SetText(string text)
        {
            if (this.input != null)
            {
                this.input.value = text;
            }
            else if (this.area != null)
            {
                this.area.value = text;
            }
        }


        dom.HTMLElement any;
        dom.HTMLInputElement input;
        dom.HTMLTextAreaElement area;
        Label validation;

        #endregion
    }
}
