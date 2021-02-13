
# Services

A front-end application may often have reusable services used throughout several parts of the application.
Services provide a way of declaring what they can do as interfaces, and then makes them available for use by any control or other services.
A service may have multiple possible implementations to easily be swapped out.

One of the goals of using services is to make the code more unit-testable. As they may be easily replaced by mock-up implementations in a test project.


## Built-in services

The following services are built into Jolt.

* `IApiClient`\
   Provides functionality for calling web-API's asynchronously from the application.
   The default implementation provided by Jolt uses JSON formatting and will do fine for most applications.

* `IErrorHandler`\
   Service for handling unexpected errors that occur in the application.
   The default implementation will output the error to the console. It could be replaced by a custom handler if you e.g. want to implement a "Report error"- function in your app.

* `IGlobalEvents`\
   Provides access to the most common global browser events, such as mouse or keyboard events.
   
* `IJoltImageProvider`\
   Provides the URL's to the images used by the default controls in Jolt.

* `IJoltLocale`\
   Provides the language strings used by the default controls in Jolt. It may be configured or replaced for multi-language implementations.


## Using services from a control

Here is an example of a fully functional control named `ApiClientDemo` using the built-in service `IApiClient`.

This control will show a button and a label to the user. 
When the user presses the button, a web-API at the server is called while the button indicates an action is in progress, and then the label is changed to greet the user based on JSON- data received from the server.

```c#
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
            .Append(this.label)
            .Append(this.btnGetData);
    }

    readonly Button btnGetData = new Button("Get my user data");
    readonly Label label = new Label("Click the button.");

    class UserData
    {
        public string UserName { get; set; }

        public string NickName { get; set; }
    }
}
```

This control may be added just like any control with the following code in the application's entrypoint:

```c#
Html.AppendToBody<ApiClientDemo>();
```

Lets break this code down a little, here's what happens:

1. The control control named `ApiClientDemo` is appended to the HTML body from the application's entrypoint.

2. Jolt looks at the `ApiClientDemo` constructor and see that it depends on the `IApiClient` service.
   Since we have not replaced this service, Jolt will give our control the default implementation for JSON formatted data. The control is constructed by Jolt.

3. The `ApiClientDemo` constructor creates a button and a label, assigns an async handler to the button's Click- event
   and appends both the label and the button to this control's private DOM tree. This control's private DOM tree is appended to the main DOM tree on use from the entrypoint.

4. When the user clicks on the button, the `GetAsync` method of the `IApiClient` service is used to call the webserver's api (`GET DemoData/CurrentUser`).
   The call is async since the webserver needs time to respond to the request. We expect to get this JSON response data from the server:
   ```json
   {
     "UserName":"Christian Lundheim",
	 "NickName":"CL"
   }
   ```

5. We have a private nested class called `UserData` with properties corresponding to the above JSON data. We use the properties of the result data to update the label.


## Custom services

If your api's have a different format than JSON, or you want to mock the `IApiClient` service for unit-test purposes, you can roll your own implementation of it.
All controls and services depending on `IApiClient` would then use your implementation.

We will look at replacing the built-in Jolt- services, or adding custom services in the next chapter.
