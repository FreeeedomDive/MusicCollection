namespace MusicCollection.BusinessLogic.Utils;

public static class MusicFileExtensions
{
    public static readonly string[] SupportedFileExtensions = new[] { "mp3", "flac", "m4a" }
        .SelectMany(x => new[] { x, $".{x}" })
        .ToArray();

    public static Dictionary<string, string> ExtensionToMimeType()
    {
        return SupportedFileExtensions.ToDictionary(x => x, x => x == "mp3" ? "audio/mpeg" : $"audio/{x}");
    }
}