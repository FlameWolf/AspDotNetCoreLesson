using AspDotNetCoreLesson.Attributes;
using AspDotNetCoreLesson.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Providers
{
	public class EntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
	{
		public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
		{
			var assembly = typeof(EntityControllerFeatureProvider).Assembly;
			var candidates = assembly
				.GetExportedTypes()
				.Where
				(
					x => x.GetCustomAttributes(true)
					.Any
					(
						x => x is GeneratedControllerAttribute
					)
				);
			foreach (var candidate in candidates)
			{
				feature.Controllers.Add
				(
					typeof(EntityControllerBase<>).MakeGenericType(candidate).GetTypeInfo()
				);
			}
		}
	}
}