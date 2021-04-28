using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CarStocks.Common
{
    public abstract class BaseRepository
    {
        private readonly IConfiguration _config;

        public BaseRepository(IConfiguration config)
        {
            this._config = config;
        }

        public DbConnection GetDbconnection()
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");
            var dataDirectory = _config.GetValue<string>("DataDirectory");

            var db = new SqliteConnection(connectionString.Replace("|DataDirectory|", dataDirectory));

            return db;
        }
    }
}
