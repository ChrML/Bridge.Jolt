namespace System.ComponentModel
{
    /// <summary>
    /// Adds transactional capability when adding a new item to a collection.
    /// </summary>
    public interface ICancelAddNew
    {
        /// <summary>
        /// Discards a pending new item from the collection.
        /// </summary>
        /// <param name="itemIndex">The index of the item that was previously added to the collection.</param>
        /// <remarks>
        /// The <see cref="CancelNew"/> method rolls back a pending addition (AddNew) of an item previously added to the collection at position <paramref name="itemIndex"/>.
        /// The index parameter is necessary because several new items can be simultaneously pending.
        /// </remarks>
        void CancelNew(int itemIndex);

        /// <summary>
        /// Commits a pending new item to the collection.
        /// </summary>
        /// <param name="itemIndex">The index of the item that was previously added to the collection.</param>
        /// <remarks>
        /// The <see cref="EndNew"/> commits a pending addition (AddNew) of an item previously added to the collection at position <paramref name="itemIndex"/>.
        /// The index parameter is necessary because several new items can be simultaneously pending.
        /// </remarks>
        void EndNew(int itemIndex);
    }
}
