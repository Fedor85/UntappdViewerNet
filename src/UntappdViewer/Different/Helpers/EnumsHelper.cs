using System;
using UntappdViewer.Different;
using UntappdViewer.Infrastructure;
using UntappdViewer.Views.Controls.ViewModel;

namespace UntappdViewer.Helpers
{
    public static class EnumsHelper
    {
        public static bool IsValidFileStatus(FileStatus fileStatus)
        {
            return fileStatus == FileStatus.Available;
        }

        public static Entity GetntappdEntity(UntappdEntity untappdEntity)
        {
            return new Entity((long)untappdEntity, GetUntappdEntityName(untappdEntity));
        }

        private static string GetUntappdEntityName(UntappdEntity untappdEntity)
        {
            switch (untappdEntity)
            {
                case UntappdEntity.Checkin: return Properties.Resources.Checkin;
                case UntappdEntity.Beer: return Properties.Resources.Beer;
                case UntappdEntity.Brewery: return Properties.Resources.Brewery;
            }
            return Properties.Resources.NoName;
        }
    }
}