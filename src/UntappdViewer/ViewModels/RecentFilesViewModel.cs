using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;

namespace UntappdViewer.ViewModels
{
    public class RecentFilesViewModel : ActiveAwareBaseModel
    {
        public ICommand OpenRecentFileCommand { get; }

        public List<FileItem> FileItems { get; set; }

        public RecentFilesViewModel()
        {
            OpenRecentFileCommand = new DelegateCommand<FileItem>(OpenRecentFile);
        }

        private void OpenRecentFile(FileItem entity)
        {

        }

        protected override void Activate()
        {
            base.Activate();
        }

        protected override void DeActivate()
        {
            base.DeActivate();
        }
    }
}