using UntappdViewer.Infrastructure.Properties;
using UntappdViewer.Interfaces.Services;

namespace UntappdViewer.Infrastructure.Services
{
    public class SettingService: ISettingService
    {
        public void Reset()
        {
            Settings.Default.Reset();
        }

        public string GetRecentFilePaths()
        {
            return Settings.Default.RecentFilePaths;
        }

        public void SetRecentFilePaths(string value)
        {
            Settings.Default.RecentFilePaths = value;
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

        public string GetDefaultUserName()
        {
            return Settings.Default.DefaultUserName;
        }

        public int GetTreeItemNameMaxLength()
        {
            return Settings.Default.TreeItemNameMaxLength;
        }

        public int GetMaxRecentFilePaths()
        {
            return Settings.Default.MaxRecentFilePaths;
        }

        public bool GetIsCheckedUniqueCheckBox()
        {
            return Settings.Default.IsCheckedUniqueCheckBox;
        }

        public void SetIsCheckedUniqueCheckBox(bool isChecked)
        {
            Settings.Default.IsCheckedUniqueCheckBox = isChecked;
            Settings.Default.Save();
        }
    }
}