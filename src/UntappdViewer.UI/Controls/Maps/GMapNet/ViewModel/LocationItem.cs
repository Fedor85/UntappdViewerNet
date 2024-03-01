namespace UntappdViewer.UI.Controls.Maps.GMapNet.ViewModel
{
    public class LocationItem
    {
        public Location Location { get; private set; }

        public string ToolTip { get; set; }

        public LocationItem(double latitude, double longitude)
        {
            Location = new Location(latitude, longitude);
        }
    }
}