namespace MusicCollection.Api.Dto.Exceptions;

public class MusicCollectionApiExceptionBase : Exception
{
    protected MusicCollectionApiExceptionBase(string message): base(message)
    {
    }
}