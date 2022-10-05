namespace MusicCollection.Api.Dto.Exceptions;

public class UserWithLoginAlreadyExistsException : Exception
{
    public UserWithLoginAlreadyExistsException(string login): base($"User with login {login} already exists")
    {
        Login = login;
    }
    
    public string Login { get; }
}