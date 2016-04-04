using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;
using System;
using StructureMap.Configuration.DSL;
using System.Web.Mvc;

namespace SmartAdminMvc.Infrastructure
{
    public class ControllerConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if(type.CanBeCastTo(typeof(Controller)) && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }
        }
    }
}