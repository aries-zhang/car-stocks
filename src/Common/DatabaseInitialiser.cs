using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;

namespace CarStocks.Common
{
    public static class DatabaseInitialiser
    {
        public static void Initialise(IConfiguration config)
        {
            using var db = new SqliteConnection(config.GetConnectionString("DefaultConnection"));

            if (!TableExists(db, "Dealer"))
            {
                db.Execute(@"CREATE TABLE Dealer (
                                Id    INTEGER PRIMARY KEY AUTOINCREMENT,
                                Token STRING
                            );");
                
                SeedDealders(db);
            }

            if (!TableExists(db, "Car"))
            {
                db.Execute(@"CREATE TABLE Car (
                                Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                                DealerId INTEGER,
                                Make     STRING,
                                Model    STRING,
                                Year     INTEGER,
                                Stock    INTEGER
                            );");
            }
        }

        private static bool TableExists(SqliteConnection db, string table)
        {
            return db.Query<int>("select count(name) from sqlite_master where type='table' and name=@table", new { table }).First() > 0;
        }

        private static void SeedDealders(SqliteConnection db)
        {
            db.Execute(@"INSERT INTO Dealer ( Token ) VALUES ( 'abc' );");
            db.Execute(@"INSERT INTO Dealer ( Token ) VALUES ( 'def' );");
        }
    }
}
