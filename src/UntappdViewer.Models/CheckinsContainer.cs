using System;
using System.Collections.Generic;
using System.Linq;

namespace UntappdViewer.Models
{
    [Serializable]
    public class CheckinsContainer
    {
        public bool IsСhanges { get; set; }

        public List<Checkin> Checkins { get; private set; }

        public List<Beer> Beers { get; private set; }

        private List<Brewery> brewerys;

        private List<Venue> venues;

        public CheckinsContainer()
        {
            Checkins = new List<Checkin>();
            Beers = new List<Beer>();
            brewerys = new List<Brewery>();
            venues = new List<Venue>();
            IsСhanges = false;
        }

        public Beer GetBeer(long beerId)
        {
            return Beers.FirstOrDefault(item => item.Id == beerId);
        }

        public Brewery GetBrewery(long breweryId)
        {
            return brewerys.FirstOrDefault(item => item.Id == breweryId);
        }

        public Venue GetVenue(Venue venue)
        {
            return venues.FirstOrDefault(item => item.Equals(venue));
        }

        public void AddCheckin(Checkin checkin)
        {
            Checkins.Add(checkin);
            IsСhanges = true;
        }

        public void AddBeer(Beer beer)
        {
            Beers.Add(beer);
            IsСhanges = true;
        }
        public void AddBrewery(Brewery brewery)
        {
            brewerys.Add(brewery);
            IsСhanges = true;
        }

        public void AddVenue(Venue venue)
        {
            venues.Add(venue);
            IsСhanges = true;
        }
    }
}