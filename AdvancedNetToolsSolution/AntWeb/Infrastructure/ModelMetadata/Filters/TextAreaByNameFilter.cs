using System;
using System.Collections.Generic;

namespace FailTracker.Web.Infrastructure.ModelMetadata.Filters
{
	public class TextAreaByNameFilter : IModelMetadataFilter
	{
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
        private static readonly HashSet<string> TextAreaFieldNames =
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
                new HashSet<string>
						{
							"body",
							"comments"
						};

		public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata,
			IEnumerable<Attribute> attributes)
		{
			if (!string.IsNullOrEmpty(metadata.PropertyName) &&
				string.IsNullOrEmpty(metadata.DataTypeName) &&
				TextAreaFieldNames.Contains(metadata.PropertyName.ToLower()))
			{
				metadata.DataTypeName = "MultilineText";
			}
		}
	}
}