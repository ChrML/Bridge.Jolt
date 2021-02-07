namespace System.ComponentModel
{
    /// <summary>
    /// Provides data for the <see cref="IBindingList.ListChanged"/> event.
    /// </summary>
    public class ListChangedEventArgs : EventArgs
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class given the type of change and the <see cref="PropertyDescriptor"/> affected.
        /// </summary>
        /// <param name="listChangedType">A <see cref="ListChangedType"/> value indicating the type of change.</param>
        /// <param name="propDesc">The <see cref="PropertyDescriptor"/> that was added, removed, or changed.</param>
        /// <remarks>
        /// This constructor can be used only if the schema of the object has changed.
        /// The listChangedType parameter should be <see cref="ListChangedType.PropertyDescriptorAdded"/>, <see cref="ListChangedType.PropertyDescriptorChanged"/> or <see cref="ListChangedType.PropertyDescriptorDeleted"/>.
        /// </remarks>
        public ListChangedEventArgs(ListChangedType listChangedType, PropertyDescriptor propDesc)
        {
            // Check arguments.
            if (listChangedType != ListChangedType.PropertyDescriptorAdded &&
                listChangedType != ListChangedType.PropertyDescriptorChanged &&
                listChangedType != ListChangedType.PropertyDescriptorDeleted)
            {
                throw new ArgumentException($"Argument must be value {nameof(ListChangedType.PropertyDescriptorAdded)}, {nameof(ListChangedType.PropertyDescriptorChanged)} or {nameof(ListChangedType.PropertyDescriptorDeleted)}.", nameof(listChangedType));
            }

            // Set arguments.
            this.PropertyDescriptor = propDesc;
            this.ListChangedType = listChangedType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class given the type of change and the index of the affected item.
        /// </summary>
        /// <param name="listChangedType">A <see cref="ListChangedType"/> value indicating the type of change.</param>
        /// <param name="newIndex">The index of the item that was added, changed, or removed.</param>
        /// <remarks>Use this constructor if only one item is affected by a change. In this case, the <see cref="OldIndex"/> property is set to -1.</remarks>
        public ListChangedEventArgs(ListChangedType listChangedType, int newIndex)
        {
            this.ListChangedType = listChangedType;
            this.NewIndex = newIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class given the type of change, the index of the affected item, and a <see cref="PropertyDescriptor"/> describing the affected item.
        /// </summary>
        /// <param name="listChangedType">A <see cref="ListChangedType"/> value indicating the type of change.</param>
        /// <param name="newIndex">The index of the item that was added or changed.</param>
        /// <param name="propDesc">The <see cref="PropertyDescriptor"/> describing the item.</param>
        public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc)
        {
            this.PropertyDescriptor = propDesc;
            this.ListChangedType = listChangedType;
            this.NewIndex = newIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgs"/> class given the type of change and the old and new index of the item that was moved.
        /// </summary>
        /// <param name="listChangedType">A <see cref="ListChangedType"/> value indicating the type of change.</param>
        /// <param name="newIndex">The new index of the item that was moved.</param>
        /// <param name="oldIndex">The old index of the item that was moved.</param>
        public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex)
        {
            this.ListChangedType = listChangedType;
            this.NewIndex = newIndex;
            this.OldIndex = oldIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of change.
        /// </summary>
        /// <value>A <see cref="ListChangedType"/> value indicating the type of change.</value>
        public ListChangedType ListChangedType { get; }

        /// <summary>
        /// Gets the index of the item affected by the change.
        /// </summary>
        /// <value>The index of the affected by the change.</value>
        public int NewIndex { get; } = -1;

        /// <summary>
        /// Gets the old index of an item that has been moved.
        /// </summary>
        /// <value>The old index of the moved item.</value>
        public int OldIndex { get; } = -1;

        /// <summary>
        /// Gets the <see cref="PropertyDescriptor"/> that was added, changed, or deleted.
        /// </summary>
        /// <value>The <see cref="PropertyDescriptor"/> affected by the change.</value>
        public PropertyDescriptor PropertyDescriptor { get; }

        #endregion
    }
}
