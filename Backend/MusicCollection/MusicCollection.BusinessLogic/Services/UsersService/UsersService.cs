using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;
using MusicCollection.BusinessLogic.Repositories.Files;
using MusicCollection.BusinessLogic.Services.UsersService;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public class AuthService : IUsersService
{
    private readonly IUsersRepository usersRepository;

    public AuthService(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }
    public async Task<User> ReadAsync(Guid id)
    {
        return await usersRepository.ReadAsync(id);
    }

    public async Task<User> ReadAsync(AuthCredentials authCredentials)
    {
        return await usersRepository.ReadAsync(authCredentials.Login, 
            CryptoService.Encrypt(authCredentials.Password));
    }

    public async Task<User?> TryReadAsync(Guid id)
    {
        return await usersRepository.TryReadAsync(id);
    }

    public async Task CreateAsync(AuthCredentials authCredentials)
    {
        await usersRepository.CreateAsync(authCredentials.Login, authCredentials.Password);
    }
}