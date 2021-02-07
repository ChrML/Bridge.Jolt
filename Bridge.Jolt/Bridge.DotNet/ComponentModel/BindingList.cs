using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.ComponentModel
{
    /// <summary>
    /// Provides a generic collection that supports data binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class BindingList<T> : Collection<T>, IBindingList, ICancelAddNew, IRaiseItemChangedEvents
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingList{T}"/> class using default values.
        /// </summary>
        public BindingList() 
            : base()
        {
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingList{T}"/> class with the specified list.
        /// </summary>
        /// <param name="list">An <see cref="IList{T}"/> of items to be contained in the <see cref="BindingList{T}"/>.</param>
        public BindingList(IList<T> list) 
            : base(list)
        {
            this.Initialize();
        }

        /// <summary>
        /// Common initialization used by both constructors.
        /// </summary>
        void Initialize()
        {
            // Set the default value of AllowNew based on whether type T has a default constructor
            this.allowNew = this.ItemTypeHasDefaultConstructor;

            // Check for INotifyPropertyChanged
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T)))
            {
                // Supports INotifyPropertyChanged
                this.raiseItemChangedEvents = true;

                // Loop thru the items already in the collection and hook their change notification.
                foreach (T item in this.Items)
                {
                    this.HookPropertyChanged(item);
                }
            }
        }

        /// <summary>
        /// Gets if the item type has a default constructor.
        /// </summary>
        bool ItemTypeHasDefaultConstructor
        {
            get
            {
                // NOTE: This implementation is not complete.

                Type itemType = typeof(T);
                return !itemType.IsAbstract && !itemType.IsInterface;
            }
        }

        #endregion

        #region AddingNew event

        /// <summary>
        /// Occurs before an item is added to the list.
        /// </summary>
        public event AddingNewEventHandler AddingNew
        {
            add
            {
                bool allowNewWasTrue = this.AllowNew;
                this.onAddingNew += value;
                if (allowNewWasTrue != this.AllowNew)
                {
                    this.FireListChanged(ListChangedType.Reset, -1);
                }
            }
            remove
            {
                bool allowNewWasTrue = this.AllowNew;
                this.onAddingNew -= value;
                if (allowNewWasTrue != this.AllowNew)
                {
                    this.FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /// <summary>
        /// Occurs before an item is added to the list.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAddingNew(AddingNewEventArgs e) => this.onAddingNew?.Invoke(this, e);

        /// <summary>
        /// Private helper method.
        /// </summary>
        /// <returns></returns>
        object FireAddingNew()
        {
            AddingNewEventArgs e = new AddingNewEventArgs(null);
            this.OnAddingNew(e);
            return e.NewObject;
        }

        #endregion

        #region ListChanged event

        /// <summary>
        /// Occurs when the list or an item in the list changes.
        /// </summary>
        public event ListChangedEventHandler ListChanged;

        /// <summary>
        /// Occurs when the list or an item in the list changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnListChanged(ListChangedEventArgs e) => ListChanged?.Invoke(this, e);

        /// <summary>
        /// Gets or sets a value indicating whether adding or removing items within the list raises <see cref="ListChanged"/> events.
        /// </summary>
        public bool RaiseListChangedEvents { get; set; } = true;

        /// <summary>
        /// Raises a <see cref="ListChanged"/> event of type <see cref="ListChangedType.Reset"/>.
        /// </summary>
        public void ResetBindings() => this.FireListChanged(ListChangedType.Reset, -1);

        /// <summary>
        /// Raises a <see cref="ListChanged"/> event of type <see cref="ListChangedType.ItemChanged"/> for the item at the specified position.
        /// </summary>
        /// <param name="position">A zero-based index of the item to be reset.</param>
        public void ResetItem(int position) => this.FireListChanged(ListChangedType.ItemChanged, position);

        /// <summary>
        /// Private helper method to raise changed event if we need to.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        void FireListChanged(ListChangedType type, int index)
        {
            if (this.RaiseListChangedEvents)
            {
                this.OnListChanged(new ListChangedEventArgs(type, index));
            }
        }

        #endregion

        #region Collection<T> overrides

        // Collection<T> funnels all list changes through the four virtual methods below.
        // We override these so that we can commit any pending new item and fire the proper ListChanged events.

        protected override void ClearItems()
        {
            this.EndNew(this.addNewPos);

            if (this.raiseItemChangedEvents)
            {
                foreach (T item in this.Items)
                {
                    this.UnhookPropertyChanged(item);
                }
            }

            base.ClearItems();
            this.FireListChanged(ListChangedType.Reset, -1);
        }

        protected override void InsertItem(int index, T item)
        {
            this.EndNew(this.addNewPos);
            base.InsertItem(index, item);

            if (this.raiseItemChangedEvents)
            {
                this.HookPropertyChanged(item);
            }

            this.FireListChanged(ListChangedType.ItemAdded, index);
        }

        protected override void RemoveItem(int index)
        {
            // Need to all RemoveItem if this on the AddNew item
            if (!this.allowRemove && !(this.addNewPos >= 0 && this.addNewPos == index))
            {
                throw new NotSupportedException();
            }

            this.EndNew(this.addNewPos);

            if (this.raiseItemChangedEvents)
            {
                this.UnhookPropertyChanged(this[index]);
            }

            base.RemoveItem(index);
            this.FireListChanged(ListChangedType.ItemDeleted, index);
        }

        protected override void SetItem(int index, T item)
        {

            if (this.raiseItemChangedEvents)
            {
                this.UnhookPropertyChanged(this[index]);
            }

            base.SetItem(index, item);

            if (this.raiseItemChangedEvents)
            {
                this.HookPropertyChanged(item);
            }

            this.FireListChanged(ListChangedType.ItemChanged, index);
        }

        #endregion

        #region ICancelAddNew interface

        /// <summary>
        /// Discards a pending new item.
        /// </summary>
        /// <param name="itemIndex"></param>
        public virtual void CancelNew(int itemIndex)
        {
            if (this.addNewPos >= 0 && this.addNewPos == itemIndex)
            {
                this.RemoveItem(this.addNewPos);
                this.addNewPos = -1;
            }
        }

        /// <summary>
        /// Commits a pending new item to the collection.
        /// </summary>
        /// <param name="itemIndex"></param>
        public virtual void EndNew(int itemIndex)
        {
            if (this.addNewPos >= 0 && this.addNewPos == itemIndex)
            {
                this.addNewPos = -1;
            }
        }

        #endregion

        #region IBindingList interface

        /// <summary>
        /// Adds a new item to the collection.
        /// </summary>
        /// <returns></returns>
        public T AddNew()
        {
            return (T)(this as IBindingList).AddNew();
        }

        object IBindingList.AddNew()
        {
            // Create new item and add it to list
            object newItem = this.AddNewCore();

            // Record position of new item (to support cancellation later on)
            this.addNewPos = (newItem != null) ? this.IndexOf((T)newItem) : -1;

            // Return new item to caller
            return newItem;
        }

        bool AddingNewHandled => this.onAddingNew != null && this.onAddingNew.GetInvocationList().Length > 0;

        /// <summary>
        /// Adds a new item to the end of the collection.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The base implementation raises the <see cref="AddingNew"/> event to allow an event handler to
        /// supply a custom item to add to the list. Otherwise an item of type T is created.
        /// The new item is then added to the end of the list.
        /// </remarks>
        protected virtual object AddNewCore()
        {
            // Allow event handler to supply the new item for us
            object newItem = this.FireAddingNew();

            // If event hander did not supply new item, create one ourselves
            if (newItem == null)
            {
                Type type = typeof(T);
                newItem = Activator.CreateInstance(type);
            }

            // Add item to end of list. Note: If event handler returned an item not of type T,
            // the cast below will trigger an InvalidCastException. This is by design.
            this.Add((T)newItem);

            // Return new item to caller
            return newItem;
        }

        /// <summary>
        /// Gets or sets a value indicating whether you can add items to the list using the <see cref="AddNew"/> method.
        /// </summary>
        public bool AllowNew
        {
            get
            {
                //If the user set AllowNew, return what they set.  If we have a default constructor, allowNew will be 
                //true and we should just return true.
                if (this.userSetAllowNew || this.allowNew)
                {
                    return this.allowNew;
                }
                //Even if the item doesn't have a default constructor, the user can hook AddingNew to provide an item.
                //If there's a handler for this, we should allow new.
                return this.AddingNewHandled;
            }
            set
            {
                bool oldAllowNewValue = this.AllowNew;
                this.userSetAllowNew = true;
                //Note that we don't want to set allowNew only if AllowNew didn't match value,
                //since AllowNew can depend on onAddingNew handler
                this.allowNew = value;
                if (oldAllowNewValue != value)
                {
                    this.FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /* private */
        bool IBindingList.AllowNew => this.AllowNew;

        /// <summary>
        /// Gets or sets a value indicating whether items in the list can be edited.
        /// </summary>
        public bool AllowEdit
        {
            get => this.allowEdit;
            set
            {
                if (this.allowEdit != value)
                {
                    this.allowEdit = value;
                    this.FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /* private */
        bool IBindingList.AllowEdit => this.AllowEdit;

        /// <summary>
        /// Gets or sets a value indicating whether you can remove items from the collection.
        /// </summary>
        public bool AllowRemove
        {
            get => this.allowRemove;
            set
            {
                if (this.allowRemove != value)
                {
                    this.allowRemove = value;
                    this.FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /* private */
        bool IBindingList.AllowRemove => this.AllowRemove;

        bool IBindingList.SupportsChangeNotification => this.SupportsChangeNotificationCore;

        /// <summary>
        /// Gets a value indicating whether <see cref="ListChanged"/> events are enabled.
        /// </summary>
        protected virtual bool SupportsChangeNotificationCore => true;

        bool IBindingList.SupportsSearching => this.SupportsSearchingCore;

        /// <summary>
        /// Gets a value indicating whether the list supports searching.
        /// </summary>
        protected virtual bool SupportsSearchingCore => false;


        bool IBindingList.SupportsSorting => this.SupportsSortingCore;

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected virtual bool SupportsSortingCore => false;

        bool IBindingList.IsSorted => this.IsSortedCore;

        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        protected virtual bool IsSortedCore => false;

        PropertyDescriptor IBindingList.SortProperty => this.SortPropertyCore;

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns <c>null</c>.
        /// </summary>
        protected virtual PropertyDescriptor SortPropertyCore => null;

        ListSortDirection IBindingList.SortDirection => this.SortDirectionCore;

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        protected virtual ListSortDirection SortDirectionCore => ListSortDirection.Ascending;

        void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            this.ApplySortCore(prop, direction);
        }

        /// <summary>
        /// Sorts the items if overridden in a derived class; otherwise, throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="direction"></param>
        /// <exception cref="NotSupportedException">Always thrown in default implementation.</exception>
        protected virtual void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            this.RemoveSortCore();
        }

        /// <summary>
        /// Removes any sort applied with <see cref="ApplySortCore(PropertyDescriptor, ListSortDirection)"/> if sorting is implemented in a derived class;
        /// otherwise, raises <see cref="NotSupportedException"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">Always thrown in default implementation.</exception>
        protected virtual void RemoveSortCore()
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor prop, object key)
        {
            return this.FindCore(prop, key);
        }

        /// <summary>
        /// Searches for the index of the item that has the specified property descriptor with the specified value, if searching
        /// is implemented in a derived class; otherwise, a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Always thrown in default implementation.</exception>
        protected virtual int FindCore(PropertyDescriptor prop, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.AddIndex(PropertyDescriptor prop)
        {
            // Not supported
        }

        void IBindingList.RemoveIndex(PropertyDescriptor prop)
        {
            // Not supported
        }

        #endregion

        #region Property Change Support

        void HookPropertyChanged(T item)
        {
            // Note: inpc may be null if item is null, so always check.
            if (item is INotifyPropertyChanged inpc)
            {
                if (this.propertyChangedEventHandler == null)
                {
                    this.propertyChangedEventHandler = new PropertyChangedEventHandler(this.Child_PropertyChanged);
                }
                inpc.PropertyChanged += this.propertyChangedEventHandler;
            }
        }

        void UnhookPropertyChanged(T item)
        {
            // Note: inpc may be null if item is null, so always check.
            if (item is INotifyPropertyChanged inpc && null != this.propertyChangedEventHandler)
            {
                inpc.PropertyChanged -= this.propertyChangedEventHandler;
            }
        }

        void Child_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.RaiseListChangedEvents)
            {
                if (sender == null || e == null || String.IsNullOrEmpty(e.PropertyName))
                {
                    // Fire reset event (per INotifyPropertyChanged spec)
                    this.ResetBindings();
                }
                else
                {
                    // The change event is broken should someone pass an item to us that is not
                    // of type T.  Still, if they do so, detect it and ignore.  It is an incorrect
                    // and rare enough occurrence that we do not want to slow the mainline path
                    // with "is" checks.
                    T item;

                    try
                    {
                        item = (T)sender;
                    }
                    catch (InvalidCastException)
                    {
                        this.ResetBindings();
                        return;
                    }

                    // Find the position of the item.  This should never be -1.  If it is,
                    // somehow the item has been removed from our list without our knowledge.
                    int pos = this.lastChangeIndex;

                    if (pos < 0 || pos >= this.Count || !this[pos].Equals(item))
                    {
                        pos = this.IndexOf(item);
                        this.lastChangeIndex = pos;
                    }

                    if (pos == -1)
                    {
                        Debug.Fail("Item is no longer in our list but we are still getting change notifications.");
                        this.UnhookPropertyChanged(item);
                        this.ResetBindings();
                    }
                    else
                    {
                        // Get the property descriptor
                        if (null == this.itemTypeProperties)
                        {
                            // Get Shape
                            this.itemTypeProperties = TypeDescriptor.GetProperties(typeof(T));
                        }
                        PropertyDescriptor pd = this.itemTypeProperties.Find(e.PropertyName, true);

                        // Create event args.  If there was no matching property descriptor,
                        // we raise the list changed anyway.
                        ListChangedEventArgs args = new ListChangedEventArgs(ListChangedType.ItemChanged, pos, pd);

                        // Fire the ItemChanged event
                        this.OnListChanged(args);
                    }
                }
            }
        }

        #endregion

        #region IRaiseItemChangedEvents interface

        /// <summary>
        /// Gets a value indicating whether item property value changes raise ListChanged events of type ItemChanged.
        /// This member cannot be overridden in a derived class.
        /// </summary>
        /// <value>
        /// Returns false to indicate that BindingList does NOT raise ListChanged events of type ItemChanged as a result of property changes
        /// on individual list items unless those items support INotifyPropertyChanged.
        /// </value>
        bool IRaiseItemChangedEvents.RaisesItemChangedEvents => this.raiseItemChangedEvents;

        #endregion

        #region Privates

        int addNewPos = -1;
        bool raiseItemChangedEvents = false;

        [NonSerialized()]
        PropertyDescriptorCollection itemTypeProperties = null;

        [NonSerialized()]
        PropertyChangedEventHandler propertyChangedEventHandler = null;

        [NonSerialized()]
        AddingNewEventHandler onAddingNew;

        [NonSerialized()]
        int lastChangeIndex = -1;

        bool allowNew = true;
        bool allowEdit = true;
        bool allowRemove = true;
        bool userSetAllowNew = false;

        #endregion
    }
}
