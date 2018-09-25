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
            var list = Query(connectionString, sql, commandType: CommandType.Text).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        public static void Insert()
        {
            var random = new Random();
            string sql = "INSERT INTO demo(name, hint)VALUES(@name, @hint);";
            var affectedRows = Execute(connectionString, sql,
            new[]
            {
                new { name = string.Format("Name {0:00}", random.Next(1, 99)), hint = string.Format("Hint {0:00}", random.Next(1, 99)) },
                new { name = string.Format("Name {0:00}", random.Next(1, 99)), hint = string.Format("Hint {0:00}", random.Next(1, 99)) }
            },
            commandType: CommandType.Text);
            Console.WriteLine(affectedRows);
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

        public static IEnumerable<dynamic> Query(string connectionString, string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query(sql, param, commandType: commandType, commandTimeout: 300);
            }
        }

        public static IEnumerable<T> Query<T>(string connectionString, string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Query<T>(sql, param, commandType: commandType, commandTimeout: 300);
            }
        }

        public static T QueryFirstOrDefault<T>(string connectionString, string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<T>(sql, param, commandType: commandType, commandTimeout: 300);
            }
        }

        public static int Execute(string connectionString, string sql, object param = null, CommandType commandType = CommandType.StoredProcedure)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                return connection.Execute(sql, param, commandType: commandType);
            }
        }
    }
}
