using Microsoft.AspNetCore.Http;
using MusicCollection.Api.Dto.Exceptions;
using TelemetryApp.Api.Client.Log;

namespace ApiUtils.Middlewares;

public class ExceptionsMiddleware
{
    private RequestDelegate next;
    private ILoggerClient loggerClient;

    public ExceptionsMiddleware(RequestDelegate next, ILoggerClient loggerClient)
    {
        this.next = next;
        this.loggerClient = loggerClient;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (MusicCollectionApiExceptionBase e)
        {
            context.Response.StatusCode = e.StatusCode;
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
        }
    }
}