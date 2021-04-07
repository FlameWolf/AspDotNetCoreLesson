using AspDotNetCoreLesson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;
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
	public class EntityControllerBase<TRequest> : ExtendedControllerBase<TRequest> where TRequest: class, new()
	{
		public EntityControllerBase(ILoggerFactory loggerFactory, IServiceProvider provider) : base(loggerFactory, provider)
		{
		}

		[Route("create")]
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] TRequest request)
		{
			try
			{
				var Id = GetPropertyValue<uint>("Id", request);
				var response = await Repository.Get(Id);
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
				response = await Repository.Add(request);
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
			var response = await Repository.Get(id);
			if (response == null)
			{
				return NotFound();
			}
			return Ok(response);
		}

		[Route("get-all")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var response = await Repository.GetAll();
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
			TRequest entity = await Repository.Get(id);
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
			var response = await Repository.Update(entity);
			return Ok(response);
		}

		[Route("delete/{id}")]
		[HttpDelete]
		public async Task<IActionResult> Delete(uint id)
		{
			var response = await Repository.Delete(id);
			return Ok(response);
		}
	}
}