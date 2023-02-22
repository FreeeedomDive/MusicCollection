using MusicCollection.Api.Dto.Exceptions.Users;
using MusicCollection.Api.Dto.Users;
using MusicCollection.UserService.Repositories.Users.Auth;
using MusicCollection.UserService.Repositories.Users.Personalization;

namespace MusicCollection.UserService.Services.UsersService;

public class UsersService : IUsersService
{
    private readonly IUsersRepository usersRepository;
    private readonly IUserSettingsRepository userSettingsRepository;

    public UsersService(IUsersRepository usersRepository, IUserSettingsRepository userSettingsRepository)
    {
        this.usersRepository = usersRepository;
        this.userSettingsRepository = userSettingsRepository;
    }

    public async Task<User> LoginAsync(AuthCredentials authCredentials)
    {
        return await usersRepository.FindAsync(authCredentials.Login, CryptoService.Encrypt(authCredentials.Password))
               ?? throw new UserNotFoundException(authCredentials.Login);
    }

    public async Task<User> RegisterAsync(AuthCredentials authCredentials)
    {
        var userWithLogin = await usersRepository.FindAsync(authCredentials.Login);
        if (userWithLogin != null)
        {
            throw new UserWithLoginAlreadyExistsException(authCredentials.Login);
        }

        return await usersRepository.CreateAsync(authCredentials.Login,
            CryptoService.Encrypt(authCredentials.Password));
    }

    public async Task<UserSettings> ReadOrCreateSettingsAsync(Guid userId)
    {
        return await userSettingsRepository.ReadOrCreateAsync(userId);
    }

    public async Task UpdateSettingsAsync(Guid userId, UserSettings userSettings)
    {
        await userSettingsRepository.UpdateAsync(userId, userSettings);
    }
}