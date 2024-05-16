using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AspDotNetCoreLesson
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(builder =>
				{
					builder.UseStartup<Startup>();
				});
		}
	}
}