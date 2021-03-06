﻿using Jolt.Controls;
using System.Threading.Tasks;

namespace Jolt.Demo.Controls
{
    /// <summary>
    /// Implements a control for demonstrating all the features of a <see cref="Button"/>.
    /// </summary>
    class ButtonDemo: HtmlControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonDemo"/> class.
        /// </summary>
        public ButtonDemo()
        {
            Button btn1 = new Button("Click me");
            btn1.Click += (o, e) => { };

            Button btn2 = new Button
            {
                ClickAsync = async (o, e) => await Task.Delay(2000),
                Text = "Click me (2 sec)"
            };

            Button btn3 = new Button
            {
                ClickAsync = async (o, e) =>
                {
                    await Task.Delay(5000);
                    this.Dom.Remove();
                },
                Image = "img/Play.png",
                Text = "Play (5 sec)"
            };

            this.Dom
                .Append(btn1)
                .Append(btn2)
                .Append(btn3);
        }
    }
}
