using System.Data.Common;

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
            return new SqliteConnection(this._config.GetConnectionString("DefaultConnection"));
        }
    }
}
