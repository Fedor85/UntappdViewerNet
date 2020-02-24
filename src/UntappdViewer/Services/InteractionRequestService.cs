using Prism.Interactivity.InteractionRequest;

namespace UntappdViewer.Services
{
    public class InteractionRequestService
    {
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }

        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public InteractionRequestService()
        {
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            NotificationRequest = new InteractionRequest<INotification>();
        }
    }
}