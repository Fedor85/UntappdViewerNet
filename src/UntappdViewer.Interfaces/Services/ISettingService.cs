namespace UntappdViewer.Interfaces.Services
{
    public interface ISettingService
    {
        void Reset();

        string GetLastOpenedFilePath();

        void SetLastOpenedFilePath(string value);

        double GetTreeRegionWidth();

        void SetTreeRegionWidth(double value);

        string GetDefaultUserName();

        int GetTreeItemNameMaxLength();
    }
}