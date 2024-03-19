﻿using System;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Brewery: BasePropertyChanged
    {
        private string name;

        /// <summary>
        /// id пивоварни
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// наименавине пивоварни
        /// </summary>
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

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