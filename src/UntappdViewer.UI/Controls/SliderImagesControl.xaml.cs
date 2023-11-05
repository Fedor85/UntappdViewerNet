using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using UntappdViewer.UI.Helpers;
using UntappdViewer.Utils;

namespace UntappdViewer.UI.Controls
{
    /// <summary>
    /// Interaction logic for SliderImagesControl.xaml
    /// </summary>
    public partial class SliderImagesControl : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(SliderImagesControl), new PropertyMetadata(SetItemsSource));

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private int index;

        public SliderImagesControl()
        {
            InitializeComponent();
            ToolTipService.SetShowDuration(Image, Int32.MaxValue);
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
            Image.Source = null;
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

            PropertyInfo imagePathPropertyInfo = item.GetType().GetProperty("ImagePath");
            if (imagePathPropertyInfo != null)
            {
                string imagePath = imagePathPropertyInfo.GetValue(item, null).ToString();
                if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    Image.Source = ImageConverter.GetBitmapSource(imagePath);
            }

            PropertyInfo imageDescriptionPropertyInfo = item.GetType().GetProperty("ImageDescription");
            if (imageDescriptionPropertyInfo != null)
            {
                string imageDescription = imageDescriptionPropertyInfo.GetValue(item, null).ToString();
                Image.ToolTip = StringHelper.GetSplitByLength(imageDescription, 50);
                ToolTipService.SetIsEnabled(Image, !String.IsNullOrEmpty(imageDescription));
            }

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
            ((SliderImagesControl)dependencyObject).InitializeItemsSource();
        }
    }
}