using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
            return new SqlConnection(this._config.GetConnectionString("DefaultConnection"));
        }
    }
}
