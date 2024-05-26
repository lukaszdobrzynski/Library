using System;
using Library.BuildingBlocks.Application;
using Microsoft.AspNetCore.Http;

namespace Library.API.ExecutionContext;

public class ExecutionContextAccessor : IExecutionContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CorrelationId
    {
        get
        {
            if (IsAvailable == false)
            {
                throw new ArgumentException("Http context is not available.");
            }
            
            if (_httpContextAccessor.HttpContext.Request.Headers.Keys.Contains(CorrelationMiddleware.CorrelationIdHeader) == false)
            {
                throw new ArgumentException("CorrelationId is not available.");
            }
            
            return Guid.Parse(
                _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationIdHeader]);
        }
    }

    public bool IsAvailable => _httpContextAccessor.HttpContext is not null;
}