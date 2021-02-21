using Bridge;
using Jolt.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jolt.Navigation
{
    /// <summary>
    /// Provides a default implementation for navigation in a single-page application.
    /// </summary>
    [Reflectable(true)]
    sealed class Navigation : INavigation
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Navigation"/> class.
        /// </summary>
        /// <param name="browserHistory">The browser history API to use.</param>
        public Navigation(IBrowserHistory browserHistory)
        {
            this.history = browserHistory ?? throw new ArgumentNullException(nameof(browserHistory));
            this.history.PopState += this.BrowserHistory_OnPopState;
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public IHtmlElement Current { get; private set; }

        #endregion

        #region Events

        /// <inheritdoc/>
        public event EventHandler<AfterNavigateEventArgs> AfterNavigate;

        /// <summary>
        /// Raises the <see cref="AfterNavigate"/> event.
        /// </summary>
        /// <param name="e"></param>
        void OnAfterNavigated(AfterNavigateEventArgs e) => this.AfterNavigate?.Invoke(this, e);

        /// <inheritdoc/>
        public event EventHandler<BeforeNavigateEventArgs> BeforeNavigate;

        /// <summary>
        /// Raises the <see cref="BeforeNavigate"/> event.
        /// </summary>
        /// <param name="e"></param>
        void OnBeforeNavigated(BeforeNavigateEventArgs e) => this.BeforeNavigate?.Invoke(this, e);

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void Go(int delta)
        {
            if (delta != 0)
            {
                this.history.Go(delta);
            }
            else
            {
                this.GoToItem(this.currentItem);
            }
        }

        /// <inheritdoc/>
        public async Task NavigateToAsync(string title, Func<NavigateData, IHtmlElement> createControl, NavigateOptions options = null)
        {
            options = options ?? NavigateOptions.Default;

            // Go to the new item.
            Item newItem = this.CreateItem(title, createControl);
            bool firstNavigate = this.currentItem == null;

            this.currentItem = newItem;
            await this.GoToItemAsync(newItem);

            // Add the navigate to the browser's history.
            string json = this.CreateStateJson(this.currentItem, this.Current, out string url);
            if (firstNavigate)
            {
                this.history.ReplaceState(json, title, url);
            }
            else
            {
                this.history.PushState(json, title, url);
            }
        }

        #endregion

        #region Privates

        /// <summary>
        /// Called by the browser on history state pop.
        /// </summary>
        void BrowserHistory_OnPopState(object sender, BrowserHistoryPopEventArgs e)
        {
            // TODO: Include the user state.

            // Parse the browser state.
            State state = null;
            if (e.State is string json && !String.IsNullOrWhiteSpace(json))
            {
                try
                {
                    state = JsonConvert.DeserializeObject<State>(json);
                }
                catch (Exception exception)
                {
                    this.OnError(exception);
                }
            }

            // Navigate if successful.
            if (state != null)
            {
                if (!String.IsNullOrEmpty(state.Id) && this.historyItems.TryGetValue(state.Id, out Item item))
                {
                    this.currentItem = item;
                    this.GoToItem(item);
                }
                else
                {
                    // TODO: Not yet implemented.
                }
            }
        }

        /// <summary>
        /// Creates a navigation item.
        /// </summary>
        Item CreateItem(string title, Func<NavigateData, IHtmlElement> createControl)
        {
            Item item = new Item(title, createControl);
            this.historyItems[item.Id] = item;
            return item;
        }

        /// <summary>
        /// Creates a new state for serialization.
        /// </summary>
        State CreateState(Item item, IHtmlElement element)
        {
            // Grab the page state if any.
            object userState = null;
            if (element is IControlState hasState)
            {
                try
                {
                    userState = hasState.GetState();
                }
                catch (Exception getPageStateException)
                {
                    this.OnError(getPageStateException);
                }
            }

            // Create the state that we will store in the browser as JSON.
            return new State
            {
                Id = item.Id,
                Title = item.Title,
                Url = null,
                UserState = userState
            };
        }

        /// <summary>
        /// Create a new state as string.
        /// </summary>
        string CreateStateJson(Item item, IHtmlElement element, out string url)
        {
            State state = this.CreateState(item, element);
            url = state.Url;
            return JsonConvert.SerializeObject(state);
        }

        /// <summary>
        /// Gets the current navigator.
        /// </summary>
        INavigator GetNavigator() => Services.Get<INavigator>();

        /// <summary>
        /// Starts a task to navigate to the given item.
        /// </summary>
        /// <param name="item"></param>
        async void GoToItem(Item item)
        {
            try
            {
                await this.GoToItemAsync(item);
            }
            catch (Exception exception)
            {
                this.OnError(exception);
            }
        }

        /// <summary>
        /// Navigates to the given item.
        /// </summary>
        async Task GoToItemAsync(Item item)
        {
            // Get the navigator to use.
            INavigator navigator = this.GetNavigator();
            NavigateData data = new NavigateData(Services.Default, item.Title);

            // Create the control to navigate to.
            IHtmlElement element;
            try
            {
                element = item.CreateControl(data);
            }
            catch (Exception createException)
            {
                this.Current = null;
                navigator.SetError(data, null, createException);
                return;
            }

            // Navigation begun event. It may cancel the navigation.
            BeforeNavigateEventArgs beginArgs = new BeforeNavigateEventArgs(data, element);
            try
            {
                this.OnBeforeNavigated(beginArgs);
            }
            catch (Exception eventException)
            {
                this.OnError(eventException);
            }

            if (beginArgs.Cancel)
            {
                return;
            }
            

            // Notify the navigator to begin the navigation. It could be cancelled here as well.
            await navigator.BeginNavigateAsync(beginArgs);
            if (beginArgs.Cancel)
            {
                return;
            }

            // Dispose the old page if required.
            try
            {
                if (this.Current is IDisposable disposableCurrent)
                {
                    disposableCurrent.Dispose();
                }
            }
            catch (Exception disposeException)
            {
                this.OnError(disposeException);
            }

            // Load the new page's contents if required.
            try
            {
                if (element is IWithAsyncLoad requiresAsync)
                {
                    await requiresAsync.LoadAsync();
                }
            }
            catch (Exception loadException)
            {

                this.Current = null;
                navigator.SetError(data, null, loadException);
                return;
            }

            // Notify the navigator to complete the navigation.
            await navigator.EndNavigateAsync(new AfterNavigateEventArgs(data, element));
            this.Current = element;

            // Raise the event for a completed navigation.
            try
            {
                this.OnAfterNavigated(new AfterNavigateEventArgs(data, element));
            }
            catch (Exception eventException)
            {
                this.OnError(eventException);
            }
        }

        /// <summary>
        /// Called when an unhandled error occurs within the navigation system.
        /// </summary>
        /// <param name="exception"></param>
        void OnError(Exception exception)
        {
            Services.Get<IErrorHandler>().OnError(exception);
        }


        Item currentItem = null;

        readonly IBrowserHistory history;
        readonly Dictionary<string, Item> historyItems = new Dictionary<string, Item>();


        [Reflectable(false)]
        class Item
        {
            public Item(string title, Func<NavigateData, IHtmlElement> createControl)
            {
                this.Title = title;
                this.CreateControl = createControl;
            }

            public Func<NavigateData, IHtmlElement> CreateControl { get; }

            public string Id { get; } = Guid.NewGuid().ToString().ToLower();

            public string Title { get; }
        }

        [Reflectable(true)]
        class State
        {
            public string Id { get; set; }

            public int Index { get; set; }

            public string Url { get; set; }

            public string Title { get; set; }

            [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
            public object UserState { get; set; }
        }

        #endregion
    }
}
