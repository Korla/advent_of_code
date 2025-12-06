using NUnit.Framework;

namespace Utils;

public static class EnumerableExtensions
{
    public static int Multiply(this IEnumerable<int> source) => source.Aggregate(1, (prev, curr) => prev * curr);
    public static long Multiply(this IEnumerable<long> source) => source.Aggregate((prev, curr) => prev * curr);
    public static int Multiply<T>(this IEnumerable<T> source, Func<T, int> func) => source.Aggregate(1, (prev, curr) => prev * func(curr));

    public static IEnumerable<T> Log<T>(this IEnumerable<T> source)
    {
        Console.WriteLine(string.Join("\n", source.Select(e => e?.ToString())));
        return source;
    }

    public static string FromCharArrayToString(this IEnumerable<char> source) => new(source.ToArray());

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

    public static IEnumerable<TResult> Pairwise<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> resultSelector)
    {
        var previous = default(TSource);

        using var it = source.GetEnumerator();
        if (it.MoveNext())
            previous = it.Current;

        while (it.MoveNext())
            yield return resultSelector(previous, previous = it.Current);
    }

    public static IEnumerable<string> Flip(this IEnumerable<string> source)
    {
        return source.Flip<char>()
            .Select(a => string.Join("", a.Select(b => b)));
    }

    public static IEnumerable<IEnumerable<T>> Flip<T>(this IEnumerable<IEnumerable<T>> source)
    {
        return source
            .SelectMany(s => s.Select((c, i) => (c, i)))
            .GroupBy(a1 => a1.i, a2 => a2.c);
    }

    public static void Log(this IEnumerable<string> source)
    {
        Console.WriteLine(string.Join(", ", source));
    }

    private class EnumerableExtensionsTests
    {
        [Test]
        public void Pairwise()
        {
            var data = new List<int> { 1, 2, 3 }.Pairwise((a, b) => (a, b));
            Assert.That(new List<(int, int)> { (1, 2), (2, 3) }, Is.EqualTo(data));
        }

        [Test]
        public void Flip()
        {
            Assert.That(new List<string> { "12", "34" }.Flip(), Is.EqualTo(new List<string> { "13", "24" }));
        }

        [Test]
        public void SlidingWindowValues()
        {
            var data = new List<int> { 1, 2, 3 }.SlidingWindowValues(2);
            Assert.That(new List<List<int>> { new() { 1, 2 }, new() { 2, 3 } }, Is.EqualTo(data));
        }
    }
}