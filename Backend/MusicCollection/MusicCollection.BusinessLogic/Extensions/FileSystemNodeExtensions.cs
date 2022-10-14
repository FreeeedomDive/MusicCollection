using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Extensions;

public static class FileSystemNodeExtensions
{
    public static FileSystemNode Extend(
        this FileSystemNode fileSystemNode,
        Action<FileSystemNode> extendAction
    )
    {
        extendAction(fileSystemNode);
        return fileSystemNode;
    }
}