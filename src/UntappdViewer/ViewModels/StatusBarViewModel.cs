﻿using System;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class StatusBarViewModel : ActiveAwareBaseModel
    {
        private IInteractionRequestService interactionRequestService;

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        public StatusBarViewModel(IInteractionRequestService interactionRequestService)
        {
            this.interactionRequestService = interactionRequestService;
        }

        protected override void Activate()
        {
            base.Activate();
            interactionRequestService.ShowMessageOnStatusBarEvent += ShowMessage;
            interactionRequestService.ClearMessageOnStatusBarEnvent += ClearMessage;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            interactionRequestService.ShowMessageOnStatusBarEvent -= ShowMessage;
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