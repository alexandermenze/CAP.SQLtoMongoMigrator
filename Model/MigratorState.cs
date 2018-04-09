using MongoDB.Driver;

namespace CAP.SQLToMongoMigrator.Model
{
    internal class MigratorState
    {
        public SqlServerLogsEntities SqlClient { get; set; }
        public MongoClient MongoClient { get; set; }
        public IntRange IdRange { get; set; }
    }
}
