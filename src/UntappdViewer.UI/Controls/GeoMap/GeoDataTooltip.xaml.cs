using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UntappdViewer.UI.Controls.GeoMap.Data;

namespace UntappdViewer.UI.Controls.GeoMap
{
    /// <summary>
    /// Interaction logic for GeoDataTooltip.xaml
    /// </summary>
    public partial class GeoDataTooltip : UserControl
    {
        public static readonly DependencyProperty GeoDataProperty = DependencyProperty.Register("ItemsGeoData", typeof(List<GeoData>), typeof(GeoDataTooltip), new PropertyMetadata(default(List<GeoData>)));

        public List<GeoData> ItemsGeoData
        {
            get { return (List<GeoData>)GetValue(GeoDataProperty); }
            set { SetValue(GeoDataProperty, value); }
        }

        public GeoData GeoData
        {
            set { ItemsGeoData = new List<GeoData> { value }; }
        }

        public GeoDataTooltip()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}