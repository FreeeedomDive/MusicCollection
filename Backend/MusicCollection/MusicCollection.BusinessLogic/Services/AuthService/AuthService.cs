﻿using MusicCollection.Api.Dto.Auth;
using MusicCollection.BusinessLogic.Repositories;
using MusicCollection.BusinessLogic.Repositories.Auth;
using MusicCollection.BusinessLogic.Repositories.Database;
using MusicCollection.BusinessLogic.Repositories.Files;

namespace MusicCollection.BusinessLogic.Services.AuthService;

public class AuthService : IAuthService
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

    public async Task<User?> TryReadAsync(Guid id)
    {
        return await usersRepository.TryReadAsync(id);
    }

    public async Task CreateOrUpdateAsync(User user)
    {
        await usersRepository.CreateOrUpdateAsync(user);
    }

    public async Task CreateAsync(User user)
    {
        await usersRepository.CreateAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await usersRepository.UpdateAsync(user);
    }

    public async Task<bool> TryDeleteAsync(User user)
    {
        return await usersRepository.TryDeleteAsync(user);
    }
}