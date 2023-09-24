using Microsoft.AspNetCore.Http;
using MusicCollection.Api.Dto.Exceptions;
using Newtonsoft.Json;
using TelemetryApp.Api.Client.Log;

namespace ApiUtils.Middlewares;

public class ExceptionsMiddleware
{
    public ExceptionsMiddleware(RequestDelegate next, ILoggerClient loggerClient)
    {
        this.next = next;
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
        var result = JsonConvert.SerializeObject(
            exception, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            }
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(result);
    }

    private readonly RequestDelegate next;
}