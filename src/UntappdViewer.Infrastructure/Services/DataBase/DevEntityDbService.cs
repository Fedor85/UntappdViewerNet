using System.IO;
using System.Windows.Media.Imaging;
using UntappdViewer.Interfaces.Services.DataBase;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Infrastructure.Services.DataBase
{
    public class DevEntityDbService : BaseEntityDbService<Entity>, IDevEntityDbService
    {
        public DevEntityDbService(IDbContext dbContext) : base(dbContext, "DevSetting")
        {

        }

        public BitmapSource GetBitmapSource(string key)
        {
            Stream st = GetFileStream(key);
            if (st == null)
                return null;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = st;
            bi.EndInit();
            return bi;
        }

        public void AddBitmapSource(string key, BitmapSource bitmapSource)
        {
            using (Stream memoryStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapSource));
                enc.Save(memoryStream);
                AddFile(key, memoryStream);
            }
        }
    }
}