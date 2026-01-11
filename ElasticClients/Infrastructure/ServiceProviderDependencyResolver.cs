using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElasticaClients.Infrastructure
{
    public class ServiceProviderDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderDependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var service = _serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(serviceType));
            return service as IEnumerable<object> ?? Enumerable.Empty<object>();
        }
    }
}
