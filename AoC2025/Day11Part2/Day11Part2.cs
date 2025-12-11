namespace AoC2025.Day11Part2;

public class Day11Part2
{
    private long Run(IEnumerable<string> d)
    {
        var graph = d.Select(d => d.Replace(":", "").Split(' '))
            .ToDictionary(s => s.First(), s => s.Skip(1));
        Dictionary<string, long> dacCache = new();
        Dictionary<string, long> fftCache = new();
        Dictionary<string, long> outCache = new();
        return Count(graph, "svr", "dac", dacCache) * Count(graph, "dac", "fft", fftCache) * Count(graph, "fft", "out", outCache) +
               Count(graph, "svr", "fft", fftCache) * Count(graph, "fft", "dac", dacCache) * Count(graph, "dac", "out", outCache);
    }

    private static long Count(
        Dictionary<string, IEnumerable<string>> graph,
        string current,
        string end,
        Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(current, out var cached))
        {
            return cached;
        }

        if (current == end)
        {
            return cache[current] = 1;
        }

        if (!graph.TryGetValue(current, out var destinations))
        {
            return cache[current] = 0;
        }

        return cache[current] = destinations.Sum(next => Count(graph, next, end, cache));
    }

    private class Day11Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day11Part2/testdata.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(2));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day11Part2/data.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(306594217920240));
        }
    }
}