using GMap.NET;
using Microsoft.Maps.MapControl.WPF;
using LocationVM = UntappdViewer.UI.Controls.Maps.BingMap.ViewModel.Location;

namespace UntappdViewer.UI.Helpers
{
    public static class Extension
    {
        public static Location GetMapLocation(this LocationVM location)
        {
            return new Location(location.Latitude, location.Longitude);
        }

        public static LocationVM GetWPFLocation(this Location location)
        {
            return new LocationVM(location.Latitude, location.Longitude);
        }

        public static PointLatLng GetGMapPosition(this LocationVM location)
        {
            return new PointLatLng(location.Latitude, location.Longitude);
        }

        public static LocationVM GetWPFLocation(this PointLatLng location)
        {
            return new LocationVM(location.Lat, location.Lng);
        }
    }
}
