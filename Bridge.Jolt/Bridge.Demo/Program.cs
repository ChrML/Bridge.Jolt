using Bridge.Demo.Controls;
using Bridge.Demo.CustomService;
using Bridge.Jolt;
using Bridge.Jolt.Controls;
using Bridge.Jolt.Services;
using Retyped;
using System.Threading.Tasks;

namespace Bridge.Demo
{
    public static class Program
    {
        public static void Main()
        {
            AppServices.UseStartup<Startup>();

            IMyCustomService test = AppServices.Default.Resolve<IMyCustomService>();

            dom.HTMLElement root = Html.GetByIdRequired<dom.HTMLElement>("Demo-Root");

            Label label1 = new Label("Here is a few buttons: ");


            ButtonDemo btnDemo = new ButtonDemo();


            ListView list1 = new ListView();
            list1.Items
                .Add("Item 1")
                .Add("Item 2")
                .Add("Item 3");

            root
                .Append(label1)
                .Append(btnDemo)
                .Append(list1);
        }
    }
}
