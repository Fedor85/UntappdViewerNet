using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using WpfToolkit.Controls;

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
            //anything that’s only used in XAML and not in code behind, isn't deemed to be 'in use' by MSBuild.
            VirtualizingWrapPanel useByMSBuild = new VirtualizingWrapPanel();

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