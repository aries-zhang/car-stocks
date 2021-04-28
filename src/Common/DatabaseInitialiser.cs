using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System;
using System.IO;

namespace CarStocks.Common
{

    public class DatabaseInitialiser : BaseRepository
    {
        public DatabaseInitialiser(IConfiguration config) : base(config)
        {
        }

        public void Initialise()
        {
            using var db = this.GetDbconnection();

            db.Execute(@"CREATE TABLE IF NOT EXISTS Dealer (
                            Id    INTEGER PRIMARY KEY AUTOINCREMENT,
                            Token STRING  UNIQUE ON CONFLICT FAIL
                        );

                        CREATE UNIQUE INDEX IF NOT EXISTS unique_token ON Dealer (
                            Token
                        );

                        CREATE TABLE IF NOT EXISTS Car (
                            Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                            DealerId INTEGER,
                            Make     STRING,
                            Model    STRING,
                            Year     INTEGER,
                            Stock    INTEGER
                        );

                        INSERT OR IGNORE INTO Dealer ( Token ) VALUES ( 'abc' );
                        INSERT OR IGNORE INTO Dealer ( Token ) VALUES ( 'def' ); 
            ");
        }

        public void Destroy()
        {
            var connectionString = this.GetDbconnection().ConnectionString;

            var segaments = (connectionString.Contains(";") ? connectionString.Split(";") : new string[] { connectionString })
            .Select(s => s.Contains("=") ? s.Split("=") : new string[] { s, string.Empty })
            .ToDictionary(s => s[0], s => s[1]);

            var path = segaments.ContainsKey("Data Source") ? segaments["Data Source"] : string.Empty;
        }
    }
}
