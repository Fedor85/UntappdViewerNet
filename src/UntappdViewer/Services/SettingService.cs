using UntappdViewer.Interfaces.Services;
using UntappdViewer.Properties;

namespace UntappdViewer.Services
{
    public class SettingService: ISettingService
    {
        public void Reset()
        {
            Settings.Default.Reset();
        }

        public string GetLastOpenedFilePath()
        {
            return Settings.Default.LastOpenedFilePath;
        }

        public void SetLastOpenedFilePath(string value)
        {
            Settings.Default.LastOpenedFilePath = value;
            Settings.Default.Save();
        }

        public double GetTreeRegionWidth()
        {
            return Settings.Default.TreeRegionWidth;
        }

        public void SetTreeRegionWidth(double value)
        {
            Settings.Default.TreeRegionWidth = value;
            Settings.Default.Save();
        }
    }
}