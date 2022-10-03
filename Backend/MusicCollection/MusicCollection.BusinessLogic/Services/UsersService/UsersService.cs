using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories.Auth;

namespace MusicCollection.BusinessLogic.Services.UsersService;

public class UsersService : IUsersService
{
    private readonly IUsersRepository usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }

    public async Task<User> FindAsync(AuthCredentials authCredentials)
    {
        return await usersRepository.FindAsync(
            authCredentials.Login,
            CryptoService.Encrypt(authCredentials.Password)
        );
    }

    public async Task<User> CreateAsync(AuthCredentials authCredentials)
    {
        return await usersRepository.CreateAsync(authCredentials.Login, CryptoService.Encrypt(authCredentials.Password));
    }
}