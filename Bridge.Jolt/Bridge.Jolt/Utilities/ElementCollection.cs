using Retyped;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bridge.Jolt.Utilities
{
    /// <summary>
    /// Implements a collection of elements that map ites element items directly to a DOM- parent.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ElementCollection<T>: Collection<T>, IList<T>, IHtmlElement where T:IHtmlElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCollection{T}"/> class.
        /// </summary>
        public ElementCollection()
        {
            this.DomElement = Html.NewDiv<ElementCollection<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCollection{T}"/> class using a custom container.
        /// </summary>
        /// <param name="domContainer">The container element for hosting these elements.</param>
        public ElementCollection(dom.HTMLElement domContainer)
        {
            this.DomElement = domContainer ?? Html.NewDiv<ElementCollection<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCollection{T}"/> class using a custom container. <br/>
        /// This constructor also allows the caller to add methods that will be called whenever items go in/out of the collection.
        /// </summary>
        /// <param name="domContainer"></param>
        /// <param name="itemAdded"></param>
        /// <param name="itemRemoved"></param>
        public ElementCollection(dom.HTMLElement domContainer, Action<T, int> itemAdded, Action<T, int> itemRemoved)
        {
            this.DomElement = domContainer ?? Html.NewDiv<ElementCollection<T>>();
            this.itemAdded = itemAdded;
            this.itemRemoved = itemRemoved;
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public dom.HTMLElement DomElement { get; } = Html.NewDiv<ElementCollection<T>>();

        #endregion

        #region Methods

        /// <summary>
        /// Called after an item has been added to this collection.
        /// </summary>
        /// <param name="item">The item added to the collection.</param>
        /// <param name="index">Index that the new item was added to.</param>
        protected virtual void AfterItemGoesIn(T item, int index)
        {
            this.itemAdded?.Invoke(item, index);
        }

        /// <summary>
        /// Called after an item has been removed from this collection.
        /// </summary>
        /// <param name="item">The item removed from the collection.</param>
        /// <param name="index">Index that the item was removed from.</param>
        protected virtual void AfterItemGoesOut(T item, int index)
        {
            this.itemRemoved?.Invoke(item, index);
        }

        /// <summary>
        /// Called before an item goes into this collection.
        /// </summary>
        /// <param name="item">The item that will go into the collection.</param>
        /// <param name="index">Index that the item will go into the collection at.</param>
        protected virtual void BeforeItemGoesIn(T item, int index)
        {
        }

        /// <summary>
        /// Called before an item will be removed from this collection.
        /// </summary>
        /// <param name="item">The item that will be removed from the collection.</param>
        /// <param name="index">Index of the item that will be removed.</param>
        protected virtual void BeforeItemGoesOut(T item, int index)
        {
        }

        /// <summary>
        /// Clears the elements from the DOM when the base class clears its collection.
        /// </summary>
        protected override void ClearItems()
        {
            // Before removal.
            T[] copy = this.ToArray();
            int length = copy.Length;
            for (int i = 0; i < length; i++)
            {
                this.BeforeItemGoesOut(copy[i], i);
            }

            // Clear the DOM / list.
            base.ClearItems();
            this.DomElement.RemoveChildren();

            // After removal.
            for (int i = 0; i < length; i++)
            {
                this.AfterItemGoesOut(copy[i], i);
            }
        }

        /// <summary>
        /// Insert an item to the DOM when it's added to the collection at an index.
        /// </summary>
        /// <param name="index">Index the item will be inserted to.</param>
        /// <param name="item">The item that will be inserted to the collection.</param>
        protected override void InsertItem(int index, T item)
        {
            this.BeforeItemGoesIn(item, index);
            base.InsertItem(index, item);

            this.DomElement.Insert(index, item.DomElement);
            this.AfterItemGoesIn(item, index);
        }

        /// <summary>
        /// Removes an item from the DOM when it's removed from the collection.
        /// </summary>
        /// <param name="index">The index the item will be removed from.</param>
        protected override void RemoveItem(int index)
        {
            T oldItem = this[index];
            this.BeforeItemGoesOut(oldItem, index);
            base.RemoveItem(index);

            oldItem.DomElement.Remove();
            this.AfterItemGoesOut(oldItem, index);
        }

        /// <summary>
        /// Sets the item at the given index to a new element.
        /// </summary>
        /// <param name="index">The index the item will be set at.</param>
        /// <param name="item">The item that will be set.</param>
        protected override void SetItem(int index, T item)
        {
            // Skip if indifferent.
            T old = this[index];
            if (ReferenceEquals(item, old))
            {
                return;
            }

            // Before insert/remove.
            this.BeforeItemGoesOut(old, index);
            this.BeforeItemGoesIn(item, index);

            // Actual replacement.
            base.SetItem(index, item);
            this.DomElement.ReplaceChild(old.DomElement, item.DomElement);

            // After insert/remove.
            this.AfterItemGoesOut(old, index);
            this.AfterItemGoesIn(item, index);
        }

        #endregion

        #region Privates

        readonly Action<T, int> itemAdded;
        readonly Action<T, int> itemRemoved;

        #endregion
    }
}
