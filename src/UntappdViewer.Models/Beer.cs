using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Beer
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

        public event Action Changed;

        public Beer()
        {
            Brewery = new Brewery();
        }

        public override string ToString()
        {
            return $"Name:{Name}/Type:{Type}";
        }

        private void OnPropertyChanged()
        {
            if (Changed != null)
                Changed.Invoke();
        }
    }
}
