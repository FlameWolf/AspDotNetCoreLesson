using AspDotNetCoreLesson.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AspDotNetCoreLesson.Controllers
{
	[ApiController]
	public class ExtendedControllerBase<T>: ControllerBase
	{
		protected ILogger Logger { private set; get; }
		protected IEntityRepository<T> Repository { private set; get; }

		public ExtendedControllerBase(ILoggerFactory loggerFactory, IServiceProvider provider)
		{
			Logger = loggerFactory.CreateLogger<ExtendedControllerBase<T>>();
			Repository = provider.GetService<IEntityRepository<T>>();
		}

		protected string GetTemplateForAction(string actionName)
		{
			var provider = HttpContext.RequestServices.GetService<IActionDescriptorCollectionProvider>();
			return provider.ActionDescriptors.Items.FirstOrDefault
			(
				x => (x as ControllerActionDescriptor)?.ActionName == actionName
			)
			.AttributeRouteInfo?.Template;
		}

		protected PropertyType GetPropertyValue<PropertyType>(string propertyName, object source)
		{
			return (PropertyType)source.GetType().GetProperty(propertyName).GetValue(source);
		}
	}
}