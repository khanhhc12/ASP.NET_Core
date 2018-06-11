using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

namespace HelloDapper
{
    public static class SimpleDapper
    {
        private static string connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = "App_Data\\sqlite.db"
        }.ToString();

        public static void Select()
        {
            string sql = "SELECT * FROM demo";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var list = connection.Query(sql, commandTimeout: 300).ToList();
                Console.WriteLine(JsonConvert.SerializeObject(list));
            }
        }

        public static void Insert()
        {
            var random = new Random();
            string sql = "INSERT INTO demo(name, hint)VALUES(@name, @hint);";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute(
                    sql,
                    new[]
                    {
                        new { name = string.Format("Name {0:00}", random.Next(1, 99)), hint = string.Format("Hint {0:00}", random.Next(1, 99)) },
                        new { name = string.Format("Name {0:00}", random.Next(1, 99)), hint = string.Format("Hint {0:00}", random.Next(1, 99)) }
                    },
                    commandType: CommandType.Text
                );
                Console.WriteLine(affectedRows);
            }
        }

        public static void Delete()
        {
            string sql = "DELETE FROM demo WHERE name LIKE 'Name __';";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute(sql);
                Console.WriteLine(affectedRows);
            }
        }
    }
}
