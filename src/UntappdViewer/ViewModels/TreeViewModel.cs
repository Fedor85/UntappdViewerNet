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
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;
using UntappdViewer.UI.Controls.ViewModel;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : LoadingBaseModel
    {
        private IUntappdService untappdService;

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
            set { SetProperty(ref treeViewCaption, value); }
        }

        public string Search
        {
            get { return search; }
            set
            {
                SetProperty(ref search, value);
                AppFilter(value);
            }
        }

        public bool IsCheckedUniqueCheckBox
        {
            get { return isCheckedUniqueCheckBox; }
            set { SetProperty(ref isCheckedUniqueCheckBox, value); }
        }

        public ObservableCollection<TreeItemViewModel> TreeItems
        {
            get { return treeItems; }
            set { SetProperty(ref treeItems, value); }
        }

        public TreeItemViewModel SelectedTreeItem
        {
            get { return selectedTreeItem; }
            set
            {
                SetProperty(ref selectedTreeItem, value);
                UpdateContent();
            }
        }

        public TreeViewModel(IUntappdService untappdService, IModuleManager moduleManager,
                                                             IRegionManager regionManager,
                                                             IEventAggregator eventAggregator,
                                                             ISettingService settingService) : base(moduleManager, regionManager, eventAggregator)
        {
            this.untappdService = untappdService;
            this.eventAggregator = eventAggregator;
            this.settingService = settingService;

            UniqueCheckedCommand = new DelegateCommand<bool?>(UniqueChecked);
            TreeItems = new ObservableCollection<TreeItemViewModel>();
        }

        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<RequestCheckinsEvent>().Subscribe(ReturnVisibleChekins);
            IsCheckedUniqueCheckBox = settingService.IsCheckedUniqueCheckBox();
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

        private void ReturnVisibleChekins(CallBackConteiner<List<long>> callBackConteiner)
        {
            callBackConteiner.Content = new List<long>();
            foreach (TreeItemViewModel treeItemViewModel in TreeItems.Where(item => item.Visibility))
                callBackConteiner.Content.Add(treeItemViewModel.Id);
        }

        private void UpdateContent()
        {
            if (SelectedTreeItem == null)
                return;

            moduleManager.LoadModule(typeof(CheckinModule).Name);
            ActivateView(RegionNames.ContentRegion, typeof(Views.Checkin));
            eventAggregator.GetEvent<ChekinUpdateEvent>().Publish(untappdService.GetCheckin(SelectedTreeItem.Id));
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
            int treeItemNameMaxLength =  settingService.GetTreeItemNameMaxLength();
            List<Checkin> checkins = untappdService.GetCheckins(isUniqueCheckins);
            for (int i = 0; i < checkins.Count; i++)
            {
                Checkin checkin = checkins[i];
                treeViewItems.Add(new TreeItemViewModel(checkin.Id, untappdService.GetTreeViewCheckinDisplayName(checkin, i + 1, treeItemNameMaxLength)));
            }
            return treeViewItems;
        }

        private void UpdateTreeViewCaption()
        {
            TreeViewCaption = $"{Properties.Resources.Checkins} ({TreeItems.Count(item => item.Visibility)}):";
        }

        private void UpdateSelectedTreeItem(long? selectedTreeItemId)
        {
            TreeItemViewModel findSelectedTreeItem = null;
            if (selectedTreeItemId.HasValue)
                findSelectedTreeItem = TreeItems.FirstOrDefault(item => item.Id.Equals(selectedTreeItemId.Value) && item.Visibility);

            if (findSelectedTreeItem == null)
                findSelectedTreeItem = TreeItems.FirstOrDefault(item => item.Visibility);

            SelectedTreeItem = findSelectedTreeItem;
        }

        private void AppFilter(string filter)
        {
            AppFilterAsync(filter);
        }

        private async void AppFilterAsync(string filter)
        {
            await Task.Run(() => SetFilter(filter));
        }

        private void SetFilter(string filter)
        {
            if (String.IsNullOrEmpty(filter) || String.IsNullOrEmpty(filter.Trim()))
            {
                foreach (TreeItemViewModel model in TreeItems.Where(item => !item.Visibility))
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
            UpdateTreeViewCaption();
        }

        private void SaveSettings()
        {
            settingService.SetIsCheckedUniqueCheckBox(IsCheckedUniqueCheckBox);
            if (SelectedTreeItem != null)
                settingService.SetSelectedTreeItemId(SelectedTreeItem.Id);
        }
    }
}