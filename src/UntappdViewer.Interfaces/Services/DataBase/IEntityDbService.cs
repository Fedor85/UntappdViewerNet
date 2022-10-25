using System.IO;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Interfaces.Services.DataBase
{
    public interface IEntityDbService<T> where T : Entity
    {
        void Add<TV>(string key, TV value);

        void Add(T item);

        void AddFile(string key, string filePath);

        void AddFile(string Key, Stream fileStream);

        TV GetValue<TV>(string key);

        T Get(string key);

        Stream GetFileStream(string key);
    }
}