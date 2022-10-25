using LiteDB;

namespace UntappdViewer.Infrastructure.Services.DataBase
{
    public class DbContext : IDbContext
    {
        public LiteDatabase Database { get; }

        public DbContext(string databaseName)
        {
            Database = new LiteDatabase($"{databaseName}.db");
        }
    }
}