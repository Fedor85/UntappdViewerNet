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

        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(ListViewExpander));

        private static readonly DependencyProperty SeparatorTemplateProperty = DependencyProperty.Register("SeparatorTemplate", typeof(DataTemplate), typeof(ListViewExpander));

        private static readonly DependencyProperty OthersItemCountProperty = DependencyProperty.Register("OthersItemCount", typeof(int), typeof(ListViewExpander));

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

        public int OthersItemCount
        {
            get { return (int)GetValue(OthersItemCountProperty); }
            private set { SetValue(OthersItemCountProperty, value); }
        }

        public ListViewExpander()
        {
            InitializeComponent();
            Expander.SetBinding(Expander.HeaderProperty, new Binding { Path = new PropertyPath(ExpanderHeaderProperty), Source = this });
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ICollection iCollection = e.NewValue as ICollection;
            if (iCollection == null || iCollection.Count == 0)
            {
                MainView.DataContext = null;
                ClearOtherItems();
                return;
            }

            ArrayList arrayList = new ArrayList(iCollection);
            int count = arrayList.Count;
            MainView.DataContext = arrayList[0];
            if (count > 1)
            {
                int otherCount = count - 1;
                OthersView.ItemsSource = arrayList.GetRange(1, otherCount);
                OthersItemCount = otherCount;
                Expander.Visibility = Visibility.Visible;
            }
            else
            {
                ClearOtherItems();
            }
        }

        private void ClearOtherItems()
        {
            OthersView.DataContext = null;
            OthersItemCount = 0;
            Expander.Visibility = Visibility.Collapsed;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0)
                return;

            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (!double.IsNaN(scrollViewer.Height))
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                e.Handled = true;
            }

            ScrollViewer parentScrollViewer = UIHelper.FindParent<ScrollViewer>(this);
            if (parentScrollViewer != null)
            {
                if (parentScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                    parentScrollViewer.ScrollToVerticalOffset(parentScrollViewer.VerticalOffset - e.Delta);
            }
            else
            {
                FrameworkElement frameworkElement = UIHelper.FindParent<FrameworkElement>(this);
                if (frameworkElement != null)
                {
                    MouseWheelEventArgs mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                    mouseWheelEventArgs.RoutedEvent = UIElement.MouseWheelEvent;
                    frameworkElement.RaiseEvent(mouseWheelEventArgs);
                }
            }
        }
    }
}