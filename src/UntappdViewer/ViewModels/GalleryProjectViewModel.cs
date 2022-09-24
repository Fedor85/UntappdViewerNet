using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
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
            int count = 500;
            List<Checkin> checkins = untappdService.GetCheckins();
            List<Views.Controls.VewModel.CheckinViewModel > viewModels = new List<Views.Controls.VewModel.CheckinViewModel>();

            for (int i = 0; i < checkins.Count; i++)
            {
                string photoPath = untappdService.GetCheckinPhotoFilePath(checkins[i]);
                viewModels.Add(new Views.Controls.VewModel.CheckinViewModel()
                {
                    PhotoPath = photoPath
                });
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