namespace System.ComponentModel
{
    /// <summary>
    /// Specifies how the list changed.
    /// </summary>
    public enum ListChangedType
    {
        /// <summary>
        /// Much of the list has changed. Any listening controls should refresh all their data from the list.
        /// </summary>
        Reset = 0,

        /// <summary>
        /// An item added to the list. NewIndex contains the index of the item that was added.
        /// </summary>
        ItemAdded = 1,

        /// <summary>
        /// An item deleted from the list. NewIndex contains the index of the item that was deleted.
        /// </summary>
        ItemDeleted = 2,

        /// <summary>
        /// An item moved within the list. OldIndex contains the previous index for the item, whereas NewIndex contains the new index for the item.
        /// </summary>
        ItemMoved = 3,

        /// <summary>
        /// An item changed in the list. NewIndex contains the index of the item that was changed.
        /// </summary>
        ItemChanged = 4,

        /// <summary>
        /// A PropertyDescriptor was added, which changed the schema.
        /// </summary>
        PropertyDescriptorAdded = 5,

        /// <summary>
        /// A PropertyDescriptor was changed, which changed the schema.
        /// </summary>
        PropertyDescriptorDeleted = 6,

        /// <summary>
        /// A PropertyDescriptor was deleted, which changed the schema.
        /// </summary>
        PropertyDescriptorChanged = 7,
    }
}
