using System;
using System.Threading;

namespace WebApi.Integration;

public class RequestContext
{
    private static readonly AsyncLocal<string> RequestSetCorId = new AsyncLocal<string>();

    public static void SetRequestSetCorId(string requestSetCorId)
    {
        if (string.IsNullOrWhiteSpace(requestSetCorId))
        {
            throw new ArgumentException("Request correlation Id cannot be null or empty", nameof(requestSetCorId));
        }

        if (!string.IsNullOrWhiteSpace(RequestSetCorId.Value))
        {
            throw new InvalidOperationException("Request Set correlation Id is already set for the context");
        }

        RequestSetCorId.Value = requestSetCorId;
    }

    public static string GetRequestSetCorId()
    {
        return RequestSetCorId?.Value;
    }
}