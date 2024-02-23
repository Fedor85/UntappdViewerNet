using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GMap.NET;
using GMap.NET.MapProviders;
using UntappdViewer.UI.Controls.Maps.BingMap.ViewModel;
using UntappdViewer.UI.Helpers;
using LocationVM = UntappdViewer.UI.Controls.Maps.BingMap.ViewModel.Location;

namespace UntappdViewer.UI.Controls.Maps.GMapNet
{
    /// <summary>
    /// Interaction logic for GMapNet.xaml
    /// </summary>
    public partial class GMapNet : UserControl
    {
        private const double defaultZoomLevel = 2;

        private static readonly LocationVM defaultCenter = new LocationVM(40.13618, -0.45822);

        private static readonly DependencyProperty CredentialsProviderProperty = DependencyProperty.Register("CredentialsProvider", typeof(string), typeof(GMapNet));

        private static readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(GMapNet), new PropertyMetadata(defaultZoomLevel, UpdateZoomLevel));

        private static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(LocationVM), typeof(GMapNet), new PropertyMetadata(defaultCenter, UpdateCenter));

        private static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GMapNet));

        private static readonly DependencyProperty MainLocationItemProperty = DependencyProperty.Register("MainLocationItem", typeof(LocationItem), typeof(GMapNet), new PropertyMetadata(SetMainLocationItem));

        public static readonly DependencyProperty ItemDataTemplateProperty = DependencyProperty.Register("ItemDataTemplate", typeof(DataTemplate), typeof(GMapNet), new PropertyMetadata(UpdateItemDataTemplate));

        private static readonly DependencyProperty ScaleVisibilityProperty = DependencyProperty.Register("ScaleVisibility", typeof(Visibility), typeof(GMapNet));

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

        public Visibility ScaleVisibility
        {
            get { return (Visibility)GetValue(ScaleVisibilityProperty); }
            set { SetValue(ScaleVisibilityProperty, value); }
        }

        public bool IsBlockParentScroll { get; set; }

        public bool IsVisibleLogoAndSing { get; set; }

        public GMapNet()
        {
            InitializeComponent();

            MapControl.MinZoom = 1;
            MapControl.MaxZoom = 20;

            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            MapControl.Position = Center.GetGMapPosition();

            GMapProvider googleMapProvider = GMapProviders.GoogleMap;
            MapControl.MapProvider = googleMapProvider;

            MapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            MapControl.Zoom = ZoomLevel;

            MapControl.CanDragMap = true;
            MapControl.DragButton = MouseButton.Left;
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
    }
}