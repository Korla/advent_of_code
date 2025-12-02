using Utils;

namespace AoC2025.Day02Part1;

public class Day02Part1
{
    private long Run(IEnumerable<string> data)
    {
        var ranges = data.SelectMany(r => r.Split(",")).Select(p => p.Split("-").Select(double.Parse).ToList())
            .Select(a => new Utils.Range(a.First(), a.Last()))
            .Simplify()
            .ToList();
        return Enumerable.Range(1, (int)Math.Pow(10, 5) - 1).Select(number => long.Parse($"{number}{number}"))
            .Where(candidate => ranges.Any(r => r.Contains(candidate)))
            .Sum();
    }

    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day02Part1/testdata.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1227775554));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day02Part1/data.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(40214376723));
        }
    }
}