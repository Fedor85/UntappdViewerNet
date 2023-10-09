using System;
using System.Collections.Generic;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Beer: BasePropertyChanged
    {
        private string description;

        private double globalRatingScore;

        private long? ibu;

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
        /// описание пива
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// реитинг пива
        /// </summary>
        public double GlobalRatingScore
        {
            get { return globalRatingScore; }
            set
            {
                globalRatingScore = value;
                OnPropertyChanged();
            }
        }

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
        public long? IBU
        {
            get { return ibu; }
            set
            {
                ibu = value;
                OnPropertyChanged();
            }
        }

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

        public Collaboration Collaboration { get; }

        public Beer()
        {
            Brewery = new Brewery();
            Collaboration = new Collaboration();
        }

        public IEnumerable<Brewery> GetFullBreweries()
        {
            List<Brewery> breweries = new List<Brewery>();
            breweries.Add(Brewery);
            breweries.AddRange(Collaboration.Brewerys);
            return breweries;
        }

        public override string ToString()
        {
            return $"Name:{Name}/Type:{Type}";
        }
    }
}