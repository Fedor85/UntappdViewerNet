using System;
using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebApiClient
    {
        event Action<string> UploadedProgress;

        void Initialize(string accessToken);

        bool Check();

        void FillFullCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null);

        void FillFirstCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null);

        void FillToEndCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null);

        void UpdateBeers(List<Beer> beers, Func<Beer, bool> predicate, ref long offset, ICancellationToken<Checkin> cancellation = null);

        void FillServingType(List<Checkin> checkins, string defaultServingType, ICancellationToken<Checkin> cancellation = null);

        void FillCollaboration(List<Beer> beers, List<Brewery> breweries, ICancellationToken<Checkin> cancellation = null);

        string GetDevAvatarImageUrl();

        string GetDevProfileHeaderImageUrl();

        ICancellationToken<T> GetCancellationToken<T>();
    }
}