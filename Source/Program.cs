using CAP.SQLToMongoMigrator.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAP.SQLToMongoMigrator.Source
{
    internal class Program
    { 
        internal static void Main(string[] args)
        {
            using(var sqlClient = new SqlServerLogsEntities())
            {
                var mongoClient = CreateMongoClient();
                int lastId = FindLastId(sqlClient);
                IList<IntRange> sqlServerParts = new List<IntRange>();

                for (int i = 0; i <= lastId; i += 1000)
                {
                    sqlServerParts.Add(new IntRange(i, i + 999));
                }

                RunMigrators(sqlClient, sqlServerParts, mongoClient);
            }
        }

        private static void RunMigrators(SqlServerLogsEntities sqlClient, IList<IntRange> sqlServerParts, MongoClient mongoClient) 
            => Parallel.ForEach(sqlServerParts, (idRange) =>
                {
                    Migrate(new MigratorState { SqlClient = sqlClient, MongoClient = mongoClient, IdRange = idRange });
                });

        private static void Migrate(MigratorState state)
        {

        }

        private static int FindLastId(SqlServerLogsEntities sqlClient)
            => sqlClient.CustomsApprovalLogs
                .OrderByDescending((c) => c.Id)
                .First()
                .Id;

        private static MongoClient CreateMongoClient()
            => new MongoClient("mongodb://localhost");
    }
}
