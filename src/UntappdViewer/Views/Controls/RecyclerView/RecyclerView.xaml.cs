using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;

namespace UntappdViewer.Views.Controls
{ 
    /// <summary>
    /// Interaction logic for RecyclerView.xaml
    /// </summary>
    public partial class RecyclerView : ListBox
    {
        public event Action<IEnumerable> ItemsSourceChanged;

        public RecyclerView()
        {
            InitializeComponent();
            TypeDescriptor.GetProperties(this)["ItemsSource"].AddValueChanged(this, ListViewItemsSourceChanged);

        }

        private void ListViewItemsSourceChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            ItemsSourceChanged?.Invoke(listBox.ItemsSource);
        }
    }
}