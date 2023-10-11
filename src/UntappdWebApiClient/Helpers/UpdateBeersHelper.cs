using System;
using System.Collections.Generic;
using System.Linq;
using Beer = UntappdViewer.Models.Beer;
using BeerWeb = QuickType.Beers.WebModels.Beer;

namespace UntappdWebApiClient
{
    public static class UpdateBeersHelper
    {
        public static int UpdateBeers(List<Beer> beers, List<BeerWeb> webBeers)
        {
            int counter = 0;
            foreach (BeerWeb webBeer in webBeers)
            {
                Beer beer = beers.FirstOrDefault(item => item.Id == webBeer.Bid);
                if (beer != null && (UpdateBeer(beer, webBeer)))
                    counter++;
            }
            return counter;
        }

        public static bool UpdateBeer(Beer beer, BeerWeb webBeer)
        {
            bool isUpdate = false;

            if (webBeer.RatingScore > 0 && webBeer.RatingScore != beer.GlobalRatingScore)
            {
                beer.GlobalRatingScore = webBeer.RatingScore;
                isUpdate = true;
            }

            if (webBeer.BeerIbu > 0 && webBeer.BeerIbu != beer.IBU)
            {
                beer.IBU = webBeer.BeerIbu;
                isUpdate = true;
            }

            if (!String.IsNullOrEmpty(webBeer.BeerDescription) && !webBeer.BeerDescription.Equals(beer.Description))
            {
                beer.Description = webBeer.BeerDescription;
                isUpdate = true;
            }
            return isUpdate;
        }
    }
}