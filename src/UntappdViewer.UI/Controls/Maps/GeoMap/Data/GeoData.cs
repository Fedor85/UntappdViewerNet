namespace UntappdViewer.UI.Controls.Maps.GeoMap.Data
{
    public class GeoData
    {
        public string Name { get; private set; }

        public double Value { get; private set; }

        public GeoData(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}