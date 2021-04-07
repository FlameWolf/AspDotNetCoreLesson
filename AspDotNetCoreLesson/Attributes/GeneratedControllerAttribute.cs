using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class GeneratedControllerAttribute: Attribute
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