﻿using UntappdViewer.Infrastructure.Properties;
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

        public bool IsCheckedUniqueCheckBox()
        {
            return Settings.Default.IsCheckedUniqueCheckBox;
        }

        public void SetIsCheckedUniqueCheckBox(bool isChecked)
        {
            Settings.Default.IsCheckedUniqueCheckBox = isChecked;
            Settings.Default.Save();
        }

        public long GetSelectedTreeItemId()
        {
            return Settings.Default.SelectedTreeItemId;
        }

        public void SetSelectedTreeItemId(long itemId)
        {
            Settings.Default.SelectedTreeItemId = itemId;
            Settings.Default.Save();
        }

        public bool IsCheckedSaveAccessToken()
        {
            return Settings.Default.IsCheckedSaveAccessToken;
        }

        public void SetIsCheckedSaveAccessToken(bool isChecked)
        {
            Settings.Default.IsCheckedSaveAccessToken = isChecked;
            Settings.Default.Save();
        }

        public string GetAccessToken()
        {
            return Settings.Default.AccessToken;
        }

        public void SetAccessToken(string accessToken)
        {
            Settings.Default.AccessToken = accessToken;
            Settings.Default.Save();
        }

        public long GetOffsetUpdateBeer()
        {
            return Settings.Default.OffsetUpdateBeer;
        }

        public void SetOffsetUpdateBeer(long offSet)
        {
            Settings.Default.OffsetUpdateBeer = offSet;
            Settings.Default.Save();
        }

        public void SetStartWelcomeView(bool isStart)
        {
            Settings.Default.IsStartWelcomeView = isStart;
            Settings.Default.Save();
        }

        public bool IsStartWelcomeView()
        {
            return Settings.Default.IsStartWelcomeView;
        }
    }
}