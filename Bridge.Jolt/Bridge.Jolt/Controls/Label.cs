using Bridge;
using System;

namespace Jolt.Controls
{
    /// <summary>
    /// Implements a label control to display static text.
    /// </summary>
    [Reflectable(true)]
    public class Label: HtmlControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with an initial text configured.
        /// </summary>
        public Label(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the text to render in this label.
        /// </summary>
        public string Text
        {
            get => this._text;
            set
            {
                if (value != this._text)
                {
                    this._text = value;
                    if (String.IsNullOrEmpty(value))
                    {
                        this.DomElement.innerHTML = "&nbsp;";
                    }
                    else
                    {
                        this.DomElement.innerText = value;
                    }
                }
            }
        }
        string _text;

        #endregion
    }
}
