using Utils;

namespace AoC2025.Day05Part2;

public class Day05Part2
{
    private double Run(IEnumerable<string> data)
    {
        var dataList = data.ToList();
        var ranges = dataList.TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split('-').Select(long.Parse).ToArray())
            .Select(line => new Utils.Range(line[0], line[1]))
            .ToList();
        return ranges.Simplify().Sum(range => range.Size());
    }

    private class Day05Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day05Part2/testdata.txt");
            var sut = new Day05Part2();
            Assert.That(sut.Run(data), Is.EqualTo(14));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day05Part2/data.txt");
            var sut = new Day05Part2();
            Assert.That(sut.Run(data), Is.EqualTo(505));
        }
    }
}