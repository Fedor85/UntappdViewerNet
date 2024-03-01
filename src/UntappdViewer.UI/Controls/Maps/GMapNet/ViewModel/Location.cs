namespace UntappdViewer.UI.Controls.Maps.GMapNet.ViewModel
{
    [System.ComponentModel.TypeConverter(typeof(LocationConverter))]
    public class Location
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}