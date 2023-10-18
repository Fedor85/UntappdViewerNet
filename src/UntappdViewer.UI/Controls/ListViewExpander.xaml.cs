using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UntappdViewer.UI.Helpers;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for ListViewExpander.xaml
    /// </summary>
    public partial class ListViewExpander : UserControl
    {
        private static readonly DependencyProperty ExpanderHeaderProperty = DependencyProperty.Register("ExpanderHeader", typeof(string), typeof(ListViewExpander), new PropertyMetadata(Properties.Resources.Others));

        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(ListViewExpander), new PropertyMetadataParameter<string>(UpdateResources, "ContentTemplate"));

        private static readonly DependencyProperty SeparatorTemplateProperty = DependencyProperty.Register("SeparatorTemplate", typeof(DataTemplate), typeof(ListViewExpander), new PropertyMetadataParameter<string>(UpdateResources, "SeparatorTemplate"));

        public string ExpanderHeader
        {
            get { return (string)GetValue(ExpanderHeaderProperty); }
            set { SetValue(ExpanderHeaderProperty, value); }
        }

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public DataTemplate SeparatorTemplate
        {
            get { return (DataTemplate)GetValue(SeparatorTemplateProperty); }
            set { SetValue(SeparatorTemplateProperty, value); }
        }

        public bool ScrollViewerDisable
        {
            set
            {
                if (value)
                    ScrollViewer.Height = double.NaN;
            }
        }

        public ListViewExpander()
        {
            InitializeComponent();
            Expander.SetBinding(Expander.HeaderProperty, new Binding { Path = new PropertyPath(ExpanderHeaderProperty), Source = this });
            MainView.SetBinding(ContentControl.ContentTemplateProperty, new Binding { Path = new PropertyPath(ItemDataTemplateProperty), Source = this });
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ICollection iCollection = e.NewValue as ICollection;
            if (iCollection == null || iCollection.Count == 0)
            {
                MainView.DataContext = null;
                OthersView.DataContext = null;
                Expander.Visibility = Visibility.Collapsed;
                return;
            }
            ArrayList arrayList = new ArrayList(iCollection);
            MainView.DataContext = arrayList[0];
            if (arrayList.Count > 1)
            {
                OthersView.ItemsSource = arrayList.GetRange(1, arrayList.Count - 1);
                Expander.Visibility = Visibility.Visible;
            }
            else
            {
                OthersView.DataContext = null;
                Expander.Visibility = Visibility.Collapsed;
            }
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;

            ScrollViewer parentScrollViewer = UIHelper.FindParent<ScrollViewer>(this);
            if (parentScrollViewer != null && parentScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                parentScrollViewer.ScrollToVerticalOffset(parentScrollViewer.VerticalOffset - e.Delta); ;
        }

        private static void UpdateResources(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PropertyMetadataParameter<string> propertyMetadata = e.Property.GetMetadata(d) as PropertyMetadataParameter<string>;
            ListViewExpander listControl = d as ListViewExpander;
            listControl.Resources[propertyMetadata.Parameter] = e.NewValue;
        }
    }
}