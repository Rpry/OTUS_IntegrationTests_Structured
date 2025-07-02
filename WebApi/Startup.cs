using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Services.HttpClients;
using ComponentRegistrar;
using Demo.Authentication.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WebApi.Mapping;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InstallAutomapper(services);
            services.AddServices(Configuration);
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            
            // Добавляем аутентификацию
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.Events.OnSignedIn = context =>
                    {
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Database>();
            
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            
            services.AddHttpClient<IService1HttpClient, Service1HttpClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(Configuration["Service1Uri"]);
                client.DefaultRequestHeaders.Add(BusinessLogic.Services.Constants.RequestCorrelationIdName,RequestContext.GetRequestCorId() ?? Guid.NewGuid().ToString());
                client.DefaultRequestHeaders.Add(BusinessLogic.Services.Constants.RequestSetCorrelationIdName,RequestContext.GetRequestSetCorId() ?? Guid.NewGuid().ToString());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRequestContextMiddleware();
            app.UseSimpleHttpLogging();
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (!env.IsProduction())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
               
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        private static IServiceCollection InstallAutomapper(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var loggerFactory = sp.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
            services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration(loggerFactory)));
            return services;
        }
        
        private static MapperConfiguration GetMapperConfiguration(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseMappingsProfile>();
                cfg.AddProfile<LessonMappingsProfile>();
                cfg.AddProfile<BusinessLogic.Services.Mapping.CourseMappingsProfile>();
                cfg.AddProfile<BusinessLogic.Services.Mapping.LessonMappingsProfile>();
            }, loggerFactory);
            configuration.AssertConfigurationIsValid();
            return configuration;
        }
    }
}