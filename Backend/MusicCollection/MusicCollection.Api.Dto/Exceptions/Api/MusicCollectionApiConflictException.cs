namespace MusicCollection.Api.Dto.Exceptions.Api;

public class MusicCollectionApiConflictException : MusicCollectionApiExceptionBase
{
    public MusicCollectionApiConflictException(string message) : base(message)
    {
    }

    public override int StatusCode { get; set; } = 409;
}