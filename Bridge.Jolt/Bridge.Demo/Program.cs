using Bridge.Jolt;
using Bridge.Jolt.Controls;
using Retyped;
using System.Threading.Tasks;

namespace Bridge.Demo
{
    public static class Program
    {
        public static void Main()
        {
            dom.HTMLElement root = Html.GetByIdRequired<dom.HTMLElement>("Demo-Root");

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

            root
                .Append(btn1)
                .Append(btn2)
                .Append(btn3);

        }
    }
}
