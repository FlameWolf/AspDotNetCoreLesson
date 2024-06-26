using System.Linq;
using System.Threading.Tasks;
using AspDotNetCoreLesson.Extensions;
using AspDotNetCoreLesson.Models;
using AspDotNetCoreLesson.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspDotNetCoreLesson.Controllers
{
	public class EntityControllerBase<TRequest>(ILogger<EntityControllerBase<TRequest>> Logger, IEntityRepository<TRequest> Repository) : ControllerBase where TRequest : new()
	{
		protected string GetTemplateForAction(string actionName)
		{
			var provider = HttpContext.RequestServices.GetService<IActionDescriptorCollectionProvider>();
			return provider.ActionDescriptors.Items.FirstOrDefault
			(
				x => (x as ControllerActionDescriptor)?.ActionName == actionName
			).AttributeRouteInfo?.Template;
		}

		protected PropertyType GetPropertyValue<PropertyType>(string propertyName, object source)
		{
			return (PropertyType)source.GetType().GetProperty(propertyName).GetValue(source);
		}

		[Route("add")]
		[HttpPost]
		public async Task<IActionResult> AddAsync([FromBody] TRequest request)
		{
			try
			{
				var id = GetPropertyValue<uint>("Id", request);
				var response = await Repository.GetAsync(id);
				if (response != null)
				{
					return Conflict
					(
						$"A {typeof(TRequest).Name} with the specified ID ({id}) already exists"
					);
				}
				response = await Repository.AddAsync(request);
				var routeTemplate = GetTemplateForAction(nameof(GetAsync));
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
		public async Task<IActionResult> GetAsync(uint id)
		{
			var response = await Repository.GetAsync(id);
			if (response == null)
			{
				return NotFound
				(
					$"Unable to find a {typeof(TRequest).Name.ToCamel()} with the specified ID ({id})"
				);
			}
			return Ok(response);
		}

		[Route("get")]
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			try
			{
				var response = await Repository.GetAsync();
				return Ok(response);
			}
			catch
			{
				return NoContent();
			}
		}

		[Route("update/{id}")]
		[HttpPatch]
		public async Task<IActionResult> UpdateAsync(uint id, [FromBody] PatchRequest<TRequest> request)
		{
			var response = await Repository.GetAsync(id);
			if (response == null)
			{
				return NotFound
				(
					$"Unable to find a {typeof(TRequest).Name.ToCamel()} with the specified ID ({id})"
				);
			}
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
			response = await Repository.UpdateAsync(response);
			return Ok(response);
		}

		[Route("delete/{id}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteAsync(uint id)
		{
			var response = await Repository.GetAsync(id);
			if (response == null)
			{
				return NotFound
				(
					$"Unable to find a {typeof(TRequest).Name.ToCamel()} with the specified ID ({id})"
				);
			}
			response = await Repository.DeleteAsync(response);
			return Ok(response);
		}
	}
}