using System;
using System.Collections.Generic;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Untappd
    {

        public CheckinsContainer CheckinsContainer { get; private set; }

        ///// <summary>
        /// Чекины
        /// </summary>
        public List<Checkin> Checkins { get { return CheckinsContainer.Checkins; } }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Дата создания проекта
        /// </summary>
        public DateTime CreatedDate { get;}

        private bool isСhanges { get; set; }

        public Untappd(string userName)
        {
            CheckinsContainer = new CheckinsContainer();
            UserName = userName;
            CreatedDate = DateTime.Now;
            isСhanges = false;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
            isСhanges = true;
        }

        public void SortDataDescCheckins()
        {
            Checkins.Sort(SortCheckinsDataDesc);
        }

        public bool IsСhanges()
        {
            return isСhanges || CheckinsContainer.IsСhanges;
        }

        public void ResetСhanges()
        {
            isСhanges = false;
            CheckinsContainer.IsСhanges = false;
        }

        public List<Checkin> GetUniqueCheckins()
        {
            List<Checkin> checkins = new List<Checkin>();
            Dictionary<long, List<Checkin>> beers = new Dictionary<long, List<Checkin>>();
            foreach (Checkin checkin in Checkins)
            {
                if (beers.ContainsKey(checkin.Beer.Id))
                    beers[checkin.Beer.Id].Add(checkin);
                else
                    beers.Add(checkin.Beer.Id, new List<Checkin> {checkin});
            }

            foreach (KeyValuePair<long, List<Checkin>> keyValuePair in beers)
            {
                if (keyValuePair.Value.Count == 1)
                {
                    checkins.Add(keyValuePair.Value[0]);
                    continue;
                }
                Checkin addedCheckin = null;
                keyValuePair.Value.Sort(SortCheckinsDataDesc);
                foreach (Checkin curretCheckin in keyValuePair.Value)
                {
                    if (!String.IsNullOrEmpty(curretCheckin.UrlPhoto))
                        addedCheckin = curretCheckin;
                    break;
                }

                if (addedCheckin == null)
                    addedCheckin = keyValuePair.Value[0];

                checkins.Add(addedCheckin);
            }
            beers.Clear();
            checkins.Sort(SortCheckinsDataDesc);
            return checkins;
        }

        public override string ToString()
        {
            return $"UserName:{UserName}/CreatedDate:{CreatedDate}/CheckinsCount:{Checkins.Count}";
        }

        private int SortCheckinsDataDesc(Checkin x, Checkin y)
        {
            if (x.CreatedDate < y.CreatedDate)
                return 1;

            if (x.CreatedDate > y.CreatedDate)
                return -1;

            return 1;
        }
    }
}