using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthScope.Configuration;
using HealthScope.HealthCheck;

namespace HealthScope
{
    public class Runner
    {
        public Result[] RunHealthChecks()
        {
            var registry = new Registry();
            var results = new List<Result>();
            foreach (var healthCheckContainer in registry.HealthCheckContainers)
            {
                var healthCheck = healthCheckContainer.GetInstance();
                var context = new CurrentContext();
                var stopWatch = new Stopwatch();
                var result = new Result
                             {
                                 HealthCheckName = healthCheck.Name,
                                 HealthCheckDescription = healthCheck.Description,
                             };

                if (healthCheck is IAutoHealthCheck)
                {
                    stopWatch.Start();
                    ((IAutoHealthCheck) healthCheck).Run(context);
                    stopWatch.Stop();
                }
                else if (healthCheck is ITriggeredHealthCheck)
                {
                    ((ITriggeredHealthCheck) healthCheck).GetStatus(context);
                }
                else
                {
                    throw new Exception("Unknow health check type");
                }

                result.TimeTakenInTicks = stopWatch.ElapsedTicks;
                result.Passed = context.Response.Passed;
                result.Data = context.Response.Data;

                healthCheckContainer.SetState(result.Passed);

                result.FailureCount = healthCheckContainer.FailureCount;
                result.FailingSince = healthCheckContainer.FailingSince;
            }

            return results.ToArray();
        }
    }
}
