using System;
using Prism;
using Prism.Mvvm;

namespace UntappdViewer.ViewModels
{
    public abstract class ActiveAwareBaseModel : BindableBase, IActiveAware
    {
        private bool active;

        public event EventHandler IsActiveChanged;

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

        protected virtual void Activate()
        {
        }

        protected virtual void DeActivate()
        {
        }
    }
}