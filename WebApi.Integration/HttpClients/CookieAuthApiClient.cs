using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Microsoft.Extensions.Configuration;

namespace WebApi.Integration.HttpClients;

public class CookieAuthApiClient : BaseHttpClient
{
    public CookieAuthApiClient(HttpClient httpClient, IConfiguration configuration): base(httpClient)
    {
        BaseUri = configuration["BaseUri"];
    }
    
    public async Task<HttpResponseMessage> GetAuthCookieAsync(string login, string password)
    {
        return await HttpClient.PostAsJsonAsync($"{BaseUri}/Auth/Login", new AuthDto { Login = login, Password = password });
    }
}