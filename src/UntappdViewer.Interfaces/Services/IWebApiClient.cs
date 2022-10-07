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

        void FillFullCheckins(CheckinsContainer checkinsContainer);

        void FillFirstCheckins(CheckinsContainer checkinsContainer);

        void FillToEndCheckins(CheckinsContainer checkinsContainer);

        void UpdateBeers(List<Beer> beers, Func<Beer, bool> predicate, ref long offset);

        void FillServingType(List<Checkin> checkins, ICancellationToken<Checkin> cancellation = null);

        ICancellationToken<T> GetCancellationToken<T>();
    }
}