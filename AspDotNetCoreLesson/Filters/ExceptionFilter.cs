﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AspDotNetCoreLesson.Filters
{
	public class ExceptionFilter(ILogger<ExceptionFilter> Logger) : IAsyncExceptionFilter
	{
		public async Task OnExceptionAsync(ExceptionContext context)
		{
			context.Result = new ObjectResult
			(
				new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					ReasonPhrase = "An unexpected error occurred",
					Content = new StringContent(context.Exception.ToString())
				}
			)
			{
				StatusCode = StatusCodes.Status500InternalServerError
			};
			context.ExceptionHandled = true;
		}
	}
}