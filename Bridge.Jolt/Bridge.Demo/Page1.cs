using Jolt.Controls;
using Jolt.Demo.Controls;
using Jolt.Navigation;
using Retyped;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jolt.Demo
{
    class Page1: HtmlControl
    {
        public Page1()
        {
            this.Dom.Append<Clock>();
            this.Dom.Append<MultipleClocks>();

            Spinner spinner = new Spinner(this.Dom);
            spinner.SetStatus(TaskStatus.InProgress);

            this.Dom.Append<ApiClientDemo>();

            TextBox textBox = new TextBox();
            this.Dom.Append(textBox);

            Button btnPage2 = new Button("Go to page 2");
            btnPage2.Click += (o, e) => Services.Default.NavigateTo<Page2>("Page 2", new
            {
                CustomText = textBox.Text
            });
            this.Dom.Append(btnPage2);

            this.Dom
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
