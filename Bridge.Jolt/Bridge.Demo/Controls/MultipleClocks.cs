using Jolt.Controls;

namespace Jolt.Demo.Controls
{
    public class MultipleClocks: HtmlControl
    { 
        public MultipleClocks()
        {
            this.Dom
                .Append<Clock>()
                .Append<Clock>()
                .Append<Clock>();
        }
    }
}
