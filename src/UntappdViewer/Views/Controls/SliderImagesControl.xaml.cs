using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace UntappdViewer.Views.Controls
{
    /// <summary>
    /// Interaction logic for SliderImagesControl.xaml
    /// </summary>
    public partial class SliderImagesControl : UserControl
    {
        public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(SliderImagesControl), new FrameworkPropertyMetadata(null, SetItemsSource));

        private static object SetItemsSource(DependencyObject dependencyObject, object items)
        {
            if (items != null)
                ((SliderImagesControl)dependencyObject).ItemsSource = (IList)items;

            return items;

        }

        private IList items;

        private int index;

        public SliderImagesControl()
        {
            InitializeComponent();
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

        public IList ItemsSource
        {
            set
            {
                items = value;
                InitializeItemsSource();
            }
        }

        private void InitializeItemsSource()
        {
            index = 0;
            LabelGrid.Visibility = Visibility.Hidden;
            NavigationGrid.Visibility = Visibility.Hidden;

            if (items.Count == 0)
                return;

            InitializeItem();
            if (items.Count > 1)
            {
                LabelGrid.Visibility = Visibility.Visible;
                NavigationGrid.Visibility = Visibility.Visible;
            }
        }

        private void InitializeItem()
        {
            if (index > items.Count - 1 || index < 0)
                return;

            object item = items[index];

            PropertyInfo imagePathPropertyInfo = item.GetType().GetProperty("ImagePath");
            if (imagePathPropertyInfo != null)
            {
                string imagePath = imagePathPropertyInfo.GetValue(item, null).ToString();
                if (!String.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    using (Bitmap image = new Bitmap(imagePath))
                        Image.Source = Helpers.ImageConverter.GetBitmapSource(image);
                }
            }

            PropertyInfo imageDescriptionPropertyInfo = item.GetType().GetProperty("ImageDescription");
            if (imageDescriptionPropertyInfo != null)
            {
                string imageDescription = imageDescriptionPropertyInfo.GetValue(item, null).ToString();
                Image.ToolTip = imageDescription;
                ToolTipService.SetIsEnabled(Image, !String.IsNullOrEmpty(imageDescription));
            }

            SetCounterLabel();
        }

        private void SetCounterLabel()
        {
            CounterLabel.Content = String.Format("{0}/{1}", index + 1, items.Count);
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            index--;
            if (index < 0)
                index = items.Count - 1;

            InitializeItem();
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            index++;
            if (index > items.Count - 1)
                index = 0;

            InitializeItem();
        }
    }
}
