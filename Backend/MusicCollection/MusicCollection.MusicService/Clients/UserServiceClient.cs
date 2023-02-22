using System.Text;
using System.Text.Json;
using ApiUtils.Extensions;
using MusicCollection.Api.Dto.Exceptions.Users;
using MusicCollection.Api.Dto.Users;

namespace MusicCollection.MusicService.Clients;

public class UserServiceClient : IUserServiceClient
{

    public UserServiceClient(IConfiguration configuration)
    {
        this.configuration = configuration;
        httpClient = new HttpClient();
    }
    public async Task<UserSettings> ReadOrCreateUserSettingsAsync(Guid userId)
    {
        var usersServiceApiUrl = configuration.GetServiceApiUrl("UserService") ??
                                 throw new InvalidOperationException("UserService.Api url is not configured");
        
        var response =  await httpClient.GetAsync(new Uri($"{usersServiceApiUrl}/api/users/{userId}"));
        var userSettingsAsString = await response.Content.ReadAsStringAsync();
        var userSettings = JsonSerializer.Deserialize<UserSettings>(userSettingsAsString);
        if (userSettings == null)
        {
            throw new UserNotFoundException(userId);
        }
        return userSettings;
    }

    public async Task UpdateAsync(Guid userId, UserSettings userSettings)
    {
        var usersServiceApiUrl = configuration.GetSection("UserService").GetSection("ApiUrl").Value ??
                                 throw new InvalidOperationException("UsersService.Api url is not configured");
        var content = new StringContent(JsonSerializer.Serialize(userSettings), Encoding.UTF8, "application/json");
        await httpClient.PostAsync(new Uri($"{usersServiceApiUrl}/api/users/{userId}"), content);
    }

    private readonly IConfiguration configuration;
    private readonly HttpClient httpClient;
}