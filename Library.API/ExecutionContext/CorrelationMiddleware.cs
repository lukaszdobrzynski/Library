using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Library.API.ExecutionContext;

public class CorrelationMiddleware
{
    public const string CorrelationIdHeader = "CorrelationId";
    private readonly RequestDelegate _next;
    
    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = Guid.NewGuid();
        
        context.Request?.Headers.Add(CorrelationIdHeader, correlationId.ToString());

        await _next.Invoke(context);
    }
}