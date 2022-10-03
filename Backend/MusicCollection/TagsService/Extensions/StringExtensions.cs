namespace TagExtractor.Extensions;

public static class StringExtensions
{
    public static string GetFileExtension(this string path)
    {
        return path.Split(".").Last();
    }
}