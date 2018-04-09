using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAP.SQLToMongoMigrator.Model
{
    internal struct IntRange
    {
        public int Min { get; }
        public int Max { get; }

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
