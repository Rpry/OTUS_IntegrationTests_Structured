using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demo.Authentication.Dto;
using Microsoft.Extensions.Configuration;

namespace WebApi.Integration.HttpClients;

public class TokenApiClient : BaseHttpClient
{
    public TokenApiClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
    {
        BaseUri = configuration["BaseUri"];
    }
    
    public async Task<HttpResponseMessage> GetTokenInternalAsync(string name, string password)
    {
        return await HttpClient.PostAsJsonAsync($"{BaseUri}/token", new AuthDto { Login = name, Password = password });
    }
}