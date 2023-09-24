using MusicCollection.Api.Dto.Exceptions.Api;

namespace MusicCollection.Api.Dto.Exceptions.Files;

public class ReadFilesFromNonDirectoryException : MusicCollectionApiConflictException
{
    public ReadFilesFromNonDirectoryException(Guid parentId) :
        base($"Forbidden try to read files from non-directory {parentId}")
    {
    }
}