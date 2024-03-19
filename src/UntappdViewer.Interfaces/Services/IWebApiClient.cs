using System;
using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebApiClient
    {
        event Action<string> UploadedProgress;

        bool LogOn(string accessToken);

        void LogOff();

        bool IsLogOn { get; }

        void FillFullCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null);

        void FillFirstCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null);

        void FillToEndCheckins(CheckinsContainer checkinsContainer, ICancellationToken<Checkin> cancellation = null);

        void UpdateBeers(List<Beer> beers, Func<Beer, bool> predicate, ref long offset, IBaseCancellationToken cancellation = null);

        void FillServingType(List<Checkin> checkins, string defaultServingType, IBaseCancellationToken cancellation = null);

        void FillCollaboration(List<Beer> beers, List<Brewery> breweries, IBaseCancellationToken cancellation = null);

        string GetDevAvatarImageUrl();

        string GetDevProfileHeaderImageUrl();

        ICancellationToken<T> GetCancellationToken<T>();
    }
}