using System;
namespace UntappdViewer.Models.Different
{
    public static class Extension
    {
        public static bool IsNeedsUpdating(this Brewery brewery)
        {
            return String.IsNullOrEmpty(brewery.Name) && String.IsNullOrEmpty(brewery.Url);
        }
    }
}