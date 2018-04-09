using CAP.SQLToMongoMigrator.Model;
using MongoDB.Driver;
using System.Linq;

namespace CAP.SQLToMongoMigrator.Source
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var mongoClient = CreateMongoClient();
            using(var sqlClient = new SqlServerLogsEntities())
            {
                var logEntries = sqlClient.CustomsApprovalLogs
                    .OrderBy((c) => c.Id)
                    .Take(10)
                    .Select((c) =>
                        new LogEntry
                        {
                            LogId = c.Id,
                            Date = c.Date,
                            Machine = c.Machine,
                            Thread = c.Thread,
                            Level = c.Level,
                            Logger = c.Logger,
                            Message = c.Message,
                            Exception = c.Exception
                        }).ToList();

                mongoClient.GetDatabase("zoll_logs").GetCollection<LogEntry>("logs").InsertMany(logEntries);
            }
        }

        private static MongoClient CreateMongoClient()
        {
            var connectionString = "mongodb://localhost";
            return new MongoClient(connectionString);
        }
    }
}
