using NUnit.Framework;

namespace Utils;

public static class EnumerableExtensions
{
    public static int Multiply(this IEnumerable<int> source) => source.Aggregate(1, (prev, curr) => prev * curr);
    public static long Multiply(this IEnumerable<long> source) => source.Aggregate((prev, curr) => prev * curr);
    public static int Multiply<T>(this IEnumerable<T> source, Func<T, int> func) => source.Aggregate(1, (prev, curr) => prev * func(curr));

    public static IEnumerable<T> Log<T>(this IEnumerable<T> source)
    {
        Console.WriteLine();
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

    public static IEnumerable<(TSource, TSource)> AllPairs<TSource>(this IEnumerable<TSource> source)
    {
        var sourceList = source.ToList();
        for (var i = 0; i < sourceList.Count - 1; i++)
        {
            for (var j = i + 1; j < sourceList.Count; j++)
            {
                yield return (sourceList[i], sourceList[j]);
            }
        }
    }

    public static IEnumerable<string> Flip(this IEnumerable<string> source)
    {
        return source.Flip<char>()
            .Select(a => string.Join("", a));
    }

    public static IEnumerable<IEnumerable<T>> Flip<T>(this IEnumerable<IEnumerable<T>> source)
    {
        return source
            .SelectMany(s => s.Select((c, i) => (c, i)))
            .GroupBy(a1 => a1.i, a2 => a2.c);
    }

    public static IEnumerable<string> RotateClockWise(this IEnumerable<string> s)
    {
        var source = s.ToList();
        var width = source.First().Length;
        return source
            .SelectMany((row, y) => row.Select((value, x) => (value, y: x, x: width - 1 - y)))
            .OrderBy(e => e.x)
            .GroupBy(e => e.y, e => e.value)
            .Select(g => string.Join("", g))
            .ToList();
    }

    public static IEnumerable<IEnumerable<T>> RotateClockWise<T>(this IEnumerable<IEnumerable<T>> s)
    {
        var source = s.ToList();
        var width = source.First().Count();
        return source
            .SelectMany((row, y) => row.Select((value, x) => (value, y: x, x: width - 1 - y)))
            .OrderBy(e => e.x)
            .GroupBy(e => e.y, e => e.value)
            .Select(g => g.ToList())
            .ToList();
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
            Assert.That(new List<string> { "123", "456", "789" }.Flip(), Is.EqualTo(new List<string> { "147", "258", "369" }));
        }

        [Test]
        public void RotateClockWise()
        {
            Assert.That(new List<string>
            {
                "123",
                "456",
                "789"
            }.RotateClockWise(), Is.EqualTo(new List<string>
            {
                "741",
                "852",
                "963"
            }));
            Assert.That(new List<string>
            {
                "12",
                "45",
                "78"
            }.RotateClockWise(), Is.EqualTo(new List<string>
            {
                "741",
                "852"
            }));
        }

        [Test]
        public void SlidingWindowValues()
        {
            var data = new List<int> { 1, 2, 3 }.SlidingWindowValues(2);
            Assert.That(new List<List<int>> { new() { 1, 2 }, new() { 2, 3 } }, Is.EqualTo(data));
        }
    }
}