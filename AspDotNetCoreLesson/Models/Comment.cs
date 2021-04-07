using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Models
{
	public class Comment
	{
		public uint Id { set; get; }
		public uint PostId { set; get; }
		public uint UserId { set; get; }
		public string Content { set; get; }
	}

	public class CommentExample : IExamplesProvider<Comment>
	{
		public Comment GetExamples()
		{
			return new Comment
			{
				Id = 301,
				PostId = 201,
				UserId = 101,
				Content = "Test comment"
			};
		}
	}

	public class CommentPatchExample : IExamplesProvider<PatchRequest<Comment>>
	{
		public PatchRequest<Comment> GetExamples() =>
			new PatchRequest<Comment>(new CommentExample().GetExamples());
	}
}