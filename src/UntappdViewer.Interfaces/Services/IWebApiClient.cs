using System;
using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebApiClient
    {
        event Action<int> ChangeUploadedCountEvent;

        void Initialize(string accessToken);

        bool Check();

        void FillFullCheckins(List<Checkin> checkins);

        void FillFullCheckins(CheckinsContainer checkinsContainer);

        void FillFirstCheckins(List<Checkin> checkins, long endId);

        void FillFirstCheckins(CheckinsContainer checkinsContainer, long endId);

        void FillToEndCheckins(List<Checkin> checkins, long startId);

        void FillToEndCheckins(CheckinsContainer checkinsContainer, long endId);

        void BeerUpdate(List<Beer> beers);
    }
}
