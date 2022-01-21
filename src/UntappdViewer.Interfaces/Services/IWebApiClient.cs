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

        void FillFullCheckins(CheckinsContainer checkinsContainer);

        void FillFirstCheckins(CheckinsContainer checkinsContainer, long endId);

        void FillToEndCheckins(CheckinsContainer checkinsContainer, long endId);

        void BeerUpdate(List<Beer> beers);
    }
}
