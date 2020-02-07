using System;
using Prism.Modularity;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: ActiveAwareBaseModel
    {
        private UntappdService untappdService;

        private IModuleManager moduleManager;

        private ICommunicationService communicationService;

        public MainViewModel(UntappdService untappdService, IModuleManager moduleManager, ICommunicationService communicationService)
        {
            this.untappdService = untappdService;
            this.moduleManager = moduleManager;
            this.communicationService = communicationService;
        }

        protected override void Activate()
        {
            base.Activate();
            moduleManager.LoadModule(typeof(StatusBarModule).Name);
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            communicationService.ShowMessage(GetLoadingMessage());
        }

        private string GetLoadingMessage()
        {
            return $"{Properties.Resources.LoadingFrom} {untappdService.FIlePath}";
        }
    }
}