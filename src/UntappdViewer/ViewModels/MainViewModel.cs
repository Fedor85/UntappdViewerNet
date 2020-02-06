using Prism.Modularity;
using UntappdViewer.Modules;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel
    {
        private IModuleManager moduleManager { get; }

        public MainViewModel(IModuleManager moduleManager)
        {
            this.moduleManager = moduleManager;
            InitializeModules();
        }

        private void InitializeModules()
        {
            moduleManager.LoadModule(typeof(MainModule).Name);
        }
    }
}