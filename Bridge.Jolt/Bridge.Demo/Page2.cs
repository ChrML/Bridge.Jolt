using Jolt.Controls;

namespace Jolt.Demo
{
    class Page2: HtmlControl
    {
        public Page2(string customText = null)
        {
            this.Dom.Append(new Label
            {
                Text = "This is page 2 with text: " + customText ?? "null"
            });
            
        }

    }
}
