using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi;

namespace BusinessLogic.Services.HttpClients;

public class Service1HttpClient : IService1HttpClient
{
    private HttpClient _httpClient;

    public Service1HttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    /// <summary>
    /// Сохранить параметры ответа от Конфигуратора.
    /// </summary>
    public async Task SendRequestAsync()
    {   
        var request = new HttpRequestMessage(HttpMethod.Get, "weatherforecast/list");
        request.Headers.Add(Constants.RequestCorrelationIdName, RequestContext.GetRequestCorId());
        request.Headers.Add(Constants.RequestSetCorrelationIdName, RequestContext.GetRequestSetCorId());

        var responseMessage = await _httpClient.SendAsync(request);
        if (responseMessage.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Ошибка отправки запроса");
        }
    } 
}