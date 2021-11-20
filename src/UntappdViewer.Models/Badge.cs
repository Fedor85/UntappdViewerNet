using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Badge
    {
        /// <summary>
        /// id значка
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// наименования значка
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// описание значка
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// url изображения
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
