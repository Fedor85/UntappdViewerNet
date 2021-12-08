using Prism.Mvvm;
using UntappdViewer.Helpers;

namespace UntappdViewer.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        string version;

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