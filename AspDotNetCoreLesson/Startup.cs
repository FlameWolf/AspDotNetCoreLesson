using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspDotNetCoreLesson.Conventions;
using AspDotNetCoreLesson.Database;
using AspDotNetCoreLesson.Filters;
using AspDotNetCoreLesson.Providers;
using AspDotNetCoreLesson.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AspDotNetCoreLesson
{
	public class Startup(IConfiguration Configuration)
	{
		private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => new ServiceCollection().AddLogging().AddControllers().AddNewtonsoftJson().Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging();
			services.AddDbContext<DbContext, ApplicationDbContext>(context =>
			{
				context.UseSqlServer(Configuration.GetConnectionString("AspDotNetCoreLesson"));
			});
			services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepositoryBase<>));
			services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, EntityControllerModelProvider>());
			services.AddControllers(options =>
			{
				options.Conventions.Add(new EntityControllerRouteConvention());
				options.Filters.Add<ExceptionFilter>();
				options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
			}).ConfigureApplicationPartManager(config =>
			{
				config.FeatureProviders.Add(new EntityControllerFeatureProvider());
			}).AddNewtonsoftJson();
			services.AddSwaggerGen(config =>
			{
				config.SwaggerDoc
				(
					"v1",
					new OpenApiInfo
					{
						Title = "AspDotNetCoreLesson",
						Version = "v1"
					}
				);
				config.ExampleFilters();
			}).AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "AspDotNetCoreLesson v1");
				});
				app.UseRouter(builder =>
				{
					builder.MapGet("/", async context =>
					{
						await Task.Run(() => context.Response.Redirect("./swagger/index.html", permanent: false));
					});
				});
			}
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}