using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for SliderContentControl.xaml
    /// </summary>
    public partial class SliderContentControl : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(SliderContentControl), new PropertyMetadata(SetItemsSource));

        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(SliderContentControl));

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        private int index;

        public SliderContentControl()
        {
            InitializeComponent();

            ContentControl.SetBinding(ContentControl.ContentTemplateProperty, new Binding { Path = new PropertyPath(ItemDataTemplateProperty), Source = this });
            LabelGrid.Visibility = Visibility.Hidden;
            NavigationGrid.Visibility = Visibility.Hidden;
        }

        public double ArrowsSize
        {
            set
            {
                GridLength gridLength = new GridLength(value);
                LeftColumnArrow.Width = gridLength;
                RightColumnArrow.Width = gridLength;
                LeftArrow.Width = value;
                LeftArrow.Height = value;
                RightArrow.Width = value;
                RightArrow.Height = value;

                double labelSize = value < 20 ? 20 : value;
                RowCounterLabel.Height = new GridLength(labelSize);
                CounterLabel.FontSize = labelSize / 2.5;
            }
        }

        private void InitializeItemsSource()
        {
            index = 0;
            LabelGrid.Visibility = Visibility.Hidden;
            NavigationGrid.Visibility = Visibility.Hidden;
            ContentControl.DataContext = null;
            if (ItemsSource == null || ItemsSource.Count == 0)
                return;

            InitializeItem();
            if (ItemsSource.Count > 1)
            {
                LabelGrid.Visibility = Visibility.Visible;
                NavigationGrid.Visibility = Visibility.Visible;
            }
        }

        private void InitializeItem()
        {
            if (index > ItemsSource.Count - 1 || index < 0)
                return;

            object item = ItemsSource[index];
            ContentControl.DataContext = item;
            SetCounterLabel();
        }

        private void SetCounterLabel()
        {
            CounterLabel.Content = String.Format("{0}/{1}", index + 1, ItemsSource.Count);
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            index--;
            if (index < 0)
                index = ItemsSource.Count - 1;

            InitializeItem();
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            index++;
            if (index > ItemsSource.Count - 1)
                index = 0;

            InitializeItem();
        }

        private static void SetItemsSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((SliderContentControl)dependencyObject).InitializeItemsSource();
        }
    }
}