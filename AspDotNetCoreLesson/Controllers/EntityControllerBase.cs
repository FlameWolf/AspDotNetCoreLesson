using AspDotNetCoreLesson.Extensions;
using AspDotNetCoreLesson.Models;
using AspDotNetCoreLesson.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Controllers
{
	public class EntityControllerBase<T> : ControllerBase where T : class, new()
	{
		private readonly ILogger _logger;
		private readonly IEntityRepository<T> _repository;

		public EntityControllerBase(ILoggerFactory loggerFactory, IServiceProvider provider)
		{
			_logger = loggerFactory.CreateLogger<EntityControllerBase<T>>();
			_repository = provider.GetService<IEntityRepository<T>>();
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

		private async Task<IActionResult> DoIfNotNull(T response, uint id, Func<Task<IActionResult>> action)
		{
			if (response == null)
			{
				return NotFound
				(
					$"Unable to find a {typeof(T).Name.ToCamel()} with the specified ID ({id})"
				);
			}
			return await action();
		}

		[Route("add")]
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] T request)
		{
			try
			{
				var id = GetPropertyValue<uint>("Id", request);
				var response = await _repository.Get(id);
				if (response != null)
				{
					return Conflict
					(
						$"A {typeof(T).Name} with the specified ID ({id}) already exists"
					);
				}
				response = await _repository.Add(request);
				var routeTemplate = GetTemplateForAction(nameof(Get));
				return Created
				(
					routeTemplate.Replace
					(
						"{id}",
						GetPropertyValue<uint>("Id", response).ToString()
					),
					response
				);
			}
			catch
			{
				return BadRequest();
			}
		}

		[Route("get/{id}")]
		[HttpGet]
		public async Task<IActionResult> Get(uint id)
		{
			var response = await _repository.Get(id);
			return await DoIfNotNull(response, id, async () => Ok(response));
		}

		[Route("get")]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var response = await _repository.Get();
				return Ok(response);
			}
			catch
			{
				return NoContent();
			}
		}

		[Route("update/{id}")]
		[HttpPatch]
		public async Task<IActionResult> Update(uint id, [FromBody] PatchRequest<T> request)
		{
			var response = await _repository.Get(id);
			return await DoIfNotNull(response, id, async () =>
			{
				((JsonPatchDocument)request).ApplyTo(response, error =>
				{
					ModelState.AddModelError
					(
						error.AffectedObject.GetType().Name,
						error.ErrorMessage
					);
				});
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				response = await _repository.Update(response);
				return Ok(response);
			});
		}

		[Route("delete/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(uint id)
		{
			var response = await _repository.Get(id);
			return await DoIfNotNull(response, id, async () =>
			{
				response = await _repository.Delete(response);
				return Ok(response);
			});
		}
	}
}