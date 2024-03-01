using GMap.NET;
using UntappdViewer.UI.Controls.Maps.GMapNet;
using UntappdViewer.UI.Controls.Maps.GMapNet.ViewModel;

namespace UntappdViewer.UI.Helpers
{
    public static class Extension
    {

        public static PointLatLng GetGMapPosition(this Location location)
        {
            return new PointLatLng(location.Latitude, location.Longitude);
        }

        public static MapMarker GetMapMarker(this LocationItem locationItem)
        {
            return new MapMarker(locationItem.Location.GetGMapPosition()) {ToolTip = locationItem.ToolTip};
        }
    }
}
