using System.Collections.Generic;
using System.Linq;

namespace UntappdViewer.Models
{
    public class CheckinsContainer
    {
        public List<Checkin> Checkins;

        public List<Beer> Beers;

        public List<Brewery> Brewerys;

        public List<Venue> Venues;

        public CheckinsContainer()
        {
            Checkins = new List<Checkin>();
            Beers = new List<Beer>();
            Brewerys = new List<Brewery>();
            Venues = new List<Venue>();
        }

        public Beer GetBeer(long beerId)
        {
            return Beers.FirstOrDefault(item => item.Id == beerId);
        }

        public Brewery GetBrewery(long breweryId)
        {
            return Brewerys.FirstOrDefault(item => item.Id == breweryId);
        }

        public Venue GetVenue(Venue venue)
        {
            return Venues.FirstOrDefault(item => item.Equals(venue));
        }

        public void AddCheckin(Checkin checkin)
        {
            Checkins.Add(checkin);
        }

        public void AddBeer(Beer beer)
        {
            Beers.Add(beer);
        }
        public void AddBrewery(Brewery brewery)
        {
            Brewerys.Add(brewery);
        }

        public void AddVenue(Venue venue)
        {
            Venues.Add(venue);
        }
    }
}