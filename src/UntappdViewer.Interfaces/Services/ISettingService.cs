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
    }
}