using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreLesson.Models
{
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