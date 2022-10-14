namespace MusicCollection.BusinessLogic.Extensions;

public static class StringExtensions
{
    public static string GetFileExtension(this string path)
    {
        return new FileInfo(path).Extension;
    }

    public static bool IsSupportedExtension(this string path)
    {
        var extension = path.GetFileExtension();
        return Extensions.Contains(extension);
    }

    private static readonly string[] Extensions = new[] { "mp3", "flac" }
        .SelectMany(x => new[] { x, $".{x}" })
        .ToArray();
}