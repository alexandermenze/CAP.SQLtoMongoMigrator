using CAP.SQLToMongoMigrator.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAP.SQLToMongoMigrator.Source
{
    internal class Program
    { 
        internal static void Main(string[] args)
        {
            Console.WriteLine("Starting up!");

            int lastId;
            using (var sqlClient = new SqlServerLogsEntities())
                lastId = FindLastId(sqlClient);

            Console.WriteLine($"Last Id is '{ lastId }'");

            IList<IntRange> sqlServerParts = new List<IntRange>();

            for (int i = 0; i <= lastId; i += 1000)
                sqlServerParts.Add(new IntRange(i, i + 999));

            Console.WriteLine($"Entries have been split into '{ sqlServerParts.Count() }' parts!");
            Console.WriteLine("Starting migration!");

            RunMigrators(CreateSqlClient, sqlServerParts, CreateMongoClient);

            Console.WriteLine("Migration finished!");
        }

        private static void RunMigrators(Func<SqlServerLogsEntities> sqlClientFactory, IList<IntRange> sqlServerParts, Func<MongoClient> mongoClientFactory) 
            => Parallel.ForEach(sqlServerParts, (idRange) =>
                {
                    string uniqueMigrationId = Guid.NewGuid().ToString();
                    Console.WriteLine($"[{ uniqueMigrationId }] Migrating ids '{ idRange.Min }' to '{ idRange.Max }'");
                    Migrate(new MigratorState { SqlClientFactory = sqlClientFactory, MongoClientFactory = mongoClientFactory, IdRange = idRange });
                    Console.WriteLine($"[{ uniqueMigrationId }] Migration done!");
                });

        private static void Migrate(MigratorState state)
        {
            using(var sqlClient = state.SqlClientFactory())
            {
                var logEntries = sqlClient.CustomsApprovalLogs
                        .Where((c) => c.Id >= state.IdRange.Min)
                        .Where((c) => c.Id <= state.IdRange.Max)
                        .Select((c) => 
                            new LogEntry {
                                LogId = c.Id,
                                Date = c.Date,
                                Machine = c.Machine,
                                Thread = c.Thread,
                                Level = c.Level,
                                Logger = c.Logger,
                                Message = c.Message,
                                Exception = c.Exception
                            });

                state.MongoClientFactory()
                    .GetDatabase("zoll_logs")
                    .GetCollection<LogEntry>("logs")
                    .InsertMany(logEntries);
            }
        }

        private static int FindLastId(SqlServerLogsEntities sqlClient)
            => sqlClient.CustomsApprovalLogs
                .OrderByDescending((c) => c.Id)
                .First()
                .Id;

        private static SqlServerLogsEntities CreateSqlClient()
            => new SqlServerLogsEntities();

        private static MongoClient CreateMongoClient()
            => new MongoClient("mongodb://localhost");
    }
}
