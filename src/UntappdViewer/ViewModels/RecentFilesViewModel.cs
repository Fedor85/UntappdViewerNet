using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using UntappdViewer.Events;
using UntappdViewer.Infrastructure;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.ViewModels
{
    public class RecentFilesViewModel : ActiveAwareBaseModel
    {
        private ISettingService settingService;

        private IEventAggregator eventAggregator;

        private List<FileItem> fileItems;

        public ICommand OpenRecentFileCommand { get; }

        public ICommand DeleteRecentFileByListCommand { get; }

        public List<FileItem> FileItems
        {
            get { return fileItems; }
            set
            {
                SetProperty(ref fileItems, value);
            }
        }

        public RecentFilesViewModel(ISettingService settingService, IEventAggregator eventAggregator)
        {
            this.settingService = settingService;
            this.eventAggregator = eventAggregator;

            OpenRecentFileCommand = new DelegateCommand<string>(OpenRecentFile);
            DeleteRecentFileByListCommand = new DelegateCommand<string>(DeleteRecentFileByList);
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

        private void OpenRecentFile(string filePath)
        {
            FileHelper.AddFile(FileItems, filePath, settingService.GetMaxRecentFilePaths());
            eventAggregator.GetEvent<OpenFileEvent>().Publish(filePath);
        }

        private void DeleteRecentFileByList(string filePath)
        {
            FileHelper.RemoveFilePath(FileItems, filePath);
            settingService.SetRecentFilePaths(FileHelper.GetMergedFilePaths(FileItems));
            FileItems = FileHelper.GetExistsParseFilePaths(settingService.GetRecentFilePaths());
        }
    }
}