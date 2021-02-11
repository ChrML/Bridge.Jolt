using Jolt.Controls;
using Jolt.Demo.Controls;
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

            root
                .New<Label>(label =>
                {
                    label.Text = "Here is a few buttons: ";
                })
                .New<ButtonDemo>()
                .New<TextBoxDemo>()
                .New<ListView>(list =>
                {
                    list.Items
                        .Add("Item 1")
                        .Add("Item 2")
                        .Add("Item 3");

                });
        }
    }
}
