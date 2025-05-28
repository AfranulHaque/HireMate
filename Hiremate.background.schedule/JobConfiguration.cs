using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiremate.Background.Scheduler
{
    public class JobConfiguration
    {
        public string JobName { get; set; }
        public string JobType { get; set; }
        public string CronExpression { get; set; }
        public int? IntervalInSeconds { get; set; }
        public bool Enabled { get; set; }
    }
}
