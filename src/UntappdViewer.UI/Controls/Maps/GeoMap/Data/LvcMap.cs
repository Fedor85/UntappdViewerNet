using System.Collections.Generic;

namespace UntappdViewer.UI.Controls.Maps.GeoMap.Data
{
    public class LvcMap
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public List<MapData> Data { get; }

        public LvcMap(double width, double height)
        {
            Width = width;
            Height = height;
            Data = new List<MapData>();
        }
    }
}