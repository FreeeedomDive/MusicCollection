using System.Text.Json;
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
            await WriteExceptionAsync(context, e, e.StatusCode);
        }
        catch (Exception e)
        {
            await WriteExceptionAsync(context, e, 500);
            throw;
        }
    }

    private static async Task WriteExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.StatusCode = statusCode;
        var serializedException = JsonSerializer.Serialize(exception);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(serializedException);
    }
}