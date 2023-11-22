using System;
using System.Net.Http;
using BusinessLogic.Services;

namespace WebApi.Integration.HttpClients;

public class BaseHttpClient
{
    protected readonly HttpClient HttpClient;
    protected string BaseUri;

    protected BaseHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
        HttpClient.DefaultRequestHeaders.Add(Constants.RequestCorrelationIdName, Guid.NewGuid().ToString());
        HttpClient.DefaultRequestHeaders.Add(Constants.RequestSetCorrelationIdName, RequestContext.GetRequestSetCorId());
    }
}