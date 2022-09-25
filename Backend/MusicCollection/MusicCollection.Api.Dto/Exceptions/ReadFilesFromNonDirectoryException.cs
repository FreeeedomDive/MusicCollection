namespace MusicCollection.Api.Dto.Exceptions;

public class ReadFilesFromNonDirectoryException : MusicCollectionApiExceptionBase
{
    public Guid ParentId { get; set; }

    public ReadFilesFromNonDirectoryException(Guid parentId)
    {
        ParentId = parentId;
    }
}