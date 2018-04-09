using MongoDB.Driver;
using System;

namespace CAP.SQLToMongoMigrator.Model
{
    internal class MigratorState
    {
        public Func<SqlServerLogsEntities> SqlClientFactory { get; set; }
        public Func<MongoClient> MongoClientFactory { get; set; }
        public IntRange IdRange { get; set; }
    }
}
