using System;
using LiteDB;
using TheDwarvenHalls.Shared.Models;

namespace TheDwarvenHalls.Server.Database
{
    public class Context : IDisposable
    {
        public const string DatabasePath = "Database.db";

        private readonly LiteDatabase _database;

        public Context()
        {
            _database = new LiteDatabase(DatabasePath);
        }

        public LiteCollection<User> Users => _database.GetCollection<User>("users");
        public LiteCollection<Session> Session => _database.GetCollection<Session>("sessions");
        
        public void Dispose()
        {
            _database.Dispose();
        }
    }
}