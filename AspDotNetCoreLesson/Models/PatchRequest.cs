using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AspDotNetCoreLesson.Models
{
	public class PatchRequest<TRequest> where TRequest : new()
	{
		public string Op { set; get; }
		public string From { set; get; }
		public string Path { set; get; }
		public string Value { set; get; }

		public PatchRequest()
		{
			var requestAsJObject = GetInstanceAsJObject<TRequest>();
			Op = "test";
			From = string.Empty;
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value.ToString();
		}

		public PatchRequest(TRequest request)
		{
			var requestAsJObject = ConvertToJObject(request);
			Op = "test";
			From = string.Empty;
			Path = requestAsJObject.Properties().FirstOrDefault().Name;
			Value = requestAsJObject.Properties().FirstOrDefault().Value.ToString();
		}

		public static implicit operator JsonPatchDocument(PatchRequest<TRequest> request)
		{
			var patchDocument = new JsonPatchDocument();
			patchDocument.Operations.Add
			(
				new Operation
				{
					op = request.Op,
					from = request.From,
					path = request.Path,
					value = request.Value
				}
			);
			return patchDocument;
		}

		private static JObject ConvertToJObject(object source)
		{
			return JObject.FromObject
			(
				source,
				new JsonSerializer
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}
			);
		}

		private JObject GetInstanceAsJObject<TSource>() where TSource : new()
		{
			return ConvertToJObject(new TSource());
		}
	}
}