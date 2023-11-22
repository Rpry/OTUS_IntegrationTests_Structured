using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Integration.HttpClients;

public class LessonApiClient : BaseHttpClient
{
    public LessonApiClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
    {
        BaseUri = configuration["BaseUri"];
    }
    
    public async Task<HttpResponseMessage> GetLessonAsync(int id, string token = null)
    {
        if (token != null)
        {
            AddToken(token);
        }
        return await HttpClient.GetAsync($"{BaseUri}/lesson/{id}");
    }
    
    public async Task<HttpResponseMessage> AddLessonAsync(LessonModel lessonModel, string token = null)
    {
        if (token != null)
        {
            AddToken(token);
        }
        return await HttpClient.PostAsJsonAsync($"{BaseUri}/lesson", lessonModel);
    }
    
    public async Task<HttpResponseMessage> EditLessonAsync(int id, LessonModel lessonModel, string token = null)
    {
        if (token != null)
        {
            AddToken(token);
        }
        return await HttpClient.PutAsJsonAsync($"{BaseUri}/lesson/{id}", lessonModel);
    }
    
    private void AddToken(string token)
    {
        if (!HttpClient.DefaultRequestHeaders.Contains("Authorization"))
        {
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");    
        }
    }
}