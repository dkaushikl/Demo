namespace Demo
{
    using System.IO;
    using System.IO.Compression;
    using System.Net;

    using Demo.API.Data.DatabaseContext;
    using Demo.API.Data.Interfaces;
    using Demo.API.Data.Repositories;
    using Demo.API.Services.Interfaces;
    using Demo.API.Services.Services;
    using Demo.AutoMapper;
    using Demo.Utility;

    using global::AutoMapper;

    using log4net;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true).AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(
                options =>
                    {
                        options.Run(
                            async context =>
                                {
                                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                                    if (ex != null)
                                    {
                                        var logger = LogManager.GetLogger(typeof(Program));
                                        logger.Error(
                                            "Api : " + context.Request.Path + " Message : " + ex.Error.Message);
                                        var result = JsonConvert.SerializeObject(new { error = ex.Error.Message });
                                        context.Response.ContentType = "application/json";
                                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                        await context.Response.WriteAsync(result).ConfigureAwait(false);
                                    }
                                });
                    });

            app.UseResponseCaching();

            app.UseResponseCompression();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseStaticFiles();

            app.UseStaticFiles(
                new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Logs")),
                        RequestPath = new PathString("/Logs")
                    });

            app.UseCors("CorsPolicy");

            app.UseMvc(routes => routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));

            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo Clothing"));
            app.UseWelcomePage();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var volselContext = serviceScope.ServiceProvider.GetRequiredService<DemoContext>();
                volselContext.Database.Migrate();
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.Configuration);
            services.Configure<FormOptions>(options => options.BufferBody = true);
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddCors(
                options =>
                    {
                        options.AddPolicy(
                            "CorsPolicy",
                            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                    });

            services.AddDbContext<DemoContext>(
                options => { options.UseSqlServer(this.Configuration.GetConnectionString("Demo")); });

            services.AddResponseCaching();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(
                options =>
                    {
                        options.Providers.Add<GzipCompressionProvider>();
                        options.EnableForHttps = true;
                    });

            services.AddMvc()
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddJsonOptions(
                    options =>
                        {
                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        });

            Mapper.Initialize(c => c.AddProfile<AutoMapperProfile>());
            AddSwagger(services);

            services.Configure<ConnectionStrings>(this.Configuration.GetSection("ConnectionStrings"));

            this.InitilizeRepository(services);
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                    {
                        var info = new Info
                                       {
                                           Version = "v1",
                                           Title = "Demo",
                                           Description = "Demo",
                                           Contact = new Contact { Name = "Demo", Url = "www.Demo.com" }
                                       };
                        options.SwaggerDoc("v1", info);
                    });
        }

        private void InitilizeRepository(IServiceCollection services)
        {
            services.AddScoped<ICatalogRepository, CatalogRepository>();
            services.AddScoped<ICatalogService, CatalogService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IDemoContext, DemoContext>();
            services.AddScoped<IMapper, Mapper>();
        }
    }
}