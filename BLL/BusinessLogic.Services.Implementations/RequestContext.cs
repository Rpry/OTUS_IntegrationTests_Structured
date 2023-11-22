using System;
using System.Threading;

namespace WebApi;

public class RequestContext
{
    private static readonly AsyncLocal<string> RequestCorId = new AsyncLocal<string>();
    private static readonly AsyncLocal<string> RequestSetCorId = new AsyncLocal<string>();

    public static void SetRequestCorId(string requestCorId)
    {
        if (string.IsNullOrWhiteSpace(requestCorId))
        {
            throw new ArgumentException("Request Correlation Id cannot be null or empty", nameof(requestCorId));
        }

        if (!string.IsNullOrWhiteSpace(RequestCorId.Value))
        {
            throw new InvalidOperationException("Request Correlation Id is already set for the context");
        }
        
        RequestCorId.Value = requestCorId;
    }

    public static string GetRequestCorId()
    {
        return RequestCorId.Value;
    }
    
    public static void SetRequestSetCorId(string requestSetCorId)
    {
        /*
        if (string.IsNullOrWhiteSpace(requestSetCorId))
        {
            throw new ArgumentException("Request Set Correlation Id cannot be null or empty", nameof(requestSetCorId));
        }

        if (!string.IsNullOrWhiteSpace(RequestSetCorId.Value))
        {
            throw new InvalidOperationException("Request Set Correlation Id is already set for the context");
        }
        */
        RequestSetCorId.Value = requestSetCorId;
    }

    public static string GetRequestSetCorId()
    {
        return RequestSetCorId.Value;
    }
}