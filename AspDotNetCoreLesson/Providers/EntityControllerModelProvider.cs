using AspDotNetCoreLesson.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Providers
{
	public class EntityControllerModelProvider : IApplicationModelProvider
	{
		public int Order => -991;

		public void OnProvidersExecuted(ApplicationModelProviderContext context)
		{
		}

		public void OnProvidersExecuting(ApplicationModelProviderContext context)
		{
			foreach
			(
				var controllerModel
				in context.Result.Controllers
				.Where
				(
					x =>
					(
						x.ControllerType.Name ==
						typeof(EntityControllerBase<>).Name
					)
				)
			)
			{
				controllerModel.ControllerName = controllerModel
					.ControllerType
					.GenericTypeArguments[0]
					.Name;
			}
		}
	}
}