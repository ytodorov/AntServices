using System;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using StructureMap;
using StructureMap.Graph.Scanning;

namespace FailTracker.Web.Infrastructure
{
    public class ControllerConvention : IRegistrationConvention
    {
        //public void Process(Type type, Registry registry)
        //{
        //	if (type.CanBeCastTo(typeof (Controller)) && !type.IsAbstract)
        //	{
        //		registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
        //	}
        //}

        public void ScanTypes(TypeSet types, Registry registry)
        {
            System.Collections.Generic.IEnumerable<Type> allTypes = types.AllTypes();
            foreach (var type in allTypes)
            {
                if (type.CanBeCastTo(typeof(Controller)) && !type.IsAbstract)
                {
                    registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
                }
            }
        }
    }
}