using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthScope.Configuration;

namespace HealthScope.HealthCheck
{
    public interface ITriggeredHealthCheck : IHealthCheck
    {
        void GetStatus(CurrentContext context);

        void Fail();

        void Fail(string reason);

        void Fail(string reason, Exception ex);
    }
}
