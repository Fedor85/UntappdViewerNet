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
            communicationService.ShowMessageOnStatusBarEnvent += ShowMessage;
            communicationService.ClearMessageOnStatusBarEnvent += ClearMessage;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            communicationService.ShowMessageOnStatusBarEnvent -= ShowMessage;
            communicationService.ClearMessageOnStatusBarEnvent -= ClearMessage;
        }


        private void ShowMessage(string message)
        {
            Message = message;
        }

        private void ClearMessage()
        {
            Message = String.Empty;
        }
    }
}