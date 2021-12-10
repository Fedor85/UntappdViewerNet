using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Prism.Mvvm;
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
            if (untappdService.IsUNTPProject())
                ImagePaths = GetImagePaths();
        }

        private ObservableCollection<string> GetImagePaths()
        {
            string directory = untappdService.GetBadgeImageDirectory();
            return new ObservableCollection<string>(Directory.GetFiles(directory).Shuffle());
        }
    }
}