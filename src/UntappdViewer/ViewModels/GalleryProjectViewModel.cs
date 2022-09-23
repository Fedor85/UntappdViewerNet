using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Modules;
using UntappdViewer.Views;

namespace UntappdViewer.ViewModels
{
    public class GalleryProjectViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        public ICommand OkButtonCommand { get; }

        public GalleryProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            OkButtonCommand = new DelegateCommand(Exit);
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}