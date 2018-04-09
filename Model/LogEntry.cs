using System;

namespace CAP.SQLToMongoMigrator.Model
{
    internal class LogEntry
    {
        private int LogId { get; set; }
        private DateTime Date { get; set; }
        private string Machine { get; set; }
        private string Thread { get; set; }
        private string Level { get; set; }
        private string Logger { get; set; }
        private string Message { get; set; }
        private string Exception { get; set; }
    }
}
