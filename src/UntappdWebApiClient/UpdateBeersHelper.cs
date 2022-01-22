using System.Collections.Generic;
using System.Linq;
using QuickType.Beers.WebModels;
using Beer = UntappdViewer.Models.Beer;
using BeerWeb = QuickType.Beers.WebModels.Beer;

namespace UntappdWebApiClient
{
    public static class UpdateBeersHelper
    {
        public static void UpdateBeers(List<Beer> beers, BeersQuickType beersQuickType)
        {
            List<BeerWeb> webBeers = beersQuickType.Response.Beers.Items.Select(item => item.Beer).ToList();
            foreach (BeerWeb webBeer in webBeers)
            {
                Beer beer = beers.FirstOrDefault(item => item.Id == webBeer.Bid);
                if (beer == null)
                    continue;

                beer.GlobalRatingScore = webBeer.RatingScore;
                beer.IBU = webBeer.BeerIbu;
                beer.Description = webBeer.BeerDescription;
            }
        }
    }
}
