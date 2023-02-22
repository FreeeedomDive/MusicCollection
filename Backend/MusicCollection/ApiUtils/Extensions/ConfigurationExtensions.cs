using Microsoft.Extensions.Configuration;

namespace ApiUtils.Extensions;

public static class ConfigurationExtensions
{
    public static string GetServiceApiUrl(this IConfiguration configuration, string serviceName)
    {
        return configuration.GetSection(serviceName).GetSection("ApiUrl").Value;
    }
}