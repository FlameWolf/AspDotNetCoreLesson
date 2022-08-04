using AspDotNetCoreLesson.Attributes;
using Swashbuckle.AspNetCore.Filters;

namespace AspDotNetCoreLesson.Models
{
	[GenerateController("api/post")]
	public class Post
	{
		public uint Id { set; get; }
		public uint UserId { set; get; }
		public string Content { set; get; }
	}

	public class PostExample : IExamplesProvider<Post>
	{
		public Post GetExamples()
		{
			return new Post
			{
				Id = 201,
				UserId = 101,
				Content = "Test post"
			};
		}
	}

	public class PostPatchExample : IExamplesProvider<PatchRequest<Post>>
	{
		public PatchRequest<Post> GetExamples() => new(new PostExample().GetExamples());
	}
}