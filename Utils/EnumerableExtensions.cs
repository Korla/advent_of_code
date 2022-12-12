namespace Utils;

public static class EnumerableExtensions
{
    public static int Multiply(this IEnumerable<int> source) => source.Aggregate(1, (prev, curr) => prev * curr);
    public static int Multiply<T>(this IEnumerable<T> source, Func<T, int> func) => source.Aggregate(1, (prev, curr) => prev * func(curr));

    public static IEnumerable<IList<T>> SlidingWindowValues<T>(this IEnumerable<T> source, int windowSize)
    {
        var windows = Enumerable.Range(0, windowSize)
            .Select(_ => new List<T>())
            .ToList();

        var i = 0;
        using var iter = source.GetEnumerator();
        while (iter.MoveNext())
        {
            var c = Math.Min(i + 1, windowSize);
            for (var j = 0; j < c; j++)
            {
                windows[(i - j) % windowSize].Add(iter.Current);
            }
            if (i >= windowSize - 1)
            {
                var previous = (i + 1) % windowSize;
                yield return windows[previous];
                windows[previous] = new List<T>();
            }
            i++;
        }
    }
}