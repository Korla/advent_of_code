using Utils;

namespace AoC2025.Day05Part1;

public class Day05Part1
{
    private long Run(IEnumerable<string> data)
    {
        var dataList = data.ToList();
        var ranges = dataList.TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .Select(line => new Utils.Range(line[0], line[1]))
            .ToList();
        var simplified = ranges.Simplify().ToList();
        return dataList.Skip(ranges.Count + 1)
            .Select(long.Parse)
            .Count(number => simplified.Any(range => range.Contains(number)));
    }

    private class Day05Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day05Part1/testdata.txt");
            var sut = new Day05Part1();
            Assert.That(sut.Run(data), Is.EqualTo(3));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day05Part1/data.txt");
            var sut = new Day05Part1();
            Assert.That(sut.Run(data), Is.EqualTo(505));
        }
    }
}