using System.Collections.Generic;
using System.ComponentModel;
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

            OpenRecentFileCommand = new DelegateCommand<FileItem>(OpenRecentFile);
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

        private void OpenRecentFile(FileItem entity)
        {
            FileHelper.AddFile(FileItems, entity.FilePath, settingService.GetMaxRecentFilePaths());
            eventAggregator.GetEvent<OpenFileEvent>().Publish(entity.FilePath);
        }
    }
}