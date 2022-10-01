using MusicCollection.Api.Dto.FileSystem;

namespace MusicCollection.BusinessLogic.Extensions;

public static class FileSystemNodeExtensions
{
    public static async Task<FileSystemNode> ExtendAsync(
        this FileSystemNode fileSystemNode,
        Func<FileSystemNode, Task> extendAction
    )
    {
        await extendAction(fileSystemNode);
        return fileSystemNode;
    }
}