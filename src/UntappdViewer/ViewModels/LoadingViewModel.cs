using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class LoadingViewModel : ActiveAwareBaseModel
    {
        private IEventAggregator eventAggregator;

        private IInteractionRequestService interactionRequestService;

        private bool visibilityCancelButton;

        private string message;

        public ICommand CancelButtonCommand { get; }

        public bool VisibilityCancelButton
        {
            get { return visibilityCancelButton; }
            set { SetProperty(ref visibilityCancelButton, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public LoadingViewModel(IEventAggregator eventAggregator, IInteractionRequestService interactionRequestService)
        {
            this.eventAggregator = eventAggregator;
            this.interactionRequestService = interactionRequestService;
            CancelButtonCommand = new DelegateCommand(CancelButton);
        }


        protected override void Activate()
        {
            eventAggregator.GetEvent<LoadingActivateCancel>().Subscribe(ActivateCancelButton);
            interactionRequestService.ShowMessageOnLoadingEvent += ShowMessage;
            VisibilityCancelButton = false;
        }

        protected override void DeActivate()
        {
            eventAggregator.GetEvent<LoadingActivateCancel>().Unsubscribe(ActivateCancelButton);
            interactionRequestService.ShowMessageOnLoadingEvent -= ShowMessage;
            VisibilityCancelButton = false;
            Message = String.Empty;
        }

        private void ActivateCancelButton()
        {
            VisibilityCancelButton = true;
        }

        private void CancelButton()
        {
            eventAggregator.GetEvent<LoadingCancel>().Publish();
        }

        private void ShowMessage(string message)
        {
            Message = message;
        }
    }
}