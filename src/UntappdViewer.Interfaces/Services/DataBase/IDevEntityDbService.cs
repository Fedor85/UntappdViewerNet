using System.Windows.Media.Imaging;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Interfaces.Services.DataBase
{
    public interface IDevEntityDbService : IEntityDbService<Entity>
    {
        BitmapSource GetBitmapSource(string key);

        void AddBitmapSource(string key, BitmapSource bitmapSource);
    }
}