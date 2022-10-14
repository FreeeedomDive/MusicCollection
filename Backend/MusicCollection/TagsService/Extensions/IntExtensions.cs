namespace MusicCollection.Common.TagsService.Extensions;

public static class IntExtensions
{
    public static string NormalizeToLength(this int number, int length)
    {
        var stringInput = number.ToString();
        var leadingZeros = new string('0', length - stringInput.Length);
        return string.Concat(leadingZeros, stringInput);
    }
}