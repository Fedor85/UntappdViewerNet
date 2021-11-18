using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Beer
    {
        /// <summary>
        /// id пива
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// наименования пива
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// тип пива
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// реитинг пива
        /// </summary>
        public double GlobalRatingScore { get; set; }

        /// <summary>
        /// взвешенный рейтинг пива
        /// </summary>
        public double GlobalWeightedRatingScore { get; set; }

        /// <summary>
        /// крепость пива
        /// </summary>
        public double ABV { get; set; }

        /// <summary>
        /// международная единица горечи
        /// </summary>
        public double? IBU { get; set; }

        /// <summary>
        /// url пива
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// url лого
        /// </summary>
        public string LabelUrl { get; set; }

        /// <summary>
        /// пивоварня
        /// </summary>
        public Brewery Brewery { get; set; }

        public Beer()
        {
            Brewery = new Brewery();
        }

        public override string ToString()
        {
            return $"Name:{Name}/Type:{Type}";
        }
    }
}
