using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;
using Checkin = UntappdViewer.Models.Checkin;

namespace UntappdViewer.ViewModels
{
    public class GalleryProjectViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IUntappdService untappdService;

        private IEnumerable items;

        public ICommand OkButtonCommand { get; }

        public IEnumerable Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
            }
        }

        public GalleryProjectViewModel(IRegionManager regionManager, IModuleManager moduleManager,
                                                                    IUntappdService untappdService) : base(regionManager)
        {
            this.moduleManager = moduleManager;
            this.untappdService = untappdService;


            OkButtonCommand = new DelegateCommand(Exit);
        }

        protected override void Activate()
        {
            base.Activate();
            Items = GetItems();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            Items = null;
        }

        private IEnumerable GetItems()
        {
            List<Views.Controls.VewModel.CheckinViewModel > viewModels = new List<Views.Controls.VewModel.CheckinViewModel>();
            foreach (Checkin checkin in untappdService.GetCheckins())
            {
                string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);
                viewModels.Add(ConverterHelper.GetCheckinViewModel(checkin, photoPath));
            }
            return viewModels;
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}