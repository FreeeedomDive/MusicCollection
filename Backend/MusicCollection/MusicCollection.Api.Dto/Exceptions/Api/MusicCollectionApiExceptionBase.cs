namespace MusicCollection.Api.Dto.Exceptions;

public abstract class MusicCollectionApiExceptionBase : Exception
{
    protected MusicCollectionApiExceptionBase(string message): base(message) { }

    public virtual int StatusCode { get; set; }
}