using MusicCollection.BusinessLogic.Utils;

namespace MusicCollection.BusinessLogic.Extensions;

public static class StringExtensions
{
    public static string GetFileExtension(this string path)
    {
        var extension = new FileInfo(path).Extension;
        return extension.StartsWith(".") ? extension[1..] : extension;
    }

    public static bool IsSupportedExtension(this string path)
    {
        var extension = path.GetFileExtension();
        return MusicFileExtensions.SupportedFileExtensions.Contains(extension);
    }
}