using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace FailTracker.Web.Infrastructure.ModelMetadata
{
	public class ExtensibleModelMetadataProvider
		: DataAnnotationsModelMetadataProvider
	{
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
        private readonly IModelMetadataFilter[] _metadataFilters;
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency

        public ExtensibleModelMetadataProvider(
			IModelMetadataFilter[] metadataFilters)
		{
			_metadataFilters = metadataFilters;
		}

		protected override System.Web.Mvc.ModelMetadata CreateMetadata(
			IEnumerable<Attribute> attributes,
			Type containerType,
			Func<object> modelAccessor,
			Type modelType,
			string propertyName)
		{
			System.Web.Mvc.ModelMetadata metadata = base.CreateMetadata(
				attributes,
				containerType,
				modelAccessor,
				modelType,
				propertyName);

			_metadataFilters.ForEach(m =>
				m.TransformMetadata(metadata, attributes));

			return metadata;
		}
	}
}