using Prism.Modularity;
using UntappdViewer.Modules;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: ActiveAwareBaseModel
    {
        private IModuleManager moduleManager;

        public MainViewModel(IModuleManager moduleManager)
        {
            this.moduleManager = moduleManager;
        }

        protected override void Activate()
        {
            base.Activate();
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            moduleManager.LoadModule(typeof(StatusBarModule).Name);
        }
    }
}