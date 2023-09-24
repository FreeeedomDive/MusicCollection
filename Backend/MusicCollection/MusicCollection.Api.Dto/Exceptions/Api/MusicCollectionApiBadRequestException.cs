namespace MusicCollection.Api.Dto.Exceptions.Api;

public class MusicCollectionApiBadRequestException : MusicCollectionApiExceptionBase
{
    public MusicCollectionApiBadRequestException(string message) : base(message)
    {
    }

    public override int StatusCode { get; set; } = 400;
}