using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Integration.HttpClients;

public class CourseApiClient : BaseHttpClient
{
    public CourseApiClient(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
    {
        BaseUri = configuration["BaseUri"];
    }

    public async Task<HttpResponseMessage> CreateCourseAsync(AddCourseModel course, string cookie = null)
    {
        if (cookie != null)
        {
            AddAuthCookie(cookie);
        }

        SetContext();
        var res = await HttpClient.PostAsJsonAsync($"{BaseUri}/course", course);
        return res;
    }
    
    public async Task<HttpResponseMessage> GetCourseAsync(int id, string cookie = null)
    {
        if (cookie != null)
        {
            AddAuthCookie(cookie);
        }

        SetContext();
        var res = await HttpClient.GetAsync($"{BaseUri}/course/{id}");
        return res;
    }

    private void AddAuthCookie(string cookie)
    {
        HttpClient.DefaultRequestHeaders.Add("cookie", cookie);
    }
}