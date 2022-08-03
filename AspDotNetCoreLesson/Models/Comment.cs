using AspDotNetCoreLesson.Attributes;
using Swashbuckle.AspNetCore.Filters;

namespace AspDotNetCoreLesson.Models
{
	[GeneratedController("api/comment")]
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
		public PatchRequest<Comment> GetExamples() => new(new CommentExample().GetExamples());
	}
}