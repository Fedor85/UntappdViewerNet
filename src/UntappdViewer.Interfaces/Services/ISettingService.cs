namespace UntappdViewer.Interfaces.Services
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

        bool GetIsCheckedUniqueCheckBox();

        void SetIsCheckedUniqueCheckBox(bool isChecked);

        long GetSelectedTreeItemId();

        void SetSelectedTreeItemId(long itemId);

        bool GetIsCheckedSaveAccessToken();

        void SetIsCheckedSaveAccessToken(bool isChecked);

        string GetAccessToken();

        void SetAccessToken(string accessToken);

        long GetOffsetUpdateBeer();

        void SetOffsetUpdateBeer(long offSet);

        void SetStartWelcomeView(bool isStart);

        bool IsStartWelcomeView();
    }
}