using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Venue
    {
        /// <summary>
        /// id локации
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// имя локации
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// страна локации
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// штат локации
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// город локации
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// широта
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// долгота
        /// </summary>
        public double? Longitude { get; set; }

        public Venue()
        {
            Name = String.Empty;
            Country = String.Empty;
            State = String.Empty;
            City = String.Empty;
        }

        public bool Equals(Venue venue)
        {
            return Id == venue.Id &&
                   Name == venue.Name &&
                   Country == venue.Country &&
                   State == venue.State &&
                   City == venue.City &&
                   Latitude == venue.Latitude &&
                   Longitude == venue.Longitude;
        }

        public override string ToString()
        {
            return $"{GetDispayParameter("Name", Name)}/{GetDispayParameter("Country", Country)}/{GetDispayParameter("State", State)}/{GetDispayParameter("City", City)}";
        }

        private string GetDispayParameter(string caption, string valaue)
        {
            return $"{caption}:{(String.IsNullOrEmpty(valaue) ? "no data" : valaue)}";
        }
    }
}