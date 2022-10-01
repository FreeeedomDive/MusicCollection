﻿using MusicCollection.Api.Dto.Auth;

namespace MusicCollection.BusinessLogic.Services.UsersService;

public interface IUsersService
{
    Task<User> FindAsync(AuthCredentials authCredentials);
    Task CreateAsync(AuthCredentials authCredentials);
}