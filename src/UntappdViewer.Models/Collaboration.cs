using System;
using System.Collections.Generic;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Models
{
    [Serializable]
    public class Collaboration : BasePropertyChanged
    {
        private CollaborationState state;

        public CollaborationState State
        {
            get { return state; }
            private set { SetProperty(ref state, value); }
        }

        public List<Brewery> Brewerys { get; }

        public Collaboration()
        {
            Brewerys = new List<Brewery>();
            State = CollaborationState.Undefined;
        }

        public void AddBrewery(Brewery brewery)
        {
            Brewerys.Add(brewery);
            SetDefined();
        }

        public void SetDefined()
        {
            State = CollaborationState.Defined;
        }
    }
}
