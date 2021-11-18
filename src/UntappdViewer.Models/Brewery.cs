using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Brewery
    {
        /// <summary>
        /// id пивоварни
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// наименавине пивоварни
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// url пивоварни
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// локация пивоварни
        /// </summary>
        public Venue Venue { get; set; }

        /// <summary>
        /// url лого
        /// </summary>
        public string LabelUrl { get; set; }

        public Brewery()
        {
            Venue = new Venue();
        }

        public override string ToString()
        {
            return $"Name:{Name}";
        }
    }
}