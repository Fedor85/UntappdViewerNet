using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using UntappdViewer.UI.Controls.Maps.BingMap.ViewModel;
using UntappdViewer.UI.Helpers;
using UntappdViewer.Utils;
using LocationVM = UntappdViewer.UI.Controls.Maps.BingMap.ViewModel.Location;

namespace UntappdViewer.UI.Controls.Maps.GMapNet
{
    /// <summary>
    /// Interaction logic for GMapNet.xaml
    /// </summary>
    public partial class GMapNet : UserControl
    {
        private const double defaultZoomLevel = 2;

        private const int minZoom = 1;

        private static readonly LocationVM defaultCenter = new LocationVM(0, 0);

        private static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(GMapNet), new PropertyMetadata(defaultZoomLevel, UpdateZoomLevel));

        private static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(LocationVM), typeof(GMapNet), new PropertyMetadata(defaultCenter, UpdateCenter));

        private static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GMapNet), new PropertyMetadata(UpdateItemsSource));

        private static readonly DependencyProperty MainLocationItemProperty = DependencyProperty.Register("MainLocationItem", typeof(LocationItem), typeof(GMapNet), new PropertyMetadata(SetMainLocationItem));

        public static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(GMapNet), new PropertyMetadata(UpdateItemDataTemplate));

        private static readonly DependencyProperty EmptyMapBackgroundProperty = DependencyProperty.Register("EmptyMapBackground", typeof(Brush), typeof(GMapNet), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), SetEmptyMapBackground));

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

        public Brush EmptyMapBackground
        {
            get { return (Brush)GetValue(EmptyMapBackgroundProperty); }
            set { SetValue(EmptyMapBackgroundProperty, value); }
        }
        public LocationVM Test { get; set; }

        public bool IsBlockParentScroll { get; set; }

        public bool IsVisibleLogoAndSing { get; set; }

        public GMapNet()
        {
            InitializeComponent();

            IsBlockParentScroll = true;
            IsVisibleLogoAndSing = true;

            MapControl.ScaleMode = ScaleModes.Dynamic;
            MapControl.MinZoom = minZoom;
            MapControl.MaxZoom = 20;
            MapControl.EmptyMapBackground = EmptyMapBackground;

            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            MapControl.Position = Center.GetGMapPosition();
            MapControl.ShowCenter = false;

            MapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            MapControl.IgnoreMarkerOnMouseWheel = true;

            MapControl.CanDragMap = true;
            MapControl.DragButton = MouseButton.Left;
            MapControl.Loaded += MapControlLoaded;
            MapControl.SizeChanged += MapControlSizeChanged;
            MouseRightButtonDown += GMapNetMouseRightButtonDown;
            MouseWheel += GMapNetMouseWheel;
        }

        private void MapControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetZoom(MapControl.Zoom);
        }

        private void MapControlLoaded(object sender, RoutedEventArgs e)
        {
            SetMapProvider();

            Canvas mapCanvas = GetMapCanvas();
            if (mapCanvas != null)
                UpdateOffset(mapCanvas);
        }

        private void SetMapProvider()
        {
            GMapProvider googleMapProvider = CloneHelper.Clone(GMapProviders.GoogleMap);
            if (!IsVisibleLogoAndSing)
                googleMapProvider.Copyright = String.Empty;

            MapControl.MapProvider = googleMapProvider;
        }

        private void GMapNetMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MapControl.Position = Center.GetGMapPosition();
            SetZoom(ZoomLevel);
        }

        private void SetZoom(double zoom)
        {
            if (ZoomLevel % 1 > 0)
                MapControl.Zoom = minZoom;

            MapControl.Zoom = zoom;
        }

        private void GMapNetMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsBlockParentScroll)
                e.Handled = true;
        }

        private Canvas GetMapCanvas()
        {
            PropertyInfo propertyInfoMapCanvas = MapControl.GetType().GetProperty("MapCanvas", BindingFlags.Instance | BindingFlags.NonPublic);
            return propertyInfoMapCanvas.GetValue(MapControl) as Canvas;
        }

        private void UpdateLocalPositions()
        {
            MethodInfo forceUpdateOverlaysMethod = MapControl.GetType().GetMethod("ForceUpdateOverlays", BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            forceUpdateOverlaysMethod.Invoke(MapControl, null);
        }

        private void UpdateOffset(Canvas mapCanvas)
        {
            List<MapMarker>  mapMarkers = FillContentPresenter(mapCanvas);
            foreach (MapMarker mapMarker in mapMarkers.Where(item => item.IsContentPresenterLoaded()))
                mapMarker.SetOffset();
        }

        private List<MapMarker> FillContentPresenter(Canvas mapCanvas)
        {
            List<MapMarker> mapMarkers = new List<MapMarker>();
            IEnumerable<ContentPresenter> contentPresenters = mapCanvas.Children.OfType<ContentPresenter>();
            foreach (ContentPresenter contentPresenter in contentPresenters)
            {
                MapMarker mapMarker = contentPresenter.Content as MapMarker;
                if(mapMarker.ContentPresenter != null)
                    continue;

                mapMarker.ContentPresenter = contentPresenter;
                mapMarkers.Add(mapMarker);
            }

            return mapMarkers;
        }

        private static void UpdateZoomLevel(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GMapNet gMapNet = dependencyObject as GMapNet;
            gMapNet.MapControl.Zoom = Convert.ToDouble(e.NewValue ?? 0);
        }

        private static void UpdateCenter(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GMapNet gMapNet = dependencyObject as GMapNet;
            gMapNet.MapControl.Position = gMapNet.Center.GetGMapPosition();
        }

        private static void UpdateItemsSource(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GMapNet gMapNet = dependencyObject as GMapNet;

            List<LocationItem> items = e.NewValue as List<LocationItem>;
            if (items == null || items.Count == 0)
            {
                gMapNet.MapControl.ItemsSource = Enumerable.Empty<GMapMarker>();
                return;
            }

            IEnumerable<GMapMarker> mapMarkers = items.ConvertAll(item => item.GetMapMarker());
            gMapNet.MapControl.ItemsSource = mapMarkers;

            Canvas mapCanvas = gMapNet.GetMapCanvas();
            if (mapCanvas != null)
            {
                gMapNet.UpdateOffset(mapCanvas);
                gMapNet.UpdateLocalPositions();          
            }
        }

        private static void SetMainLocationItem(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            LocationItem locationItem = e.NewValue as LocationItem;
            if (locationItem == null)
                return;

            GMapNet gMapNet = dependencyObject as GMapNet;
            gMapNet.Center = locationItem.Location;
            gMapNet.ItemsSource = new List<LocationItem> { locationItem };
        }

        private static void UpdateItemDataTemplate(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GMapNet gMapNet = dependencyObject as GMapNet;
            gMapNet.Resources["DefaultItemMapTemplate"] = e.NewValue as DataTemplate;
        }

        private static void SetEmptyMapBackground(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GMapNet gMapNet = dependencyObject as GMapNet;
            gMapNet.MapControl.EmptyMapBackground = e.NewValue as Brush;
        }
    }
}