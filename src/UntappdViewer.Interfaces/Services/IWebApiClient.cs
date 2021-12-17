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

        void FillFirstCheckins(List<Checkin> checkins, long endId);

        void FillToEndCheckins(List<Checkin> checkins, long startId);

        void BeerUpdate(List<Beer> beers);
    }
}
