using Jolt.Controls;
using Jolt.Demo.Controls;
using Jolt.Navigation;
using Retyped;

namespace Jolt.Demo
{
    public static class Program
    {
        public static void Main()
        {

            var root = Html.GetById<dom.HTMLDivElement>("NavigationArea");
            AppServices.ConfigureServices(services =>
            {
                services.SetSimpleNavigatorIn(root);
            });


            Html.GetBody()
                .Insert(0, new Button
                {
                    ClickAction = e => Services.Default.NavigateTo<Page1>("Page 1"),
                    Text = "Go to Page 1"
                })

                .Insert(1, new Button
                {
                    ClickAction = e => Services.Default.NavigateTo<Page2>("Page 2"),
                    Text = "Go to Page 2"
                });



            //AppServices.UseStartup<Startup>();


            ////dom.HTMLElement root = Html.GetBody();

            //Button btnPage1 = new Button
            //{
            //    Text = "Go to page 1"
            //};
            //btnPage1.Click += (o, e) => Services.Default.NavigateTo<Page1>("Page 1", new
            //{

            //});

            //root.Append(btnPage1);




        }
    }
}
