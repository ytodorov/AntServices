using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using System.Web.Mvc;

namespace SmartAdminMvc.Infrastructure
{
    public class StructureMapDependecyResolver : IDependencyResolver
    {

        private Func<IContainer> _containerFactory;

        public StructureMapDependecyResolver(Func<IContainer> containerFactory)
        {
            _containerFactory = containerFactory;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }

            var container = _containerFactory();

            return serviceType.IsAbstract || serviceType.IsInterface
                ? container.TryGetInterface(serviceType)
                : container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ObjectFatcory.GetAllInstances(serviceType).Cast<object>();
        }
    }
}