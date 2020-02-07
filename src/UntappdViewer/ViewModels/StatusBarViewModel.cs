using System;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class StatusBarViewModel : ActiveAwareBaseModel
    {
        private ICommunicationService communicationService;

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public StatusBarViewModel(ICommunicationService communicationService)
        {
            this.communicationService = communicationService;
        }

        protected override void Activate()
        {
            base.Activate();
            communicationService.ShowMessageEnvent += CommunicationServiceShowMessageEnvent;
            communicationService.ClearMessageEnvent += CommunicationService_ClearMessageEnvent;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            communicationService.ShowMessageEnvent -= CommunicationServiceShowMessageEnvent;
            communicationService.ClearMessageEnvent -= CommunicationService_ClearMessageEnvent;
        }


        private void CommunicationServiceShowMessageEnvent(string message)
        {
            Message = message;
        }

        private void CommunicationService_ClearMessageEnvent()
        {
            Message = String.Empty;
        }
    }
}