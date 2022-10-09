using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using UntappdViewer.Events;

namespace UntappdViewer.ViewModels
{
    public class LoadingViewModel : ActiveAwareBaseModel
    {
        private IEventAggregator eventAggregator;

        private bool visibilityCancelButton;

        public ICommand CancelButtonCommand { get; }

        public bool VisibilityCancelButton
        {
            get { return visibilityCancelButton; }
            set { SetProperty(ref visibilityCancelButton, value); }
        }

        public LoadingViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CancelButtonCommand = new DelegateCommand(CancelButton);
        }


        protected override void Activate()
        {
            eventAggregator.GetEvent<LoadingActivateCancel>().Subscribe(ActivateCancelButton);
            VisibilityCancelButton = false;
        }

        protected override void DeActivate()
        {
            eventAggregator.GetEvent<LoadingActivateCancel>().Unsubscribe(ActivateCancelButton);
            VisibilityCancelButton = false;
        }

        private void ActivateCancelButton()
        {
            VisibilityCancelButton = true;
        }
        private void CancelButton()
        {
            eventAggregator.GetEvent<LoadingCancel>().Publish();
        }
    }
}