namespace MusicCollection.Api.Dto.Exceptions;

public class ReadFilesFromNonDirectoryException : MusicCollectionApiExceptionBase
{
    public ReadFilesFromNonDirectoryException(Guid parentId) : base
    (
        $"Forbidden try to read files from non-directory {parentId}"
    )
    {
    }
}