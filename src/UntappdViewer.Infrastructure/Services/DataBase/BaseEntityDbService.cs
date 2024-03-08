using System;
using System.IO;
using LiteDB;
using UntappdViewer.Interfaces.Services.DataBase;
using UntappdViewer.Models.Different;

namespace UntappdViewer.Infrastructure.Services.DataBase
{
    public abstract class BaseEntityDbService<T> : IEntityDbService<T> where T : Entity
    {
        private LiteDatabase database;

        protected ILiteCollection<T> collection;

        protected BaseEntityDbService(IDbContext dbContext, string collectionName)
        {
            database = dbContext.Database;
            collection = database.GetCollection<T>(collectionName);
        }

        public void Add<TV>(string key, TV value)
        {
            T entity = GetObject<T>(key, value);
            Add(entity);
        }

        public void Add(T item)
        {
            T keyValue = collection.FindOne(i => i.Key.Equals(item.Key));
            if (keyValue == null)
            {
                collection.Insert(item);
            }
            else
            {
                keyValue.Value = item.Value;
                collection.Update(keyValue);
            }

            database.Commit();
        }

        public void AddFile(string key, string filePath)
        {
            using (Stream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                AddFile(key, fileStream);
        }

        public void AddFile(string Key, Stream fileStream)
        {
            LiteFileInfo<string> file = database.FileStorage.FindById(Key);
            if (file != null)
                database.FileStorage.Delete(Key);

            database.FileStorage.Upload(Key, Key, fileStream);
            database.Commit();
        }

        public TV GetValue<TV>(string key)
        {
            T entity = collection.FindOne(item => item.Key.Equals(key));
            return (TV)entity?.Value;
        }

        public T Get(string key)
        {
            return collection.FindOne(item => item.Key.Equals(key));
        }

        public Stream GetFileStream(string key)
        {
            LiteFileInfo<string> file = database.FileStorage.FindById(key);
            if (file == null)
                return null;
            byte[] bytes = { };
            using (MemoryStream stream = new MemoryStream())
            {
                database.FileStorage.Download(key, stream);
                bytes = stream.ToArray();
            }

            return bytes.Length == 0 ? null : new MemoryStream(bytes);
        }

        private TE GetObject<TE>(params object[] args)
        {
            return (TE)Activator.CreateInstance(typeof(TE), args);
        }
    }
}