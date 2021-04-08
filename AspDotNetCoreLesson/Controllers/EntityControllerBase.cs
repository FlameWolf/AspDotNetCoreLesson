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
	public class EntityControllerBase<TRequest> : ControllerBase where TRequest: class, new()
	{
		private readonly ILogger _logger;
		private readonly IEntityRepository<TRequest> _repository;

		public EntityControllerBase(ILoggerFactory loggerFactory, IServiceProvider provider)
		{
			_logger = loggerFactory.CreateLogger<EntityControllerBase<TRequest>>();
			_repository = provider.GetService<IEntityRepository<TRequest>>();
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

		[Route("add")]
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] TRequest request)
		{
			try
			{
				var Id = GetPropertyValue<uint>("Id", request);
				var response = await _repository.Get(Id);
				if (response != null)
				{
					return Conflict(new HttpResponseMessage
					{
						ReasonPhrase = $"A user with the specified Id ({Id}) already exists",
						Content = new ObjectContent
						(
							response.GetType(),
							response,
							new JsonMediaTypeFormatter()
						)
					});
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
			if (response == null)
			{
				return NotFound();
			}
			return Ok(response);
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
		public async Task<IActionResult> Update(uint id, [FromBody] PatchRequest<TRequest> request)
		{
			TRequest entity = await _repository.Get(id);
			if (entity == null)
			{
				return NotFound();
			}
			((JsonPatchDocument)request).ApplyTo(entity, error =>
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
			var response = await _repository.Update(entity);
			return Ok(response);
		}

		[Route("delete/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(uint id)
		{
			var response = await _repository.Delete(id);
			return Ok(response);
		}
	}
}