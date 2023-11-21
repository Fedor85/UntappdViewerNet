using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LinqToXaml;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Overlays;
using UntappdViewer.UI.Controls.Maps.BingMap.ViewModel;
using UntappdViewer.UI.Helpers;
using UntappdViewer.UI.IValueConverters;
using LocationVM = UntappdViewer.UI.Controls.Maps.BingMap.ViewModel.Location;

namespace UntappdViewer.UI.Controls.Maps.BingMap
{
    /// <summary>
    /// Interaction logic for BingMap.xaml
    /// </summary>
    public partial class BingMap : UserControl
    {
        private const double defaultZoomLevel = 2;

        private static readonly LocationVM defaultCenter = new LocationVM(40.13618, -0.45822);

        private static readonly DependencyProperty CredentialsProviderProperty = DependencyProperty.Register("CredentialsProvider", typeof(string), typeof(BingMap));

        private static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(BingMap), new PropertyMetadata(defaultZoomLevel, UpdateZoomLevel));

        private static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(LocationVM), typeof(BingMap), new PropertyMetadata(defaultCenter, UpdateCenter));

        private static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BingMap));

        private static readonly DependencyProperty MainLocationItemProperty = DependencyProperty.Register("MainLocationItem", typeof(LocationItem), typeof(BingMap), new PropertyMetadata(SetMainLocationItem));

        private static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(BingMap), new PropertyMetadata(UpdateItemDataTemplate));

        public string CredentialsProvider
        {
            get { return (string)GetValue(CredentialsProviderProperty); }
            set { SetValue(CredentialsProviderProperty, value); }
        }

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public LocationVM Center
        {
            get { return (LocationVM)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public LocationItem MainLocationItem
        {
            get { return (LocationItem)GetValue(MainLocationItemProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public bool IsBlockParentScroll { get; set; }

        public bool IsVisibleLogoAndSing { get; set; }

        public bool IsVisibleScale { get; set; }

        public BingMap()
        {
            InitializeComponent();
            IsBlockParentScroll = true;
            IsVisibleLogoAndSing = true;
            IsVisibleScale = true;

            MapControl.SetBinding(Map.CredentialsProviderProperty, new Binding { Path = new PropertyPath(CredentialsProviderProperty), Converter = new CredentialsProviderConverter(), Source = this });
            MapControl.Center = Center.GetMapLocation();
            MapControl.ZoomLevel = defaultZoomLevel;
            MapControl.Loaded += MapControlLoaded;
            MapControl.LayoutUpdated += MapControlLayoutUpdated;

            MapItemsControl.SetBinding(MapItemsControl.ItemsSourceProperty, new Binding { Path = new PropertyPath(ItemsSourceProperty), Source = this });

            MouseRightButtonDown += MapBingMouseRightButtonDown;
            MouseWheel += BingMapMouseWheel;
        }

        private void MapControlLoaded(object sender, RoutedEventArgs e)
        {
            if (!IsVisibleLogoAndSing)
                HideLogo();

            if(!IsVisibleScale)
                HideScale();
        }

        private void MapControlLayoutUpdated(object sender, EventArgs e)
        {
            if (!IsVisibleLogoAndSing)
                HideCopyrightSing();

            UpdateErrorControl();
        }

        private void HideLogo()
        {
            IEnumerable<Image> images = MapControl.DescendantsAndSelf().OfType<Image>();
            if (!images.Any())
                return;

            PropertyInfo baseUriProperty = typeof(Image).GetProperty("BaseUri", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (Image image in images)
            {
                string baseUri = baseUriProperty.GetValue(image)?.ToString().ToLower();
                if (!String.IsNullOrEmpty(baseUri) && baseUri.Contains("logo"))
                    image.Visibility = Visibility.Collapsed;
            }
        }

        private void HideCopyrightSing()
        {
            IEnumerable<TextBlock> copyrightTextBlocks = MapControl.DescendantsAndSelf().OfType<TextBlock>().Where(d => d.Text.ToLower().Contains("©"));
            foreach (TextBlock textBlock in copyrightTextBlocks)
                textBlock.Visibility = Visibility.Hidden;
        }

        private void HideScale()
        {
            IEnumerable<Scale> scales = MapControl.DescendantsAndSelf().OfType<Scale>();
            foreach (Scale scale in scales)
                scale.Visibility = Visibility.Hidden;
        }

        private void UpdateErrorControl()
        {
            FrameworkElement frameworkElement = MapControl.DescendantsAndSelf().OfType<FrameworkElement>().SingleOrDefault(d => d.Name.ToLower().Contains("errormessage"));
            if (frameworkElement != null)
            {
                TextBlock textBlock = frameworkElement as TextBlock;
                if (textBlock != null)
                    ErrorMessageControl.Text = textBlock.Text;

                ErrorControl.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessageControl.Text = String.Empty;
                ErrorControl.Visibility = Visibility.Hidden;
            }
        }

        private void BingMapMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (IsBlockParentScroll)
                e.Handled = true;
        }

        private void MapBingMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MapControl.Center = Center.GetMapLocation();
            MapControl.ZoomLevel = ZoomLevel;
        }

        private static void UpdateZoomLevel(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            BingMap bingMap = dependencyObject as BingMap;
            bingMap.MapControl.ZoomLevel = Convert.ToDouble(e.NewValue ?? 0);
        }

        private static void UpdateCenter(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            BingMap bingMap = dependencyObject as BingMap;
            bingMap.MapControl.Center = bingMap.Center.GetMapLocation();
        }
        private static void SetMainLocationItem(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            LocationItem locationItem = e.NewValue as LocationItem;
            if (locationItem == null)
                return;
            ;
            BingMap bingMap = dependencyObject as BingMap;
            bingMap.Center = locationItem.Location;
            bingMap.ItemsSource = new List<LocationItem> { locationItem };
        }

        private static void UpdateItemDataTemplate(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            BingMap bingMap = dependencyObject as BingMap;
            bingMap.Resources["DefaultItemMapTemplate"] = e.NewValue as DataTemplate;
        }
    }
}
