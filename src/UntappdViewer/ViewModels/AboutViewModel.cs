using System.Collections.Generic;
using System.IO;
using Prism.Mvvm;
using UntappdViewer.Helpers;
using UntappdViewer.Interfaces.Services;
using UntappdViewer.UI.Controls.ViewModel;
using UntappdViewer.Utils;

namespace UntappdViewer.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        private IUntappdService untappdService;

        private List<BaseImageViewModel> imagePaths;

        string version;

        public string EmailUrl
        {
            get { return StringHelper.GetEmailUrl(Properties.Resources.Email); }
        }

        public List<BaseImageViewModel> ImagePaths
        {
            get { return imagePaths; }
            set { SetProperty(ref imagePaths, value); }
        }

        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        public AboutViewModel(IUntappdService untappdService)
        {
            this.untappdService = untappdService;
            Version = CommunicationHelper.GetTitle();
            ImagePaths = GetImageViewModels();
        }

        private List<BaseImageViewModel> GetImageViewModels()
        {
            string directory = untappdService.GetBadgeImageDirectory();
            List<BaseImageViewModel> imageViewModels = new List<BaseImageViewModel>();
            if (!Directory.Exists(directory))
                return imageViewModels;

            foreach (string imagePath in Directory.GetFiles(directory).Shuffle())
            {
                BaseImageViewModel imageViewModel = new BaseImageViewModel();
                imageViewModel.ImagePath = imagePath;
                imageViewModels.Add(imageViewModel);
            }
            return imageViewModels;
        }
    }
}