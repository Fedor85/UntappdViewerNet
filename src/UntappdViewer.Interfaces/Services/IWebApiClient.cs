using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebApiClient
    {
        void Initialize(string accessToken);

        bool Check();

        List<Checkin> GetCheckins();
    }
}
