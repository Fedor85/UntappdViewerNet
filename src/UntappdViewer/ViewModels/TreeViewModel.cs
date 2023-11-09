using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Models;
using UntappdViewer.Modules;

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
   
        public string TreeViewCaption
        {
            get { return treeViewCaption; }
            set { SetProperty(ref treeViewCaption, value); }
        }

        public string Search
        {
            get { return search; }
            set { SetProperty(ref search, value); }
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

            TreeItems = new ObservableCollection<TreeItemViewModel>();
        }


        protected override void Activate()
        {
            base.Activate();
            eventAggregator.GetEvent<RequestCheckinsEvent>().Subscribe(ReturnVisibleChekins);
            eventAggregator.GetEvent<ChekinOffsetEvent>().Subscribe(ChekinOffset);

            IsCheckedUniqueCheckBox = settingService.IsCheckedUniqueCheckBox();

            LoadTreeItems();
            PropertyChanged += TreeViewModelPropertyChanged;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            eventAggregator.GetEvent<RequestCheckinsEvent>().Unsubscribe(ReturnVisibleChekins);
            eventAggregator.GetEvent<ChekinOffsetEvent>().Unsubscribe(ChekinOffset);
            PropertyChanged -= TreeViewModelPropertyChanged;

            SaveSettings();
            TreeItems.Clear();
            Search = String.Empty;

            DeActivateAllViews(RegionNames.ContentRegion);
        }

        private void LoadTreeItems()
        {
            LoadTreeItemsAsync();
        }

        private async void LoadTreeItemsAsync()
        {
            TreeItems = await Task.Run(() => GetTreeItems());
            ApplyFilter();
            if (TreeItems.Count > 0)
                UpdateSelectedTreeItem(settingService.GetSelectedTreeItemId());

            UpdateTreeViewCaption();
        }

        private ObservableCollection<TreeItemViewModel> GetTreeItems()
        {
            ObservableCollection<TreeItemViewModel> treeViewItems = new ObservableCollection<TreeItemViewModel>();
            int treeItemNameMaxLength = settingService.GetTreeItemNameMaxLength();
            List<Checkin> uniqueCheckins = untappdService.GetCheckins(true);
            List<Checkin> checkins = untappdService.GetCheckins();
            for (int i = 0; i < checkins.Count; i++)
            {
                Checkin checkin = checkins[i];
                TreeItemViewModel treeItem = new TreeItemViewModel(checkin.Id, untappdService.GetTreeViewCheckinDisplayName(checkin, i + 1, treeItemNameMaxLength));
                treeItem.IsUniqueCheckin = uniqueCheckins.Contains(checkin);
                treeViewItems.Add(treeItem);
            }
            return treeViewItems;
        }

        private void TreeViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsCheckedUniqueCheckBox") || e.PropertyName.Equals("Search"))
                ApplyFilterAsync();
        }

        private void ApplyFilter()
        {
            if (TreeItems == null)
                return;

            List<TreeItemViewModel> viewModels = new List<TreeItemViewModel>(TreeItems);
            if (IsCheckedUniqueCheckBox)
                viewModels.RemoveAll(item => !item.IsUniqueCheckin);

            if (!String.IsNullOrEmpty(Search))
                viewModels.RemoveAll(item => !item.NameToLower.Contains(Search.ToLower()));

            foreach (TreeItemViewModel itemView in TreeItems.Where(item => viewModels.Contains(item) && !item.Visibility))
                itemView.Visible();

            foreach (TreeItemViewModel itemView in TreeItems.Where(item => !viewModels.Contains(item) && item.Visibility))
                itemView.Hide();
        }

        private async void ApplyFilterAsync()
        {
            await Task.Run(() => ApplyFilter());
            UpdateTreeViewCaption();
        }

        private void ChekinOffset(int offset)
        {
            if(SelectedTreeItem == null)
                return;

            List<TreeItemViewModel> visibilityTreeItemViewModels = TreeItems.Where(item => item.Visibility).ToList();
            int index = visibilityTreeItemViewModels.IndexOf(SelectedTreeItem);
            int newIndex = index + offset;
            if (newIndex < 0 || newIndex > visibilityTreeItemViewModels.Count -1)
                return;

            SelectedTreeItem = visibilityTreeItemViewModels[newIndex];
        }

        private void ReturnVisibleChekins(CallBackConteiner<List<long>> callBackConteiner)
        {
            callBackConteiner.Content = new List<long>();
            callBackConteiner.Content.AddRange(TreeItems.Where(item => item.Visibility).Select(item => item.Id).ToList());
        }

        private void UpdateContent()
        {
            if (SelectedTreeItem == null)
                return;

            moduleManager.LoadModule(typeof(CheckinModule).Name);
            ActivateView(RegionNames.ContentRegion, typeof(Views.Checkin));
            eventAggregator.GetEvent<ChekinUpdateEvent>().Publish(untappdService.GetCheckin(SelectedTreeItem.Id));
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

        private void SaveSettings()
        {
            settingService.SetIsCheckedUniqueCheckBox(IsCheckedUniqueCheckBox);
            if (SelectedTreeItem != null)
                settingService.SetSelectedTreeItemId(SelectedTreeItem.Id);
        }
    }
}