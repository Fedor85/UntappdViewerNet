using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using UntappdViewer.Domain.Services;
using UntappdViewer.Events;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Modules;
using UntappdViewer.Views;

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

            UniqueCheckedCommand = new DelegateCommand<bool?>(UniqueChecked);

            TreeItems = new ObservableCollection<TreeItemViewModel>();
        }

        protected override void Activate()
        {
            base.Activate();
            IsCheckedUniqueCheckBox = settingService.GetIsCheckedUniqueCheckBox();
            UpdateTree(IsCheckedUniqueCheckBox, settingService.GetSelectedTreeItemId());
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            SaveSettings();
            DeActivateAllViews(RegionNames.ContentRegion);
            TreeItems.Clear();
        }

        private void UpdateContent()
        {
            moduleManager.LoadModule(typeof(CheckinModule).Name);
            ActivateView(RegionNames.ContentRegion, typeof(Checkin));

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
            TreeViewCaption = $"{Properties.Resources.Checkins} ({TreeItems.Count}):";

            LoadingChangeActivity(false);
            if (TreeItems.Count == 0)
                return;

            TreeItemViewModel findSelectedTreeItem = null;
            if (selectedTreeItemId.HasValue)
                findSelectedTreeItem = TreeItems.FirstOrDefault(item => item.Id.Equals(selectedTreeItemId.Value));

            SelectedTreeItem = findSelectedTreeItem ?? TreeItems[0];
  
        }

        private ObservableCollection<TreeItemViewModel> GeTreeViewItems(bool isUniqueCheckins)
        {
            ObservableCollection<TreeItemViewModel> treeViewItems = new ObservableCollection<TreeItemViewModel>();
            foreach (Models.Checkin checkin in untappdService.GeCheckins(isUniqueCheckins))
                treeViewItems.Add(new TreeItemViewModel(checkin.Id, untappdService.GetTreeViewCheckinDisplayName(checkin)));

            return treeViewItems;
        }

        private void AppFilter(string filter)
        {
            if (String.IsNullOrEmpty(filter))
            {
                foreach (TreeItemViewModel model in TreeItems)
                    model.Visibility = Visibility.Visible;
            }
            else
            {
                foreach (TreeItemViewModel model in TreeItems)
                {
                    if (!model.Name.Contains(filter))
                        model.Visibility = Visibility.Collapsed;
                    else
                        model.Visibility = Visibility.Visible;
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