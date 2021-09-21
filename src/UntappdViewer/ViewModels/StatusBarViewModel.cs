using System;
using System.ComponentModel;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class StatusBarViewModel : ActiveAwareBaseModel
    {
        private InteractionRequestService interactionRequestService;

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }

        public StatusBarViewModel(InteractionRequestService interactionRequestService)
        {
            this.interactionRequestService = interactionRequestService;
        }

        protected override void Activate()
        {
            base.Activate();
            interactionRequestService.ShowMessageOnStatusBarEnvent += ShowMessage;
            interactionRequestService.ClearMessageOnStatusBarEnvent += ClearMessage;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            interactionRequestService.ShowMessageOnStatusBarEnvent -= ShowMessage;
            interactionRequestService.ClearMessageOnStatusBarEnvent -= ClearMessage;
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