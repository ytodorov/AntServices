using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StructureMap;

namespace FailTracker.Web.Infrastructure
{
	public class StructureMapFilterProvider : FilterAttributeFilterProvider
	{
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
        private readonly Func<IContainer> _container;
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency

        public StructureMapFilterProvider(Func<IContainer> container)
		{
			_container = container;
		}

		public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			IEnumerable<Filter> filters = base.GetFilters(controllerContext, actionDescriptor);

			IContainer container = _container();

			foreach (var filter in filters)
			{
				container.BuildUp(filter.Instance);
				yield return filter;
			}
		}
	}
}