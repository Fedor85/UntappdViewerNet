using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class RecentFilesViewModel : ActiveAwareBaseModel
    {
        private ISettingService settingService;

        public ICommand OpenRecentFileCommand { get; }

        private List<FileItem> fileItems;

        public List<FileItem> FileItems
        {
            get { return fileItems; }
            set
            {
                fileItems = value;
                OnPropertyChanged();
            }
        }

        public RecentFilesViewModel(ISettingService settingService)
        {
            this.settingService = settingService;
            OpenRecentFileCommand = new DelegateCommand<FileItem>(OpenRecentFile);
        }

        private void OpenRecentFile(FileItem entity)
        {

        }

        protected override void Activate()
        {
            base.Activate();
            FileItems = FileHelper.GetExistsParseFilePaths(settingService.GetRecentFilePaths());
        }

        protected override void DeActivate()
        {
            base.DeActivate();
            FileItems.Clear();
        }
    }
}