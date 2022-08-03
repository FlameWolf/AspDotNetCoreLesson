using AspDotNetCoreLesson.Attributes;
using Swashbuckle.AspNetCore.Filters;

namespace AspDotNetCoreLesson.Models
{
	[GeneratedController("api/user")]
	public class User
	{
		public uint Id { set; get; }
		public string Handle { set; get; }
		public string Email { set; get; }
	}

	public class UserExample : IExamplesProvider<User>
	{
		public User GetExamples()
		{
			return new User
			{
				Id = 101,
				Handle = "TestUser",
				Email = "test.user@server.net"
			};
		}
	}

	public class UserPatchExample : IExamplesProvider<PatchRequest<User>>
	{
		public PatchRequest<User> GetExamples() =>
			new PatchRequest<User>(new UserExample().GetExamples());
	}
}