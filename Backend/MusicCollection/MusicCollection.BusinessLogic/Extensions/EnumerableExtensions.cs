namespace MusicCollection.BusinessLogic.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
    {
        var random = new Random();
        return collection.OrderBy(_ => random.Next(10000000));
    }

    public static IEnumerable<T> ModifyIf<T>(
        this IEnumerable<T> collection,
        bool condition,
        Func<IEnumerable<T>, IEnumerable<T>> func
    )
    {
        return condition ? func(collection) : collection;
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, T element)
    {
        return collection.Except(new[] { element });
    }
}