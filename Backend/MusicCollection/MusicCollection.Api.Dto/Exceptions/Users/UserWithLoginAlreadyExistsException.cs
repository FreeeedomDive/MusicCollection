using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Users;

public class UserWithLoginAlreadyExistsException : MusicCollectionApiConflictException
{
    public UserWithLoginAlreadyExistsException(string login): base($"User with login {login} already exists") { }
}