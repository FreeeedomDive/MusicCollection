namespace MusicCollection.Api.Dto.Exceptions.Api;

public class MusicCollectionApiNotFoundException : MusicCollectionApiExceptionBase
{
    public MusicCollectionApiNotFoundException(string message) : base(message) { }

    public override int StatusCode { get; set; } = 404;
}