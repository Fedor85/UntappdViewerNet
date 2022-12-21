using System.IO;
using System.Reflection;
using System.Xml;

namespace UntappdViewer.UI.Controls.GeoMap.Data
{
    public static class MapResolver
    {
        public static LvcMap Get(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"This file {filePath} was not found.");

            using (XmlReader reader = XmlReader.Create(filePath, new XmlReaderSettings { IgnoreComments = true }))
                return GetLvcMap(reader);
        }

        public static LvcMap GetResource(string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string sourceFileName = $@"{assembly.GetName().Name}.Resources.{fileName}.xml";
            using (Stream stream = assembly.GetManifestResourceStream(sourceFileName))
                return Get(stream);
        }

        public static LvcMap Get(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
                return GetLvcMap(reader);
        }

        private static LvcMap GetLvcMap(XmlReader reader)
        {
            LvcMap lvcMap = new LvcMap(800, 600);
            while (reader.Read())
            {
                if (reader.Name == "Height")
                    lvcMap.Height = double.Parse(reader.ReadInnerXml());

                if (reader.Name == "Width")
                    lvcMap.Width = double.Parse(reader.ReadInnerXml());

                if (reader.Name == "MapShape")
                {
                    MapData mapData = new MapData();
                    reader.Read();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType != XmlNodeType.Element)
                            reader.Read();

                        if (reader.Name == "Id")
                            mapData.Id = reader.ReadInnerXml();

                        if (reader.Name == "Name")
                            mapData.Name = reader.ReadInnerXml();

                        if (reader.Name == "Path")
                            mapData.Data = reader.ReadInnerXml();
                    }
                    lvcMap.Data.Add(mapData);
                }
            }
            return lvcMap;
        }
    }
}