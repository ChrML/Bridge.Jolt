
# Controls

In the Jolt framework any level of display is a control. A control is the Jolt equalent of a React component.


## Example 1 - Creating a simple control

To create the simplest usable control, just create a class based on `HtmlControl`.\
In this example, the constructor builds the DOM- tree within the control's scope every time it is used.

```c#
class Welcome : HtmlControl
{
    public Welcome(string name)
    {
        this.Dom.Append($"<h1>Hello, {name}</h1>");
    }
}
```

This is the equalent of the following React component:

```js
class Welcome extends React.Component {
    render() {
        return <h1>Hello, {this.props.name}</h1>;
    }
}
```


## Example 2 - Using the simple control.

To use the control we just created just add this code to the application's entrypoint:

```c#
Html.AppendToBody(new Welcome("Christian"));
```

This will add the new `Welcome` control to the page body with a name parameter.\
Now our full application looks like this:

```c#
class Welcome : HtmlControl
{
    public Welcome(string name)
    {
        this.Dom.Append($"<h1>Hello, {name}</h1>");
    }
}

public static class Program
{
    public static void Main()
    {
        Html.AppendToBody(new Welcome("Christian"));
    }
}
```


This is equalent to the following React example:


```js
class Welcome extends React.Component {
    render() {
        return <h1>Hello, {this.props.name}</h1>;
    }
}

const element = <Welcome name="Christian" />;
ReactDOM.render(
    element,
    document.body
);
```


## Example 3 - Composing controls

Controls can embed other components as their contents. This lets us use the same control abstraction for any level of detail. A button, a form, a dialog, a screen: in Jolt apps, all those are commonly expressed as controls.


For example, we can create an `App` control that renders `Welcome` many times:

```c#
class Welcome : HtmlControl
{
    public Welcome(string name)
    {
        this.Dom.Append($"<h1>Hello, {name}</h1>");
    }
}

class App : HtmlControl
{
    public App()
    {
        this.Dom
            .Append(new Welcome("Christian"))
            .Append(new Welcome("Ronald"))
            .Append(new Welcome("Maria"));
    }
}

public static class Program
{
    public static void Main()
    {
        Html.AppendToBody<App>();
    }
}
```


This is equalent to the following React- example:

```js
class Welcome extends React.Component {
    render() {
        return <h1>Hello, {this.props.name}</h1>;
    }
}

class App extends React.Component {
    render() {
        return (
            <div>
                <Welcome name="Christian" />
                <Welcome name="Ronald" />
                <Welcome name="Maria" />
            </div>
        );
    }
}

ReactDOM.render(
    <App />,
    document.body
);
```
