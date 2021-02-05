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

            Button btn1 = new Button
            {
                Text = "Click me"
            };
            btn1.Click += (o, e) => { };

            Button btn2 = new Button
            {
                Text = "Click me (2 sec)",
                ClickAsync = async (o, e) => await Task.Delay(2000)
            };

            Button btn3 = new Button
            {
                Text = "Click me (5 sec)",
                ClickAsync = async (o, e) => await Task.Delay(5000)
            };


            ListView list1 = new ListView();
            list1.Items
                .Add("Item 1")
                .Add("Item 2")
                .Add("Item 3");

            root
                .Append(label1)
                .Append(btn1)
                .Append(btn2)
                .Append(btn3)
                .Append(list1);
        }
    }
}
