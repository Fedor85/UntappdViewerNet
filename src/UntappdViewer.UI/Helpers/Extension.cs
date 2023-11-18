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
    }
}
