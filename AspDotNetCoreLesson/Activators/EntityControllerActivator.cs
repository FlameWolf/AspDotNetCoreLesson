using AspDotNetCoreLesson.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Activators
{
	public class EntityControllerActivator<T> : IControllerActivator where T: class, new()
	{
		public object Create(ControllerContext context)
		{
			return Activator.CreateInstance<EntityControllerBase<T>>() as Controller;
		}

		public void Release(ControllerContext context, object controller)
		{
		}
	}
}