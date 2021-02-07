using Jolt.Controls;
using Jolt.Demo.Controls;
using Jolt.Demo.CustomService;
using Jolt.Services;
using Retyped;

namespace Jolt.Demo
{
    public static class Program
    {
        public static void Main()
        {
            AppServices.UseStartup<Startup>();

            dom.HTMLElement root = Html.GetByIdRequired<dom.HTMLElement>("Demo-Root");

            Label label1 = new Label("Here is a few buttons: ");


            ListView list1 = new ListView();
            list1.Items
                .Add("Item 1")
                .Add("Item 2")
                .Add("Item 3");

            root
                .Append(label1)
                .Append<ButtonDemo>()
                .Append<TextBoxDemo>()
                .Append(list1);
        }
    }
}
