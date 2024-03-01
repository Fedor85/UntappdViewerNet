﻿namespace UntappdViewer.Interfaces.Services
{
    public interface ISettingService
    {
        void Reset();

        string GetRecentFilePaths();

        void SetRecentFilePaths(string value);

        double GetTreeRegionWidth();

        void SetTreeRegionWidth(double value);

        string GetDefaultUserName();

        int GetTreeItemNameMaxLength();

        int GetMaxRecentFilePaths();

        bool IsCheckedUniqueCheckBox();

        void SetIsCheckedUniqueCheckBox(bool isChecked);

        long GetSelectedTreeItemId();

        void SetSelectedTreeItemId(long itemId);

        string GetAccessToken();

        void SetAccessToken(string accessToken);

        long GetOffsetUpdateBeer();

        void SetOffsetUpdateBeer(long offSet);

        void SetStartWelcomeView(bool isStart);

        bool IsStartWelcomeView();
    }
}