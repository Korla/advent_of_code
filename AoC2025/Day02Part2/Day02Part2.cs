using Utils;

namespace AoC2025.Day02Part2;

public class Day02Part2
{
    private long Run(IEnumerable<string> data)
    {
        var ranges = data.SelectMany(r => r.Split(",")).Select(p => p.Split("-").Select(double.Parse).ToList())
            .Select(a => new Utils.Range(a.First(), a.Last()))
            .Simplify()
            .ToList();
        return GenerateInvalidCandidates()
            .Where(candidate => ranges.Any(r => r.Contains(candidate)))
            .Sum();
    }

    private static HashSet<long> GenerateInvalidCandidates()
    {
        var set = new HashSet<long>();
        foreach (var number in Enumerable.Range(1, (int)Math.Pow(10, 5) - 1))
        {
            var numberString = number.ToString();
            var candidate = numberString;
            while ((candidate = $"{candidate}{numberString}").Length <= 10)
            {
                set.Add(long.Parse(candidate));
            }
        }

        return set;
    }

    private class Day02Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day02Part2/testdata.txt");
            var sut = new Day02Part2();
            Assert.That(sut.Run(data), Is.EqualTo(4174379265));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day02Part2/data.txt");
            var sut = new Day02Part2();
            Assert.That(sut.Run(data), Is.EqualTo(50793864718));
        }
    }
}