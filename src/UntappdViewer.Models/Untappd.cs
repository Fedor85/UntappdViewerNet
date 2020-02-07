using System;
using System.Collections.Generic;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Untappd
    {
        /// <summary>
        /// Чекины
        /// </summary>
        public List<Checkin> Checkins { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Дата создания проекта
        /// </summary>
        public DateTime CreatedDate { get;}

        public Untappd(string userName)
        {
            Checkins = new List<Checkin>();
            UserName = userName;
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"UserName:{UserName}/CreatedDate:{CreatedDate}/CheckinsCount:{Checkins.Count}";
        }
    }
}
