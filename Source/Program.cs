using CAP.SQLToMongoMigrator.Model;
using MongoDB.Driver;

namespace CAP.SQLToMongoMigrator.Source
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var mongoClient = CreateMongoClient();
            using(var sqlClient = new SqlServerLogsEntities())
            {

            }
        }

        private static MongoClient CreateMongoClient()
        {
            var connectionString = "mongodb://localhost";
            return new MongoClient(connectionString);
        }
    }
}
