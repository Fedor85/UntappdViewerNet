using System;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using UntappdViewer.Different;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : ActiveAwareBaseModel
    {
        private UntappdService untappdService;

        private List<TreeViewItem> treeItems;

        public ICommand UniqueCheckedCommand { get; }

        public string treeViewCaption;

        public string TreeViewCaption
        {
            get { return treeViewCaption; }
            set
            {
                treeViewCaption = value;
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

        public TreeViewModel(UntappdService untappdService)
        {
            this.untappdService = untappdService;
            UniqueCheckedCommand = new DelegateCommand<bool?>(UniqueChecked);
            UpdateTree();
        }

        protected override void Activate()
        {
            base.Activate();
            untappdService.UpdateUntappdEvent += UpdateTree;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            untappdService.UpdateUntappdEvent -= UpdateTree;
            TreeItems.Clear();
        }

        private void UniqueChecked(bool? isChecked)
        {
        }

        private void UpdateTree()
        {
            TreeItems = untappdService.GeTreeViewItems();
            TreeViewCaption = String.Format("{0} ({1}):", Properties.Resources.Checkins, TreeItems.Count);
        }
    }
}