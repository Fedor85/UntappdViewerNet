using LiteDB;

namespace UntappdViewer.Infrastructure.Services.DataBase
{
    public interface IDbContext
    {
        LiteDatabase Database { get; }
    }
}