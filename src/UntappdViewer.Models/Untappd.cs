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
        private DateTime CreatedDate { get;}

        public Untappd(string userName)
        {
            Checkins = new List<Checkin>();
            UserName = userName;
            CreatedDate = DateTime.Now;
        }

        public void AddCheckins(List<Checkin> checkins)
        {
            Checkins.AddRange(checkins);
        }

        public void SortDataDescCheckins()
        {
            Checkins.Sort(SortDataDesc);
        }

        public override string ToString()
        {
            return $"UserName:{UserName}/CreatedDate:{CreatedDate}/CheckinsCount:{Checkins.Count}";
        }

        private int SortDataDesc(Checkin x, Checkin y)
        {
            if (x.CreatedDate < y.CreatedDate)
                return 1;

            if (x.CreatedDate > y.CreatedDate)
                return -1;

            return 1;
        }
    }
}
