using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class SettingsViewModel: RegionManagerBaseModel
    {
        private ISettingService settingService;

        private IModuleManager moduleManager;

        public ICommand CancelButtonCommand { get; }

        public ICommand OkButtonCommand { get; }

        public SettingsViewModel(ISettingService settingService, IRegionManager regionManager, IModuleManager moduleManager) : base(regionManager)
        {
            this.settingService = settingService;
            this.moduleManager = moduleManager;

            CancelButtonCommand = new DelegateCommand(Exit);
            OkButtonCommand = new DelegateCommand(Ok);
        }


        private void Ok()
        {
            Exit();
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}