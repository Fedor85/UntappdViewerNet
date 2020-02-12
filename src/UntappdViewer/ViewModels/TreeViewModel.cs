using System.Collections.Generic;
using UntappdViewer.Models;
using UntappdViewer.Services;

namespace UntappdViewer.ViewModels
{
    public class TreeViewModel : ActiveAwareBaseModel
    {
        private UntappdService untappdService;

        private List<Checkin> treeItems;

        public List<Checkin> TreeItems
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
            UpdateTree(untappdService.Untappd);
        }

        protected override void Activate()
        {
            base.Activate();
            untappdService.UpdateUntappd += UpdateTree;
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            untappdService.UpdateUntappd -= UpdateTree;
            TreeItems.Clear();
        }

        private void UpdateTree(Untappd untappd)
        {
            TreeItems = untappd.Checkins;
        }
    }
}