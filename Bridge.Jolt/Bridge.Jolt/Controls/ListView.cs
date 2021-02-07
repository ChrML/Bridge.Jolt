using Jolt.Abstractions;
using Jolt.Services;
using Jolt.Utilities;
using Retyped;
using System;

namespace Jolt.Controls
{
    /// <summary>
    /// Implements a control that can render a list of controls.
    /// </summary>
    public class ListView: HtmlControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListView"/> class.
        /// </summary>
        public ListView()
        {
            this.Items = new ElementCollection<ListViewItem>
            (
                domContainer: this.domContainer,

                itemAdded: (item, i) =>
                {
                    item.Click += this.Item_Click;
                    this.UpdateEmptyLabel();
                },

                itemRemoved: (item, i) =>
                {
                    item.Click -= this.Item_Click;
                    this.UpdateEmptyLabel();
                }
            );

            this.DomElement.Append(this.emptyLabel);
            this.DomElement.Append(this.domContainer);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text that will be displayed if the list is empty.
        /// </summary>
        public string EmptyPlaceholder
        {
            get => this.emptyLabel.Text;
            set
            {
                this.emptyLabel.Text = value;
                this.UpdateEmptyLabel();
            }
        }

        /// <summary>
        /// Gets the collection of items displayed in this listview.
        /// </summary>
        public ElementCollection<ListViewItem> Items { get; } 

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an item in the list has been clicked by the user.
        /// </summary>
        public event EventHandler<ListViewItemEventArgs> ItemClick;

        /// <summary>
        /// Raises the <see cref="ItemClick"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemClick(ListViewItemEventArgs e) => this.ItemClick?.Invoke(this, e);

        #endregion

        #region Privates

        /// <summary>
        /// Called when an item in this list has been clicked by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Item_Click(object sender, MouseEventArgs e)
        {
            ListViewItemEventArgs args = new ListViewItemEventArgs((ListViewItem)sender, e);
            this.OnItemClick(args);
        }


        /// <summary>
        /// Checks if collection is empty to eventually display the empty label.
        /// </summary>
        void UpdateEmptyLabel()
        {
            this.emptyLabel.Visible = this.Items.Count == 0 && !String.IsNullOrEmpty(this.EmptyPlaceholder);
        }


        readonly dom.HTMLDivElement domContainer = Html.NewDiv<ListView>("Container");
        readonly Label emptyLabel = new Label
        {
            CssClass = Css.GetClass<ListView>("EmptyLabel"),
            Text = Service.Resolve<IJoltLocale>().NothingToShow,
            Visible = false
        };

        #endregion

    }
}
