using System.Collections.ObjectModel;
using System.IO;
using Prism.Mvvm;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.Utils;

namespace UntappdViewer.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        private IUntappdService untappdService;

        private ObservableCollection<string> imagePaths;

        string version;

        public string EmailUrl
        {
            get { return StringHelper.GetEmailUrl(Properties.Resources.Email); }
        }

        public ObservableCollection<string> ImagePaths
        {
            get { return imagePaths; }
            set
            {
                SetProperty(ref imagePaths, value);
            }
        }

        public string Version
        {
            get { return version; }
            set
            {
                SetProperty(ref version, value);
            }
        }

        public AboutViewModel(IUntappdService untappdService)
        {
            this.untappdService = untappdService;
            Version = CommunicationHelper.GetTitle();
            if (untappdService.IsUNTPProject())
                ImagePaths = GetImagePaths();
        }

        private ObservableCollection<string> GetImagePaths()
        {
            string directory = untappdService.GetBadgeImageDirectory();
            ObservableCollection<string> imagePaths = new ObservableCollection<string>();
            if (Directory.Exists(directory))
                imagePaths.AddRange(Directory.GetFiles(directory).Shuffle());

            return imagePaths;
        }
    }
}