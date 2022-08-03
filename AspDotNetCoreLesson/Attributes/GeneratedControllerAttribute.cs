using System;

namespace AspDotNetCoreLesson.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GeneratedControllerAttribute : Attribute
	{
		public string Route { set; get; }

		public GeneratedControllerAttribute()
		{
		}

		public GeneratedControllerAttribute(string route)
		{
			Route = route;
		}
	}
}