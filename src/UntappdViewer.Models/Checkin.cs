using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Checkin
    {
        /// <summary>
        /// id чекина
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// url чекина
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// дата чекина
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// рейтинг чекина
        /// </summary>
        public double? RatingScore { get; set; }

        /// <summary>
        /// тип тары
        /// </summary>
        public string ServingType { get; set; }

        /// <summary>
        /// профиль вкуса
        /// </summary>
        public string FlavorPprofiles { get; set; }

        /// <summary>
        /// место чекина
        /// </summary>
        public Venue Venue { get; set; }

        /// <summary>
        /// место покупки
        /// </summary>
        public Venue VenuePurchase { get; set; }

        /// <summary>
        /// комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Пиво
        /// </summary>
        public Beer Beer { get; set; }

        /// <summary>
        /// url фотки
        /// </summary>
        public string UrlPhoto { get; set; }

        public Checkin()
        {
            Venue = new Venue();
            VenuePurchase = new Venue();
            Beer = new Beer();
        }

        public override string ToString()
        {
            return  $"Beer:{Beer}/CreatedDate:{CreatedDate}/RatingScore:{(RatingScore.HasValue ? (object) RatingScore.Value : "no data")}";
        }
    }
}