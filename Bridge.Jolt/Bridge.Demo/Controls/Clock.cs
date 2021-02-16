using Jolt.Controls;
using System;
using System.Threading;

namespace Jolt.Demo.Controls
{
    public class Clock : HtmlControl, ILifecycle
    {
        public Clock()
        {
            this.Dom
                .Append("<h1>Hello, world!</h1>")
                .Wrap("h2", this.clock);
        }

        public void Mounted()
        {
            this.timer = new Timer(e => this.clock.Text = "It is " + DateTime.Now.ToLocaleString(), null, 0, 1000);
        }

        public void Unmounted()
        {
            this.timer.Dispose();
        }

        readonly Label clock = new Label();
        Timer timer;
    }
}
