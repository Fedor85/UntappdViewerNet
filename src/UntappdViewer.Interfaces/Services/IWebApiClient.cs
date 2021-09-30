using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebApiClient
    {
        void Initialize(string accessToken);

        bool Check();

        List<Checkin> GetFullCheckins();

        List<Checkin> GetFirstCheckins(long endId);

        List<Checkin> GetToEndCheckins(long startId);
    }
}
