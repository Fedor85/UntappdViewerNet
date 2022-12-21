using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UntappdViewer.Interfaces;
using UntappdViewer.UI.Controls.GeoMap.Data;
using Path = System.Windows.Shapes.Path;

namespace UntappdViewer.UI.Controls.GeoMap
{
    /// <summary>
    /// Interaction logic for GeoMap.xaml
    /// </summary>
    public partial class GeoMap : UserControl
    {
        private static readonly DependencyProperty EnableZoomingAndPanningProperty = DependencyProperty.Register("EnableZoomingAndPanning", typeof(bool), typeof(GeoMap), new PropertyMetadata(default(bool)));
        private static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register("DisableAnimations", typeof(bool), typeof(GeoMap), new PropertyMetadata(default(bool)));
        private static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register("AnimationsSpeed", typeof(TimeSpan), typeof(GeoMap), new PropertyMetadata(default(TimeSpan)));

        private static readonly DependencyProperty VisibilityDataProperty = DependencyProperty.Register("VisibilityData", typeof(bool), typeof(GeoMap), new PropertyMetadata(default(bool), VisibilityDataChanged));
        private static readonly DependencyProperty BackgroundDataElementProperty = DependencyProperty.Register("BackgroundDataElement", typeof(Brush), typeof(GeoMap), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0)) { Opacity = 0.4 }));

        private static readonly DependencyProperty BackgroundGridElementProperty = DependencyProperty.Register("BackgroundGridElement", typeof(Brush), typeof(GeoMap), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(165, 165, 165)) { Opacity = 0.5 }));
        private static readonly DependencyProperty ThicknessGridElementProperty = DependencyProperty.Register("ThicknessGridElement", typeof(double), typeof(GridLines), new PropertyMetadata(0.5));
        private static readonly DependencyProperty StepGridElementProperty = DependencyProperty.Register("StepGridElement", typeof(double), typeof(GridLines), new PropertyMetadata(20.0));
        private static readonly DependencyProperty VisibilityGridElementProperty = DependencyProperty.Register("VisibilityGrid", typeof(bool), typeof(GeoMap), new PropertyMetadata(true, VisibilityGridChanged));

        private static readonly DependencyProperty LandBorderProperty = DependencyProperty.Register("LandBorder", typeof(Brush), typeof(GeoMap), new PropertyMetadata(default(Brush)));
        private static readonly DependencyProperty LandBordeThicknessProperty = DependencyProperty.Register("LandBordeThickness", typeof(double), typeof(GeoMap), new PropertyMetadata(default(double)));
        private static readonly DependencyProperty LandFillProperty = DependencyProperty.Register("LandFill", typeof(Brush), typeof(GeoMap), new PropertyMetadata(default(Brush)));
        private static readonly DependencyProperty HeatLandGradientStopCollectionProperty = DependencyProperty.Register("HeatLandGradientStopCollection", typeof(GradientStopCollection), typeof(GeoMap), new PropertyMetadata(default(GradientStopCollection)));
        private static readonly DependencyProperty GradientHelperProperty = DependencyProperty.Register("GradientHelper", typeof(IGradientHelper), typeof(GeoMap), new PropertyMetadata(default(IGradientHelper), GradientHelperChanged));

        private static readonly DependencyProperty LandOpacityProperty = DependencyProperty.Register("LandOpacity", typeof(double), typeof(GeoMap), new PropertyMetadata(default(double)));

        private static readonly DependencyProperty HeatMapProperty = DependencyProperty.Register("HeatMap", typeof(Dictionary<string, double>), typeof(GeoMap), new PropertyMetadata(new Dictionary<string, double>(), HeatMapChanged));
        private static readonly DependencyProperty LanguagePackProperty = DependencyProperty.Register("LanguagePack", typeof(Dictionary<string, string>), typeof(GeoMap), new PropertyMetadata(new Dictionary<string, string>()));

        private static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(GeoMap), new PropertyMetadata(default(string), SourceChanged));

        private Point dragOrigin { get; set; }

        private LvcMap lvcMap { get; set; }

        public bool EnableZoomingAndPanning
        {
            get { return (bool)GetValue(EnableZoomingAndPanningProperty); }
            set { SetValue(EnableZoomingAndPanningProperty, value); }
        }

        public bool DisableAnimations
        {
            get { return (bool)GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan)GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        public bool VisibilityData
        {
            get { return (bool)GetValue(VisibilityDataProperty); }
            set { SetValue(VisibilityDataProperty, value); }
        }

        public Brush BackgroundData
        {
            get { return (Brush)GetValue(BackgroundDataElementProperty); }
            set { SetValue(BackgroundDataElementProperty, value); }
        }

        public Brush BackgroundGrid
        {
            get { return (Brush)GetValue(BackgroundGridElementProperty); }
            set { SetValue(BackgroundGridElementProperty, value); }
        }

        public double ThicknessGrid
        {
            get { return (double)GetValue(ThicknessGridElementProperty); }
            set { SetValue(ThicknessGridElementProperty, value); }
        }

        public double StepGrid
        {
            get { return (double)GetValue(StepGridElementProperty); }
            set { SetValue(StepGridElementProperty, value); }
        }

        public bool VisibilityGrid
        {
            get { return (bool)GetValue(VisibilityGridElementProperty); }
            set { SetValue(VisibilityGridElementProperty, value); }
        }

        public Brush LandBorder
        {
            get { return (Brush)GetValue(LandBorderProperty); }
            set { SetValue(LandBorderProperty, value); }
        }

        public double LandOpacity
        {
            get { return (double)GetValue(LandOpacityProperty); }
            set { SetValue(LandOpacityProperty, value); }
        }

        public Brush LandFill
        {
            get { return (Brush)GetValue(LandFillProperty); }
            set { SetValue(LandFillProperty, value); }
        }

        public GradientStopCollection HeatLandGradientStopCollection
        {
            get { return (GradientStopCollection)GetValue(HeatLandGradientStopCollectionProperty); }
            set { SetValue(HeatLandGradientStopCollectionProperty, value); }
        }

        public IGradientHelper GradientHelper
        {
            get { return (IGradientHelper)GetValue(GradientHelperProperty); }
            set { SetValue(GradientHelperProperty, value); }
        }

        public double LandBordeThickness
        {
            get { return (double)GetValue(LandBordeThicknessProperty); }
            set { SetValue(LandBordeThicknessProperty, value); }
        }

        public Dictionary<string, double> HeatMap
        {
            get { return (Dictionary<string, double>)GetValue(HeatMapProperty); }
            set { SetValue(HeatMapProperty, value); }
        }

        public Dictionary<string, string> LanguagePack
        {
            get { return (Dictionary<string, string>)GetValue(LanguagePackProperty); }
            set { SetValue(LanguagePackProperty, value); }
        }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public event Action<GeoData> LandClick;

        public GeoMap()
        {
            InitializeComponent();
            LandBorder = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            LandBordeThickness = 1.5;
            LandFill = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            LandOpacity = 1;

            GradientStopCollection gradientStopCollection = new GradientStopCollection();
            gradientStopCollection.Add(new GradientStop(Color.FromRgb(190, 230, 255), 0));
            gradientStopCollection.Add(new GradientStop(Color.FromRgb(2, 119, 188), 1));
            HeatLandGradientStopCollection = gradientStopCollection;

            AnimationsSpeed = TimeSpan.FromMilliseconds(500);

            GeoDataTooltip.SetBinding(BackgroundProperty, new Binding { Path = new PropertyPath(BackgroundDataElementProperty), Source = this });
            UndefinedGeoDataTooltip.SetBinding(BackgroundProperty, new Binding { Path = new PropertyPath(BackgroundDataElementProperty), Source = this });

            GradientScale.SetBinding(BackgroundProperty, new Binding { Path = new PropertyPath(BackgroundDataElementProperty), Source = this });
            GradientScale.SetBinding(GradientScale.GradientStopCollectionProperty, new Binding { Path = new PropertyPath(HeatLandGradientStopCollectionProperty), Source = this });

            GridLines.SetBinding(GridLines.BackgroundGridProperty, new Binding { Path = new PropertyPath(BackgroundGridElementProperty), Source = this });
            GridLines.SetBinding(GridLines.ThicknessGridProperty, new Binding { Path = new PropertyPath(ThicknessGridElementProperty), Source = this });
            GridLines.SetBinding(GridLines.StepGridProperty, new Binding { Path = new PropertyPath(StepGridElementProperty), Source = this });

            InitializeWorld();
        }

        private void InitializeWorld()
        {
            Map.Children.Clear();

            lvcMap = String.IsNullOrEmpty(Source) ? MapResolver.GetResource("World") : MapResolver.Get(Source);
            foreach (MapData mapData in lvcMap.Data)
                AddLand(mapData);
        }

        private void AddLand(MapData mapData)
        {
            Path path = new Path();

            path.Data = Geometry.Parse(mapData.Data);

            path.SetBinding(Shape.StrokeProperty, new Binding { Path = new PropertyPath(LandBorderProperty), Source = this });
            path.SetBinding(Shape.StrokeThicknessProperty, new Binding { Path = new PropertyPath(LandBordeThicknessProperty), Source = this });
            path.SetBinding(Shape.FillProperty, new Binding { Path = new PropertyPath(LandFillProperty), Source = this });
            path.SetBinding(OpacityProperty, new Binding { Path = new PropertyPath(LandOpacityProperty), Source = this });

            path.MouseEnter += PathMouseEnter;
            path.MouseLeave += PathMouseLeave;
            path.MouseMove += PathMouseMove;
            path.MouseDown += PathMouseDown;

            Canvas.SetTop(path, 0);
            Canvas.SetLeft(path, 0);

            mapData.Shape = path;
            Map.Children.Add(path);
        }
        private void ShowMeSomeHeat()
        {
            double min = HeatMap.Values.Min();
            double max = HeatMap.Values.Max();

            foreach (MapData mapData in lvcMap.Data)
            {
                Path path = mapData.Shape as Path;
                if (path == null)
                    continue;

                path.SetBinding(Shape.FillProperty, new Binding { Path = new PropertyPath(LandFillProperty), Source = this });
                if (!HeatMap.ContainsKey(mapData.Id))
                    continue;

                Color color = GetColor(min, max, HeatMap[mapData.Id]);
                if (DisableAnimations)
                {
                    path.Fill = new SolidColorBrush(color);
                }
                else
                {
                    path.Fill = new SolidColorBrush();
                    ((SolidColorBrush)path.Fill).BeginAnimation(SolidColorBrush.ColorProperty, new ColorAnimation(color, AnimationsSpeed));
                }
            }
            FillUndefinedGeoData();
        }

        private void FillUndefinedGeoData()
        {
            List<GeoData> geoDatas = new List<GeoData>();

            IEnumerable<string> ids = lvcMap.Data.Select(item => item.Id);
            foreach (KeyValuePair<string, double> keyValuePair in HeatMap.Where(item => !ids.Contains(item.Key)))
                geoDatas.Add(new GeoData(keyValuePair.Key, keyValuePair.Value));

            UndefinedGeoDataTooltip.ItemsGeoData = geoDatas;
        }

        #region PathEvent

        private void PathMouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            path.Opacity = LandOpacity - 0.3;

            if (HeatMap == null || !VisibilityData)
                return;

            MapData mapData = lvcMap.Data.FirstOrDefault(item => item.Shape.Equals(path));
            if (mapData == null)
                return;

            if (!HeatMap.ContainsKey(mapData.Id))
                return;

            GeoDataTooltip.Visibility = Visibility.Visible;
            GeoDataTooltip.GeoData = GetGeoData(mapData);
        }

        private void PathMouseLeave(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            path.Opacity = LandOpacity;
            GeoDataTooltip.Visibility = Visibility.Hidden;
        }

        private void PathMouseMove(object sender, MouseEventArgs e)
        {
            Point location = e.GetPosition(this);
            GeoDataTooltip.UpdateLayout();
            const double delta = 5;
            double x = ActualWidth < location.X + delta + GeoDataTooltip.ActualWidth ? location.X - delta - GeoDataTooltip.ActualWidth : location.X + delta;
            double y = ActualHeight < location.Y + delta + GeoDataTooltip.ActualHeight ? location.Y - delta - GeoDataTooltip.ActualHeight : location.Y + delta;
            Canvas.SetLeft(GeoDataTooltip, x);
            Canvas.SetTop(GeoDataTooltip, y);
        }

        private void PathMouseDown(object sender, MouseButtonEventArgs e)
        {
            Path path = sender as Path;
            MapData mapData = lvcMap.Data.FirstOrDefault(item => item.Shape.Equals(path));
            if (mapData != null)
                LandClick?.Invoke(GetGeoData(mapData));
        }

        #endregion

        private void UndefinedGeoDataMouseEnter(object sender, MouseEventArgs e)
        {
            if (VisibilityData && UndefinedGeoDataTooltip.ItemsGeoData.Count > 0)
                UndefinedGeoDataTooltip.Visibility = Visibility.Visible;
        }

        private void UndefinedGeoDataMouseLeave(object sender, MouseEventArgs e)
        {
            UndefinedGeoDataTooltip.Visibility = Visibility.Hidden;
        }

        #region MainCanvasEvent

        private void MainMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!EnableZoomingAndPanning)
                return;

            e.Handled = true;
            ScaleTransform scaleTransform = Map.RenderTransform as ScaleTransform;
            double scaleX = scaleTransform?.ScaleX ?? 1;
            scaleX += e.Delta > 0 ? 0.05 : -0.05;
            scaleX = scaleX < 1 ? 1 : scaleX;
            Point point = e.GetPosition(Map);

            if (e.Delta > 0)
                Map.RenderTransformOrigin = new Point(point.X / Map.ActualWidth, point.Y / Map.ActualWidth);

            Map.RenderTransform = new ScaleTransform(scaleX, scaleX);
        }

        private void MainLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!EnableZoomingAndPanning)
                return;

            dragOrigin = e.GetPosition(this);
        }

        private void MainLeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!EnableZoomingAndPanning)
                return;

            Point end = e.GetPosition(this);
            if (end.Equals(dragOrigin))
                return;

            Point delta = new Point(dragOrigin.X - end.X, dragOrigin.Y - end.Y);
            foreach (UIElement path in Map.Children.OfType<UIElement>())
            {
                double x = Canvas.GetLeft(path) - delta.X;
                double y = Canvas.GetTop(path) - delta.Y;
                MovieUIElement(path, x, y);
            }
        }

        private void MainMouseRightUp(object sender, MouseButtonEventArgs e)
        {
            if (!EnableZoomingAndPanning)
                return;

            foreach (UIElement path in Map.Children.OfType<UIElement>())
                MovieUIElement(path, 0, 0);

            if (DisableAnimations)
            {
                Map.RenderTransform = new ScaleTransform(1, 1);
            }
            else if (!Map.RenderTransform.IsFrozen)
            {
                Map.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(1, AnimationsSpeed));
                Map.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, new DoubleAnimation(1, AnimationsSpeed));
            }
        }

        private void MainSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        #endregion

        private void Draw()
        {
            DrawWorld();
        }

        private void DrawWorld()
        {
            double scale = SetMapSize(lvcMap.Width, lvcMap.Height);
            ScaleTransform scaleTransform = new ScaleTransform(scale, scale);

            foreach (Path path in Map.Children.OfType<Path>())
                path.RenderTransform = scaleTransform;
        }

        private GeoData GetGeoData(MapData mapData)
        {
            string name = mapData.Name;
            double value = HeatMap[mapData.Id];
            if (LanguagePack != null && LanguagePack.ContainsKey(mapData.Id))
                name = LanguagePack[mapData.Id];

            return new GeoData(name, value);
        }

        private double SetMapSize(double width, double height)
        {
            double ratio = width / height;
            double widthRatio = ActualWidth / width;
            double heightRatio = ActualHeight / height;

            double scale;
            if (widthRatio < heightRatio)
            {
                Map.Width = ActualWidth;
                Map.Height = ActualWidth / ratio;
                scale = widthRatio;
            }
            else
            {
                Map.Width = ActualHeight * ratio;
                Map.Height = ActualHeight;
                scale = heightRatio;
            }
            return scale;
        }


        private void MovieUIElement(UIElement path, double x, double y)
        {
            if (DisableAnimations)
            {
                Canvas.SetTop(path, y);
                Canvas.SetLeft(path, x);
            }
            else
            {
                path.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(y, AnimationsSpeed));
                path.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(x, AnimationsSpeed));
            }
        }

        private void SetRangeValue(double? min, double? max)
        {
            GradientScale.Start = min;
            GradientScale.End = max;
        }

        #region GradientColor

        private Color GetColor(double min, double max, double current)
        {
            return GradientHelper?.GetRelativeColor(min, max, current) ?? GetBaseColor(min, max, current);
        }

        private Color GetBaseColor(double min, double max, double current)
        {
            double temperature = LinealInterpolation(0, 1, min, max, current);
            return ColorInterpolation(temperature);
        }

        private double LinealInterpolation(double fromComponent, double toComponent, double fromOffset, double toOffset, double value)
        {
            Point p1 = new Point(fromOffset, fromComponent);
            Point p2 = new Point(toOffset, toComponent);

            double deltaX = p2.X - p1.X;
            double m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);

            return m * (value - p1.X) + p1.Y;
        }

        private Color ColorInterpolation(double weight)
        {
            Color from = Color.FromRgb(0, 0, 0), to = Color.FromRgb(0, 0, 0);
            double fromOffset = 0, toOffset = 0;

            for (var i = 0; i < HeatLandGradientStopCollection.Count - 1; i++)
            {
                if (HeatLandGradientStopCollection[i].Offset <= weight && HeatLandGradientStopCollection[i + 1].Offset >= weight)
                {
                    from = HeatLandGradientStopCollection[i].Color;
                    to = HeatLandGradientStopCollection[i + 1].Color;

                    fromOffset = HeatLandGradientStopCollection[i].Offset;
                    toOffset = HeatLandGradientStopCollection[i + 1].Offset;

                    break;
                }
            }

            return Color.FromArgb((byte)LinealInterpolation(from.A, to.A, fromOffset, toOffset, weight),
                                    (byte)LinealInterpolation(from.R, to.R, fromOffset, toOffset, weight),
                                    (byte)LinealInterpolation(from.G, to.G, fromOffset, toOffset, weight),
                                    (byte)LinealInterpolation(from.B, to.B, fromOffset, toOffset, weight));
        }

        #endregion

        private static void HeatMapChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GeoMap geoMap = dependencyObject as GeoMap;
            if (e.NewValue == null)
            {
                geoMap.SetRangeValue(null, null);
            }
            else
            {
                Dictionary<string, double> heatMap = e.NewValue as Dictionary<string, double>;
                geoMap.SetRangeValue(heatMap.Values.Min(), heatMap.Values.Max());
                geoMap.ShowMeSomeHeat();
            }

        }

        private static void SourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GeoMap geoMap = dependencyObject as GeoMap;
            geoMap.InitializeWorld();
        }

        private static void GradientHelperChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GeoMap geoMap = dependencyObject as GeoMap;
            IGradientHelper gradientHelper = e.NewValue as IGradientHelper;
            geoMap.HeatLandGradientStopCollection = (GradientStopCollection)gradientHelper?.GetGradientStopCollection();
        }

        private static void VisibilityDataChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GeoMap geoMap = dependencyObject as GeoMap;
            geoMap.GradientScale.Visibility = Convert.ToBoolean(e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void VisibilityGridChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            GeoMap geoMap = dependencyObject as GeoMap;
            geoMap.GridLines.Visibility = Convert.ToBoolean(e.NewValue) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
