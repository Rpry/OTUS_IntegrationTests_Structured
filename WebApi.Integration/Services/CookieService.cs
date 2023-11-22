using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Integration.HttpClients;

namespace WebApi.Integration.Services;

public class CookieService
{
    private CookieAuthApiClient _cookieAuthApiClient;

    public CookieService(CookieAuthApiClient cookieAuthApiClient)
    {
        _cookieAuthApiClient = cookieAuthApiClient;
    }
    
    public async Task<string> GetAdminCookieAsync()
    {
        return await GetCookieAsync("admin", "admin");
    }
    
    public async Task<string> GetCookieAsync(string name, string password)
    {
        var response = await GetCookieInternalAsync(name, password);
        return response.Headers.FirstOrDefault(h=> h.Key == "Set-Cookie").Value.ToList().First();
    }
    
    public async Task<HttpResponseMessage> GetCookieInternalAsync(string name, string password)
    {
        return await _cookieAuthApiClient.GetAuthCookieAsync(name, password);
    }
}