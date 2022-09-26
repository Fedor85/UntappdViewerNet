using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Different;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.Views.Controls.ViewModel;
using Checkin = UntappdViewer.Models.Checkin;
using Untappd = UntappdViewer.Views.Untappd;

namespace UntappdViewer.ViewModels
{
    public class GalleryProjectViewModel: RegionManagerBaseModel
    {
        private IModuleManager moduleManager;

        private IUntappdService untappdService;

        private IEnumerable items;

        private IEnumerable displayTypeItems;

        private Entity selectTypeDisplayItem;

        public ICommand OkButtonCommand { get; }

        public IEnumerable Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
            }
        }

        public IEnumerable DisplayTypeItems
        {
            get { return displayTypeItems; }
            set
            {
                SetProperty(ref displayTypeItems, value);
            }
        }

        public Entity SelectTypeDisplayItem
        {
            get { return selectTypeDisplayItem; }
            set
            {
                SetItems(value);
                SetProperty(ref selectTypeDisplayItem, value);

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
            SetDisplayTypeItems();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            DisplayTypeItems = null;
        }

        private void SetDisplayTypeItems()
        {
            List<Entity> items = GetDisplayTypeItems();
            DisplayTypeItems = items;
            if (items.Count > 0)
                SelectTypeDisplayItem = items[0];
        }

        private void SetItems(Entity untappdEntity)
        {
            Items = null;

            if (untappdEntity == null)
                return;

            switch (untappdEntity.Id)
            {
                case (long)UntappdEntity.Checkin:
                    Items = GetCheckinItems();
                    break;
                case (long)UntappdEntity.Beer:
                    Items = GetBeerItems();
                    break;
            }
        }

        private IEnumerable GetCheckinItems()
        {
            List<RatingViewModel> viewModels = new List<RatingViewModel>();
            foreach (Checkin checkin in untappdService.GetCheckins())
            {
                string photoPath = untappdService.GetCheckinPhotoFilePath(checkin);
                viewModels.Add(ConverterHelper.GetCheckinViewModel(checkin, photoPath));
            }
            return viewModels;
        }

        private IEnumerable GetBeerItems()
        {
            List<RatingViewModel> viewModels = new List<RatingViewModel>();
            foreach (Beer beer in untappdService.GetBeers())
            {
                string photoPath = untappdService.GetBeerLabelFilePath(beer);
                viewModels.Add(ConverterHelper.GetBeerViewModel(beer, photoPath));
            }
            return viewModels;
        }

        private List<Entity> GetDisplayTypeItems()
        {
            List<Entity> typeItems = new List<Entity>();
            typeItems.Add(EnumsHelper.GetntappdEntity(UntappdEntity.Checkin));
            typeItems.Add(EnumsHelper.GetntappdEntity(UntappdEntity.Beer));
            typeItems.Add(EnumsHelper.GetntappdEntity(UntappdEntity.Brewery));
            return typeItems;
        }

        private void Exit()
        {
            moduleManager.LoadModule(typeof(UntappdModule).Name);
            ActivateView(RegionNames.MainRegion, typeof(Untappd));
        }
    }
}