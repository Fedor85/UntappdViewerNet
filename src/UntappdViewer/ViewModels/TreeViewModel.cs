using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain.Services;
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : LoadingBaseModel
    {
        private UntappdService untappdService;

        private IEventAggregator eventAggregator;

        private ISettingService settingService;

        private ObservableCollection<TreeItemViewModel> treeItems;

        private TreeItemViewModel selectedTreeItem;

        private string treeViewCaption;

        private string search;

        private bool isCheckedUniqueCheckBox;

        public ICommand UniqueCheckedCommand { get; }
   
        public string TreeViewCaption
        {
            get { return treeViewCaption; }
            set
            {
                treeViewCaption = value;
                OnPropertyChanged();
            }
        }

        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                AppFilter(value);
                UpdateTreeViewCaption();
                OnPropertyChanged();
            }
        }

        public bool IsCheckedUniqueCheckBox
        {
            get { return isCheckedUniqueCheckBox; }
            set
            {
                isCheckedUniqueCheckBox = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TreeItemViewModel> TreeItems
        {
            get { return treeItems; }
            set
            {
                treeItems = value;
                OnPropertyChanged();
            }
        }

        public TreeItemViewModel SelectedTreeItem
        {
            get { return selectedTreeItem; }
            set
            {
                selectedTreeItem = value;
                OnPropertyChanged();
                UpdateContent();
            }
        }

        public TreeViewModel(UntappdService untappdService, IModuleManager moduleManager,
                                                            IRegionManager regionManager,
                                                            IEventAggregator eventAggregator,
                                                            ISettingService settingService) : base(moduleManager, regionManager)
        {
            this.untappdService = untappdService;
            this.eventAggregator = eventAggregator;
            this.settingService = settingService;

            loadingModuleName = typeof(LoadingModule).Name;
            loadingRegionName = RegionNames.LoadingRegion;

            UniqueCheckedCommand = new DelegateCommand<bool?>(UniqueChecked);
            TreeItems = new ObservableCollection<TreeItemViewModel>();
        }

        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<RequestCheckinsEvent>().Subscribe(ReturnVisibleChekins);
            IsCheckedUniqueCheckBox = settingService.GetIsCheckedUniqueCheckBox();
            UpdateTree(IsCheckedUniqueCheckBox, settingService.GetSelectedTreeItemId());
        }



        protected override void DeActivate()
        {
            base.DeActivate();
            eventAggregator.GetEvent<RequestCheckinsEvent>().Unsubscribe(ReturnVisibleChekins);
            SaveSettings();
            DeActivateAllViews(RegionNames.ContentRegion);
            TreeItems.Clear();
            Search =String.Empty;
        }

        private void ReturnVisibleChekins(CallBackConteiner<List<Checkin>> callBackConteiner)
        {
            callBackConteiner.Content = new List<Checkin>();
            foreach (TreeItemViewModel treeItemViewModel in TreeItems.Where(item => !item.IsHidden()))
                callBackConteiner.Content.Add(untappdService.GetCheckin(treeItemViewModel.Id));
        }

        private void UpdateContent()
        {
            moduleManager.LoadModule(typeof(CheckinModule).Name);
            ActivateView(RegionNames.ContentRegion, typeof(Views.Checkin));

            eventAggregator.GetEvent<ChekinUpdateEvent>().Publish(SelectedTreeItem != null ? untappdService.GetCheckin(SelectedTreeItem.Id) : null);
        }

        private void UniqueChecked(bool? isChecked)
        {
            if (isChecked.HasValue)
                UpdateTree(isChecked.Value, SelectedTreeItem?.Id);
        }

        private void UpdateTree(bool isUniqueCheckins, long? selectedTreeItemId)
        {
            LoadingChangeActivity(true);
            UpdateTreeAsync(isUniqueCheckins, selectedTreeItemId);
        }

        private async void UpdateTreeAsync(bool isUniqueCheckins, long? selectedTreeItemId)
        {
            TreeItems = await Task.Run(() => GeTreeViewItems(isUniqueCheckins));
            AppFilter(Search);
            UpdateTreeViewCaption();

            if (TreeItems.Count > 0)
                UpdateSelectedTreeItem(selectedTreeItemId);

            LoadingChangeActivity(false);
        }

        private ObservableCollection<TreeItemViewModel> GeTreeViewItems(bool isUniqueCheckins)
        {
            ObservableCollection<TreeItemViewModel> treeViewItems = new ObservableCollection<TreeItemViewModel>();
            List<Checkin> checkins = untappdService.GeCheckins(isUniqueCheckins);
            for (int i = 0; i < checkins.Count; i++)
            {
                Checkin checkin = checkins[i];
                treeViewItems.Add(new TreeItemViewModel(checkin.Id, untappdService.GetTreeViewCheckinDisplayName(checkin, i + 1)));
            }
            return treeViewItems;
        }

        private void UpdateTreeViewCaption()
        {
            TreeViewCaption = $"{Properties.Resources.Checkins} ({TreeItems.Count(item => !item.IsHidden())}):";
        }

        private void UpdateSelectedTreeItem(long? selectedTreeItemId)
        {
            TreeItemViewModel findSelectedTreeItem = null;
            if (selectedTreeItemId.HasValue)
                findSelectedTreeItem = TreeItems.FirstOrDefault(item => item.Id.Equals(selectedTreeItemId.Value) && !item.IsHidden());

            if (findSelectedTreeItem == null)
                findSelectedTreeItem = TreeItems.FirstOrDefault(item => !item.IsHidden());

            SelectedTreeItem = findSelectedTreeItem;
        }

        private void AppFilter(string filter)
        {
            if (String.IsNullOrEmpty(filter) || String.IsNullOrEmpty(filter.Trim()))
            {
                foreach (TreeItemViewModel model in TreeItems)
                    model.Visible();
            }
            else
            {
                string lowerFilter = filter.ToLower();
                foreach (TreeItemViewModel model in TreeItems)
                {
                    if (!model.Name.ToLower().Contains(lowerFilter))
                        model.Hide();
                    else
                        model.Visible();
                }
            }
        }

        private void SaveSettings()
        {
            settingService.SetIsCheckedUniqueCheckBox(IsCheckedUniqueCheckBox);
            if (SelectedTreeItem != null)
                settingService.SetSelectedTreeItemId(SelectedTreeItem.Id);
        }
    }
}