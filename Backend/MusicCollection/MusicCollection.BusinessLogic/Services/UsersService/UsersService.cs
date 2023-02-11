using MusicCollection.Api.Dto.Exceptions.Users;
using MusicCollection.Api.Dto.Users;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.UsersService;

public class UsersService : IUsersService
{
    private readonly IUsersRepository usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
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
}