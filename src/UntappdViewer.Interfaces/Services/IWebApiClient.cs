using System.Collections.Generic;
using UntappdViewer.Models;

namespace UntappdViewer.Interfaces.Services
{
    public interface IWebApiClient
    {
        void Initialize(string accessToken);

        bool Check(out string message);

        List<Checkin> GetCheckins();
    }
}
