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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Linq;
using System.Reflection;

namespace AspDotNetCoreLesson
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
		{
			var builder = new ServiceCollection()
				.AddLogging()
				.AddControllers()
				.AddNewtonsoftJson()
				.Services
				.BuildServiceProvider();
			return builder
				.GetRequiredService<IOptions<MvcOptions>>()
				.Value
				.InputFormatters
				.OfType<NewtonsoftJsonPatchInputFormatter>()
				.First();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging(builder =>
			{
				builder.AddConsole()
					.AddDebug()
					.AddEventLog();
			}).AddSingleton<ILoggerFactory, LoggerFactory>();
			services.AddDbContext<ApplicationDbContext>(context =>
			{
				context.UseSqlServer
				(
					_configuration.GetConnectionString("AspDotNetCoreLesson")
				);
			});
			services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepositoryBase<>));
			services.TryAddEnumerable
			(
				ServiceDescriptor.Transient
				<
					IApplicationModelProvider,
					EntityControllerModelProvider
				>()
			);
			services.AddControllers(options =>
			{
				options.Conventions.Add(new EntityControllerRouteConvention());
				options.Filters.Add<ExceptionFilter>();
				options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
			})
			.ConfigureApplicationPartManager(config =>
			{
				config.FeatureProviders.Add(new EntityControllerFeatureProvider());
			})
			.AddNewtonsoftJson();
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

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint
					(
						"/swagger/v1/swagger.json",
						"AspDotNetCoreLesson v1"
					);
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