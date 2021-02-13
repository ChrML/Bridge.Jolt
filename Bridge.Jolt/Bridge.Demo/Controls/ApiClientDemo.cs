using Jolt.Abstractions;
using Jolt.Controls;

namespace Jolt.Demo.Controls
{
    class ApiClientDemo: HtmlControl
    {
        public ApiClientDemo(IApiClient api)
        {
            this.btnGetData.ClickAsync = async (sender, e) =>
            {
                UserData data = await api.GetAsync<UserData>("DemoData/CurrentUser");
                this.label.Text = $"Hello {data.UserName}. Your nickname is {data.NickName}.";
            };

            this.Dom
                .Append(this.btnGetData)
                .Append(this.label);
        }

        readonly Button btnGetData = new Button("Get my username");
        readonly Label label = new Label("Click the button.");

        class UserData
        {
            public string UserName { get; set; }

            public string NickName { get; set; }
        }
    }
}
