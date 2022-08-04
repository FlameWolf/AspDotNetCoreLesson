using System;

namespace AspDotNetCoreLesson.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GenerateControllerAttribute : Attribute
	{
		public string Route { set; get; }

		public GenerateControllerAttribute()
		{
		}

		public GenerateControllerAttribute(string route)
		{
			Route = route;
		}
	}
}