using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Filters
{
	public class ExceptionFilter : IAsyncExceptionFilter
	{
		private readonly ILogger _logger;

		public ExceptionFilter(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<ExceptionFilter>();
		}

		public async Task OnExceptionAsync(ExceptionContext context)
		{
			var result = new ObjectResult("An exception occurred during the operation");
			result.ContentTypes = new MediaTypeCollection
			{
				new MediaTypeHeaderValue("text/plain")
			};
			result.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.ExceptionHandled = true;
			context.Result = result;
		}
	}
}