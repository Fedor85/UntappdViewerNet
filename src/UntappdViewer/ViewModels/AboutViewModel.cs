using Prism.Mvvm;
using UntappdViewer.Helpers;
using UntappdViewer.Utils;

namespace UntappdViewer.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        string version;

        public string EmailUrl
        {
            get { return StringHelper.GetEmailUrl(Properties.Resources.Email); }
        }

        public string Version
        {
            get { return version; }
            set
            {
                SetProperty(ref version, value);
            }
        }

        public AboutViewModel()
        {
            Version = CommunicationHelper.GetTitle();
        }
    }
}