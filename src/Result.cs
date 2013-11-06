using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthScope
{
    public class Result
    {
        public string HealthCheckName { get; set; }

        public string HealthCheckDescription { get; set; }

        public long TimeTakenInTicks { get; set; }

        public bool Passed { get; set; }

        public NameValueCollection Data { get; set; }
        public DateTime? FailingSince { get; set; }
        public int FailureCount { get; set; }
    }
}
