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
    }

    protected void SetContext()
    {
        HttpClient.DefaultRequestHeaders.Remove(Constants.RequestCorrelationIdName);
        HttpClient.DefaultRequestHeaders.Add(Constants.RequestCorrelationIdName, Guid.NewGuid().ToString());
        HttpClient.DefaultRequestHeaders.Remove(Constants.RequestSetCorrelationIdName);
        HttpClient.DefaultRequestHeaders.Add(Constants.RequestSetCorrelationIdName, RequestContext.GetRequestSetCorId());
    }
}