using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HealthScope.Configuration;

namespace HealthScope.HealthCheck
{
    public interface IHealthCheck
    {
        string Name { get; set; }

        string Description { get; set; }
    }
}
