using System.Collections.Generic;
using UntappdViewer.Different;
using UntappdViewer.Models;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : ActiveAwareBaseModel
    {
        private UntappdService untappdService;

        private List<TreeViewItem> treeItems;

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

        private void UpdateTree()
        {
            TreeItems = untappdService.GeTreeViewItems();
        }
    }
}