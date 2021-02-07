using Jolt.Controls;
using System;
using System.Threading.Tasks;

namespace Jolt.Demo.Controls
{
    /// <summary>
    /// Implements a control for demonstrating all the features of a <see cref="TextBox"/>.
    /// </summary>
    class TextBoxDemo : HtmlControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonDemo"/> class.
        /// </summary>
        public TextBoxDemo()
        {
            TextBox edit1 = new TextBox("Text already here.");


            TextBox edit2 = new TextBox
            {
                Text = "Disabled text you can't change",
                Disabled = true
            };

            TextBox edit3 = new TextBox
            {
                ReadOnly = true,
                Text = "Disabled text you can't change"
            };

            TextBox edit4 = new TextBox
            {
                Placeholder = "Placeholder"
            };

            edit4.TextInput += (o, e) =>
            {
                if (String.IsNullOrEmpty(edit4.Text))
                {
                    edit4.SetValidateError("Input cannot be empty.");
                }
                else
                {
                    edit4.ClearValidateError();
                }
            };

            TextBox edit5 = new TextBox
            {
                Kind = TextBoxKind.Password,
                Placeholder = "Input your password here"
            };

            TextBox edit6 = new TextBox
            {
                Kind = TextBoxKind.MultiLine
            };


            this.DomElement
                .Append(edit1)
                .Append(edit2)
                .Append(edit3)
                .Append(edit4)
                .Append(edit5)
                .Append(edit6);
        }
    }
}
