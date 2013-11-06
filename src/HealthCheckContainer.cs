using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HealthScope.HealthCheck;

namespace HealthScope
{
    public class HealthCheckContainer
    {
        private readonly Type _healthCheckType;
        private IHealthCheck _healthCheck;
        private object _syncLock = new object();

        public DateTime? FailingSince { get; set; }

        public int FailureCount { get; set; }

        public HealthCheckContainer(Type healthCheckType)
        {
            _healthCheckType = healthCheckType;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == _healthCheckType;
        }

        public override int GetHashCode()
        {
            return _healthCheckType.GetHashCode();
        }

        internal IHealthCheck GetInstance()
        {
            if (_healthCheck == null)
            {
                lock (_syncLock)
                {
                    if (_healthCheck != null)
                        return _healthCheck;
                    _healthCheck = (IHealthCheck) Activator.CreateInstance(_healthCheckType);
                }
            }
            return _healthCheck;
        }

        public void SetState(bool passed)
        {
            if (passed)
            {
                FailingSince = null;
                FailureCount = 0;
            }
            else
            {
                if (!FailingSince.HasValue)
                    FailingSince = DateTime.Now;

                FailureCount++;
            }
            var healthCheck = (IHealthCheck) Activator.CreateInstance(_healthCheckType);
            Interlocked.Exchange(ref _healthCheck, healthCheck);
        }
    }
}
