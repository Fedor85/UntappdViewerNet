using System;
namespace UntappdViewer.Models
{
    [Serializable]
    public class Untappd
    {
        private bool isСhanges { get; set; }

        /// <summary>
        /// Для отслеживания версии файла, чтобы моджно было раотать с разными версиями
        /// </summary>
        private int version { get; set; }

        public CheckinsContainer CheckinsContainer { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Дата создания проекта
        /// </summary>
        public DateTime CreatedDate { get;}


        public Untappd(string userName)
        {
            CheckinsContainer = new CheckinsContainer();
            UserName = userName;
            CreatedDate = DateTime.Now;
            isСhanges = false;
            version = 3;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
            isСhanges = true;
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

        public override string ToString()
        {
            return $"UserName:{UserName}/CreatedDate:{CreatedDate}/CheckinsCount:{CheckinsContainer.Checkins.Count}";
        }
    }
}