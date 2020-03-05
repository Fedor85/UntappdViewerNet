using System.Collections.Generic;
using Prism.Events;
using UntappdViewer.Models;

namespace UntappdViewer.Events
{
    public class RequestCheckinsEvent : PubSubEvent<CallBackConteiner<List<Checkin>>>
    {
    }
}