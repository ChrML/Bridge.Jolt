
# Navigation: The basics

In the Jolt framework, any control can be the contents of a page. And any control can be navigated to as a page.

When using Jolt's navigation framework to switch page contents, it will automatically integrate seamlessly with the browser's history.

In most single-page applications, the page contents is not the top-level control of the HTML body.
The top-level of a page is usually just a part of the HTML body, with other contents around it.
Such other contents could be a menu, header or footer, making up the rest of the HTML body.

Because of this Jolt can put your page control anywhere in the HTML body of your choice. And you could have other controls around 
for your page's menu/header/footer framework.
Neither the controls around your page contents, nor the page contents themselves, need to be aware of this arrangement.


## Example 1 - Simple application that can navigate between two pages.

This is a sample application with two buttons that enable you to navigate between two pages.
The two buttons are located outside the navigation area. We have provided a DIV- element where the page contents are insered.

Place the following code in your application's entrypoint:


```c#
// Configures the default navigator service to put the page contents in a DIV of our choice.
var root = Html.GetById<dom.HTMLDivElement>("NavigationArea");
AppServices.ConfigureServices(services =>
{
    services.SetSimpleNavigatorIn(root);
});

// Creates two buttons at the beginning of our HTML- body for navigating between page 1 and page 2.
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
```

Then we can create two standard controls that implement the contents of page 1 and page 2:
```c#
class Page1 : HtmlControl
{
    public Page1()
    {
        this.Dom.Append(new Label
        {
            Text = "This is page 1"
        });
    }
}


class Page2 : HtmlControl
{
    public Page2()
    {
        this.Dom.Append(new Label
        {
            Text = "This is page 2"
        });
    }
}
```

Add this to your HTML code so the navigator has a DIV to put the contents navigated to:
```html
<html>
  <body>
    <div id="NavigationArea">
      <!-- Page contents will go here -->
    </div>
  </body>
</html>
```



## Example 2 - Passing parameters and state to the page.

Often there will be a need to pass data to the page we navigate to.
For example, you could have a generic page that will render something of the caller's choice.

Here we have created a third page that takes an argument.
```c#
class Page3 : HtmlControl
{
    public Page3(string customText = null)
    {
        this.Dom.Append(new Label
        {
            Text = "This is page 3 with the text: " + customText ?? String.Empty
        });
    }
}
```

The `= null` indicates that the parameter is optional, so the caller is not required to pass it.

Add this code to the application's entrypoint to add a third button for navigating to page 3.
Here we provide the text that will be shown by page 3 as a parameter:
```c#
    .Insert(2, new Button
    {
        ClickAction = e => Services.Default.NavigateTo<Page3>("Page 3", new
        {
            CustomText = "My custom parameter value"
        }),
        Text = "Go to Page 3"
    });
```

Clicking on the page 3 button will now render this text as the page contents:
```
This is page 3 with the text: My custom parameter value
```


## Example 3 - Combining navigation with services

As we learned from the previous chapter, any control can depend on services to provide functionality.

You may also add parameters to your page to inject a service.
Here we have modified page 3 to be able to call API's by injecting the `IApiClient` service:

```c#
class Page3 : HtmlControl
{
    public Page3(IApiClient api, string customText = null)
    {
        this.Dom.Append(new Label
        {
            Text = "This is page 3 with the text: " + customText ?? String.Empty
        });
        
        // You could add a button that uses a REST API here.
    }
}
```

As an example, even when page 3 now uses a service, you can still navigate to the page with the exact same syntax:

```c#
Services.Default.NavigateTo<Page3>("Page 3", new
{
    CustomText = "My custom parameter value"
})
```

It's easy to extend the pages later to use more services, since none of the page's callers need to be aware of this.
Jolt will automatically resolve the services, and supply your custom parameters in addition to the services when
creating your page.

As an example, we can change `Page3` to use the `INavigation` service, and add a button using it to navigate back to page 1.
We can do this and no other part of our application have to change.


```c#
class Page3 : HtmlControl
{
    public Page3(INavigation navigation, string customText = null)
    {
        this.Dom
            .Append(new Label
            {
                Text = "This is page 3 with the text: " + customText ?? String.Empty
            })
            .Append(new Button
            {
                ClickAction = e => navigation.NavigateTo<Page1>(),
                Text = "Go back to page 1"
            });
    }
}
```
