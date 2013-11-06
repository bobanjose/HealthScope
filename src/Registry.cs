using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HealthScope.HealthCheck;

namespace HealthScope
{
    public class Registry
    {
        private static List<HealthCheckContainer> _healthCheckContainers = new List<HealthCheckContainer>();  

        private static object _syncLock = new object();

        public void AutoRegister(params Assembly[] assemblies)
        {
            var autoType = (typeof (IAutoHealthCheck));
            var triggeredType = (typeof (ITriggeredHealthCheck));

            var healthCheckTypes = assemblies.Where(a => a != null).SelectMany(getTypes).Where(hc => autoType.IsAssignableFrom(hc) || triggeredType.IsAssignableFrom(hc));

            healthCheckTypes.ToList().ForEach(registerCheck);
        }

        public void Register<T>() where T:IAutoHealthCheck, ITriggeredHealthCheck
        {
            registerCheck(typeof(T));
        }

        public ITriggeredHealthCheck TriggeredHealthChecks<T>() where T : ITriggeredHealthCheck
        {
            return (ITriggeredHealthCheck)tryOrAdd(new HealthCheckContainer(typeof(T))).GetInstance();
        }

        internal List<HealthCheckContainer> HealthCheckContainers
        {
            get { return _healthCheckContainers; }
        }

        private HealthCheckContainer tryOrAdd(HealthCheckContainer healthCheckContainer)
        {
            if (!_healthCheckContainers.Contains(healthCheckContainer))
            {
                lock (_syncLock)
                {
                    if (!_healthCheckContainers.Contains(healthCheckContainer))
                        _healthCheckContainers.Add(healthCheckContainer);
                }
            }
            return _healthCheckContainers.First(hc => hc == healthCheckContainer);
        }

        private static IEnumerable<Type> getTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
        }

        private void registerCheck(Type type)
        {
            var healthCheckContainer = new HealthCheckContainer(type);
            tryOrAdd(healthCheckContainer);
        }
    }
}
