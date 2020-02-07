using System;
using Prism;
using Prism.Modularity;
using UntappdViewer.Modules;

namespace UntappdViewer.ViewModels
{
    public class MainViewModel: IActiveAware
    {
        private IModuleManager moduleManager;

        private bool active;

        public event EventHandler IsActiveChanged;

        public MainViewModel(IModuleManager moduleManager)
        {
            this.moduleManager = moduleManager;
        }

        public bool IsActive
        {
            get
            {
                return active;
            }
            set
            {
                if (active != value)
                {
                    active = value;
                    if (active)
                        Activate();
                    else
                        DeActivate();
                }
            }
        }
        private void Activate()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            moduleManager.LoadModule(typeof(StatusBarModule).Name);
        }

        private void DeActivate()
        {
        }
    }
}