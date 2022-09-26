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

        public List<Brewery> Brewerys { get; private set; }

        private List<Venue> venues;

        public CheckinsContainer()
        {
            Checkins = new List<Checkin>();
            Beers = new List<Beer>();
            Brewerys = new List<Brewery>();
            venues = new List<Venue>();
            IsСhanges = false;
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
            return venues.FirstOrDefault(item => item.Equals(venue));
        }

        public void AddCheckin(Checkin checkin)
        {
            Checkins.Add(checkin);
            IsСhanges = true;
        }

        public void AddBeer(Beer beer)
        {
            beer.Changed += BeerChanged;
            Beers.Add(beer);
            IsСhanges = true;
        }

        private void BeerChanged()
        {
            IsСhanges = true;
        }

        public void AddBrewery(Brewery brewery)
        {
            Brewerys.Add(brewery);
            IsСhanges = true;
        }

        public void AddVenue(Venue venue)
        {
            venues.Add(venue);
            IsСhanges = true;
        }
    }
}