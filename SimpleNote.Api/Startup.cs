using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleNote.Api.Data;
using SimpleNote.Api.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SimpleNote.Api.Helpers;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace SimpleNote.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setup =>
            {
                setup.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "http://www.details.com", 
                        Title = "Error！！！",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "please see the link for details",
                        Instance = context.HttpContext.Request.Path 
                    };

                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" } //RFC 7807
                    };
                };
            });

            services.Configure<MvcOptions>(config =>
            {
                var newtonSoftJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                newtonSoftJsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.mycompany.hateoas+json");

                var intputFormatter = config.InputFormatters.OfType<NewtonsoftJsonInputFormatter>().FirstOrDefault();

                if (intputFormatter != null)
                {
                    intputFormatter.SupportedMediaTypes.Add("application/vnd.mycompany.note.create+json");
                    intputFormatter.SupportedMediaTypes.Add("application/vnd.mycompany.note.update+json");
                }
            });

            services.AddDbContext<MyContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INoteRepository, NoteRepository>();

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;

            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:6001";
                    options.ApiName = "restapi";
                });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<NotePropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDevOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
           
            services.Configure<MvcOptions>(options =>
            {
                //options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAngularDevOrigin"));

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMyExceptionHandler(loggerFactory);
            app.UseCors("AllowAngularDevOrigin");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>{endpoints.MapControllers();});

            DbInitializer.SeedAsync(app).Wait();
        }
    }
}
