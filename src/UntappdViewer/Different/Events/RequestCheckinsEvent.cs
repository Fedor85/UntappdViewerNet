using System.Collections.Generic;
using Prism.Events;

namespace UntappdViewer.Events
{
    public class RequestCheckinsEvent : PubSubEvent<CallBackConteiner<List<long>>>
    {
    }
}