using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Prism.Commands;
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

        private string youTubeVideoId;

        private bool visibilityYouTubeVideoButton;

        private string devYouTubeVideoId;

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

        public string YouTubeVideoId
        {
            get { return youTubeVideoId; }
            set { SetProperty(ref youTubeVideoId, value); }
        }

        public bool VisibilityYouTubeVideoButton
        {
            get { return visibilityYouTubeVideoButton; }
            set { SetProperty(ref visibilityYouTubeVideoButton, value); }
        }

        public ICommand RunYoutubeVideoCommand { get; }

        public AboutViewModel(IUntappdService untappdService)
        {
            this.untappdService = untappdService;
            Version = CommunicationHelper.GetTitle();
            ImagePaths = GetImageViewModels();

            devYouTubeVideoId = "QXjzrG-UKh4";
            VisibilityYouTubeVideoButton = !String.IsNullOrEmpty(devYouTubeVideoId);
            RunYoutubeVideoCommand = new DelegateCommand<bool?>(RunYoutubeVideo);
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

        private void RunYoutubeVideo(bool? isRunPlay)
        {
            YouTubeVideoId = isRunPlay.HasValue && isRunPlay.Value ? devYouTubeVideoId : String.Empty;
        }
    }
}