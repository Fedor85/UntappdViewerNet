using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : ActiveAwareBaseModel
    {
        private UntappdService untappdService;

        private ISettingService settingService;

        private List<TreeViewItem> treeItems;

        private TreeViewItem selectedTreeItem;

        private string treeViewCaption;

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

        public bool IsCheckedUniqueCheckBox
        {
            get { return isCheckedUniqueCheckBox; }
            set
            {
                isCheckedUniqueCheckBox = value;
                OnPropertyChanged();
            }
        }

        public List<TreeViewItem> TreeItems
        {
            get { return treeItems; }
            set
            {
                treeItems = value;
                OnPropertyChanged();
            }
        }

        public TreeViewItem SelectedTreeItem
        {
            get { return selectedTreeItem; }
            set
            {
                selectedTreeItem = value;
                OnPropertyChanged();
            }
        }

        public TreeViewModel(UntappdService untappdService, ISettingService settingService)
        {
            this.untappdService = untappdService;
            this.settingService = settingService;
            UniqueCheckedCommand = new DelegateCommand<bool?>(UniqueChecked);
            TreeItems = new List<TreeViewItem>();
        }

        protected override void Activate()
        {
            base.Activate();
            IsCheckedUniqueCheckBox = settingService.GetIsCheckedUniqueCheckBox();
            untappdService.UpdateUntappdEvent += UpdateTree;
            UpdateTree();
            SetSelectedTreeItem();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            untappdService.UpdateUntappdEvent -= UpdateTree;
            settingService.SetIsCheckedUniqueCheckBox(IsCheckedUniqueCheckBox);
            if (SelectedTreeItem != null)
                settingService.SetSelectedTreeItemId(SelectedTreeItem.Id);

            TreeItems.Clear();
        }

        private void UniqueChecked(bool? isChecked)
        {
            if (isChecked.HasValue)
                UpdateTree(isChecked.Value);
        }

        private void UpdateTree()
        {
            UpdateTree(IsCheckedUniqueCheckBox);
        }

        private void UpdateTree(bool isUniqueCheckins)
        {
            TreeItems = untappdService.GeTreeViewItems(isUniqueCheckins);
            TreeViewCaption = $"{Properties.Resources.Checkins} ({TreeItems.Count}):";
        }

        private void SetSelectedTreeItem()
        {
            long selectedTreeItemId = settingService.GetSelectedTreeItemId();
            TreeViewItem selectedTreeItem = TreeItems.FirstOrDefault(item => item.Id.Equals(selectedTreeItemId));
            if (selectedTreeItem != null)
                SelectedTreeItem = selectedTreeItem;
        }
    }
}