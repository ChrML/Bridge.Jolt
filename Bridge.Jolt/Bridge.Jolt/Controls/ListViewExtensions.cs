using Bridge.Jolt.Utilities;
using System;

namespace Bridge.Jolt.Controls
{
    /// <summary>
    /// Provides helpful extension methods related to the <see cref="ListView"/> control.
    /// </summary>
    public static class ListViewExtensions
    {
        /// <summary>
        /// Adds a new listview item to the list with the given text.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ElementCollection<ListViewItem> Add(this ElementCollection<ListViewItem> items, string text)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            items.Add(new ListViewItem(text));
            return items;
        }
    }
}
