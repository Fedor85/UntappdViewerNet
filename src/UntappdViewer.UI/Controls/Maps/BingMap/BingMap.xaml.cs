using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LinqToXaml;
using Microsoft.Maps.MapControl.WPF;
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
        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }

        public bool IsBlockParentScroll { get; set; }

        public BingMap()
        {
            InitializeComponent();
            IsBlockParentScroll = true;

            MapControl.SetBinding(Map.CredentialsProviderProperty, new Binding { Path = new PropertyPath(CredentialsProviderProperty), Converter = new CredentialsProviderConverter(), Source = this });
            MapControl.Center = Center.GetMapLocation();
            MapControl.ZoomLevel = defaultZoomLevel;
            MapControl.LayoutUpdated += MapControlLayoutUpdated;

            MapItemsControl.SetBinding(MapItemsControl.ItemsSourceProperty, new Binding { Path = new PropertyPath(ItemsSourceProperty), Source = this });

            MouseRightButtonDown += MapBingMouseRightButtonDown;
            MouseWheel += BingMapMouseWheel;
        }

        private void MapControlLayoutUpdated(object sender, EventArgs e)
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
            BingMap mapBing = dependencyObject as BingMap;
            mapBing.MapControl.ZoomLevel = Convert.ToDouble(e.NewValue ?? 0);
        }

        private static void UpdateCenter(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            BingMap mapBing = dependencyObject as BingMap;
            mapBing.MapControl.Center = mapBing.Center.GetMapLocation();
        }

        private static void UpdateItemDataTemplate(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            BingMap mapBing = dependencyObject as BingMap;
            mapBing.Resources["DefaultItemMapTemplate"] = e.NewValue as DataTemplate;
        }
    }
}
