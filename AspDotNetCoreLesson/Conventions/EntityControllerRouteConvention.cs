using System.Reflection;
using AspDotNetCoreLesson.Attributes;
using AspDotNetCoreLesson.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AspDotNetCoreLesson.Conventions
{
	public class EntityControllerRouteConvention : IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			if (controller.ControllerType.IsGenericType)
			{
				var genericTypeArgument = controller.ControllerType.GenericTypeArguments[0];
				var generatedControllerAttribute = genericTypeArgument.GetCustomAttribute<GeneratedControllerAttribute>();
				string route = string.IsNullOrEmpty(generatedControllerAttribute?.Route) ?
					genericTypeArgument.Name.ToCamel() :
					generatedControllerAttribute.Route;
				controller.Selectors.Add
				(
					new SelectorModel
					{
						AttributeRouteModel = new AttributeRouteModel
						(
							new RouteAttribute(route)
						)
					}
				);
			}
		}
	}
}