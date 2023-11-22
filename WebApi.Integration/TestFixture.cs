using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Integration.HttpClients;
using WebApi.Integration.Services;
using Xunit;

namespace WebApi.Integration
{
    public class TestFixture : IAsyncLifetime
    {
        public IConfigurationRoot Configuration { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public string Token { get; set; }
        public string AuthCookie { get; set; }
        
        /// <summary>
        /// Выполняется перед запуском тестов
        /// </summary>
        public TestFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json").Build();
            var serviceCollection = new ServiceCollection()
                .AddSingleton((IConfiguration)Configuration)
                .AddTransient<CourseService>()
                .AddTransient<LessonService>()
                .AddTransient<TokenService>()
                .AddTransient<CookieService>()
                .AddTransient<LessonApiClient>()
                .AddTransient<CourseApiClient>()
                .AddTransient<TokenApiClient>()
                .AddTransient<CookieAuthApiClient>()
                .AddHttpClient();
            var serviceProvider = serviceCollection
                .BuildServiceProvider();
            ServiceProvider = serviceProvider;
        }
        
        public async Task InitializeAsync()
        {
            Token = await ServiceProvider.GetService<TokenService>().GetAdminTokenAsync();
            AuthCookie = await ServiceProvider.GetService<CookieService>().GetAdminCookieAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
