using System.Collections;

namespace System.ComponentModel
{
    /// <summary>
    /// Provides the features required to support both complex and simple scenarios when binding to a data source.
    /// </summary>
    public interface IBindingList : ICollection, IEnumerable, IList
    {
        /// <summary>
        /// Gets whether you can update items in the list.
        /// </summary>
        /// <value><see langword="true"/> if you can update the items in the list; otherwise, <see langword="¨false"/>.</value>
        bool AllowEdit { get; }

        /// <summary>
        /// Gets whether you can add items to the list using <see cref="AddNew"/>.
        /// </summary>
        /// <value><see langword="true"/> if you can add items to the list using <see cref="AddNew"/> otherwise, <see langword="false"/>.</value>
        bool AllowNew { get; }

        /// <summary>
        /// Gets whether you can remove items from the list, using <see cref="IList.Remove(Object)"/> or <see cref="IList.RemoveAt(Int32)"/>.
        /// </summary>
        /// <value><see langword="true"/> if you can remove items from the list; otherwise, <see langword="¨false"/>.</value>
        bool AllowRemove { get; }

        /// <summary>
        /// Gets whether the items in the list are sorted.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="ApplySort(PropertyDescriptor, ListSortDirection)"/> has been called and <see cref="RemoveSort"/> has not been called; otherwise, <see langword="false"/>.</value>
        bool IsSorted { get; }

        /// <summary>
        /// Gets the direction of the sort.
        /// </summary>
        /// <value>One of the <see cref="ListSortDirection"/> values.</value>
        /// <exception cref="NotSupportedException"><see cref="SupportsSorting"/> is <see langword="false"/>.</exception>
        ListSortDirection SortDirection { get; }

        /// <summary>
        /// Gets the <see cref="PropertyDescriptor"/> that is being used for sorting.
        /// </summary>
        /// <value>The <see cref="PropertyDescriptor"/> that is being used for sorting.</value>
        /// <exception cref="NotSupportedException"><see cref="SupportsSorting"/> is <see langword="false"/>.</exception>
        PropertyDescriptor SortProperty { get; }

        /// <summary>
        /// Gets whether a <see cref="ListChanged"/> event is raised when the list changes or an item in the list changes.
        /// </summary>
        /// <value><see langword="true"/> if a <see cref="ListChanged"/> event is raised when the list changes or when an item changes; otherwise, <see langword="false"/>.</value>
        /// <remarks>Objects in the list must notify the list when they change, so the list can raise a <see cref="ListChanged"/> event.</remarks>
        bool SupportsChangeNotification { get; }

        /// <summary>
        /// Gets whether the list supports searching using the <see cref="Find(PropertyDescriptor, Object)"/> method.
        /// </summary>
        bool SupportsSearching { get; }

        /// <summary>
        /// Gets whether the list supports sorting.
        /// </summary>
        bool SupportsSorting { get; }

        #region Events

        /// <summary>
        /// Occurs when the list changes or an item in the list changes.
        /// </summary>
        /// <remarks>This event is raised only if the <see cref="SupportsChangeNotification"/> property is true.</remarks>
        event ListChangedEventHandler ListChanged;

        #endregion

        /// <summary>
        /// Adds the <see cref="PropertyDescriptor"/> to the indexes used for searching.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> to add to the indexes used for searching.</param>
        /// <remarks>The list must support this method. However, support for this method can be a nonoperation.</remarks>
        void AddIndex(System.ComponentModel.PropertyDescriptor property);

        /// <summary>
        /// Adds a new item to the list.
        /// </summary>
        /// <returns>The item added to the list.</returns>
        /// <exception cref="NotSupportedException"><see cref="AllowNew"/> is <see langword="false"/>.</exception>
        object AddNew();

        /// <summary>
        /// Sorts the list based on a <see cref="PropertyDescriptor"/> and a <see cref="ListSortDirection"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> to sort by.</param>
        /// <param name="direction">One of the <see cref="ListSortDirection"/> values.</param>
        /// <exception cref="NotSupportedException">SupportsSorting is <see langword="false"/>.</exception>
        void ApplySort(PropertyDescriptor property, ListSortDirection direction);

        /// <summary>
        /// Returns the index of the row that has the given <see cref="PropertyDescriptor"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyDescriptor"/> to search on.</param>
        /// <param name="key">The value of the <paramref name="property"/> parameter to search for.</param>
        /// <returns>The index of the row that has the given <see cref="PropertyDescriptor"/>.</returns>
        /// <exception cref="NotSupportedException"><see cref="SupportsSearching"/> is <see langword="false"/>.</exception>
        int Find(PropertyDescriptor property, object key);

        /// <summary>
        /// Removes the <see cref="PropertyDescriptor"/> from the indexes used for searching.
        /// </summary>
        /// <param name="property"></param>
        void RemoveIndex(PropertyDescriptor property);

        /// <summary>
        /// Removes any sort applied using <see cref="ApplySort(PropertyDescriptor, ListSortDirection)"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">SupportsSorting is <see langword="false"/>.</exception>
        void RemoveSort();
    }
}
