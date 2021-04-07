using AspDotNetCoreLesson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Controllers
{
	public class UserController : EntityControllerBase<User>
	{
		public UserController(ILoggerFactory loggerFactory, IServiceProvider provider) : base(loggerFactory, provider)
		{
		}
	}
}