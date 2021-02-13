
# Controls: State- handling

In a control, any type of state is simply stored as private variables or properties of the class as per standard C#.\
If your state is a private implementation detail, make it a private variable or a get-only property.\
If your state is something that other controls should be able to change, make it a property with a getter and a setter.


# Controls: Lifecycle- handling


## Example 1 - Creating a Clock- control

We create a new control named `Clock`, which will encapsulate the functionality required for a clock.\
In this example, the clock will not update, but only show the clock at the time the control was created. 

```c#
public class Clock : HtmlControl
{
    public Clock()
    {
        this.Dom
            .Append("<h1>Hello, world!</h1>")
            .Wrap("h2", this.clock);

        this.clock.Text = "It is " + DateTime.Now.ToLocaleString();
    }

    readonly Label clock = new Label();
}
```

This is equalent to the following React component:
```js
class Clock extends React.Component {
    constructor(props) {
        super(props);
        this.state = {date: new Date()};
    }

    render() {
        return (
        <div>
            <h1>Hello, world!</h1>
            <h2>It is {this.state.date.toLocaleTimeString()}.</h2>
        </div>
        );
    }
}
```


## Example 2 - Make the clock tick

In applications with many controls, it’s very important to free up resources taken by the controls when they are destroyed.

We want to set up a timer whenever the `Clock` is rendered to the DOM for the first time.
This is called “mounting”, and is very similar to how React handles this.

We also want to clear that timer whenever the `Clock` is removed from the DOM. This is called “unmounting”.

To make the control aware of when it mounts and unmounts, we need to decorate the class with the `ILifecycle` interface.
Then add the two methods required by the interface that will be called whenever the control is mounted or unmounted.

Now in the following example we create a timer when our `Clock` is added to the DOM. And when the `Clock` is removed from the DOM, we no longer need the timer, so we dispose it to prevent it from keep raising timer- events.

```c#
public class Clock : HtmlControl, ILifecycle
{
    public Clock()
    {
        this.Dom
            .Append("<h1>Hello, world!</h1>")
            .Wrap("h2", this.clock);
        }

        public void Mounted()
        {
            this.timer = new Timer(
                e => this.clock.Text = "It is " + DateTime.Now.ToLocaleString(), 
                null, 0, 1000);
        }

        public void Unmounted()
        {
            this.timer.Dispose();
        }

        Timer timer;
        readonly Label clock = new Label();
    }
```

Now the clock is alive and updating every second.

The clock may still be embedded into any other control, or it can be the program root control.
We made the mount/unmount handling an internal matter of the `Clock`, and we still use it just like any other control:

```c#
Html.AppendToBody<Clock>();
```

This is equalent to the following React component:

```js
class Clock extends React.Component {
    constructor(props) {
        super(props);
        this.state = {date: new Date()};
    }

    componentDidMount() {
        this.timerID = setInterval(
            () => this.tick(),
            1000
        );
    }

    componentWillUnmount() {
        clearInterval(this.timerID);
    }

    tick() {
        this.setState({
            date: new Date()
        });
    }

    render() {
        return (
            <div>
                <h1>Hello, world!</h1>
                <h2>It is {this.state.date.toLocaleTimeString()}.</h2>
            </div>
        );
    }
}

ReactDOM.render(
    <Clock />,
     document.body
);
```

