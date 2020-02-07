using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Venue
    {
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