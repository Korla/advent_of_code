using NUnit.Framework;

namespace aoc2021.Day05Part2;

public class Day05Part2
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(row => row.Split(" -> ").Select(c => c.Split(",").Select(int.Parse).ToList()).ToList())
            .SelectMany(GetPositions)
            .GroupBy(c => c, _ => true)
            .Count(grouping => grouping.Count() > 1);
    }

    private List<(int, int)> GetPositions(IReadOnlyCollection<IReadOnlyCollection<int>> curr)
    {
        var start = curr.First();
        var end = curr.Last();
        var x1 = start.First();
        var y1 = start.Last();
        var x2 = end.First();
        var y2 = end.Last();

        var xRange = (x1 < x2 ? ToEnumerable(x1..(x2 + 1)) : ToEnumerable(x2..(x1 + 1)).Reverse()).ToList();
        var yRange = (y1 < y2 ? ToEnumerable(y1..(y2 + 1)) : ToEnumerable(y2..(y1 + 1)).Reverse()).ToList();
        return (
            xRange.Count == 1 ? yRange.Select(y => (x1, y)) :
            yRange.Count == 1 ? xRange.Select(x => (x, y1))
            : xRange.Zip(yRange, (x, y) => (x, y))
        ).ToList();
    }
    
    private IEnumerable<int> ToEnumerable(Range range)
    {
        for (var i = range.Start.Value; i < range.End.Value; i++)
        {
            yield return i;
        }
    }

    private class Tests
    {
        [Test]
        public void GetPositions()
        {
            var sut = new Day05Part2();
            Assert.AreEqual(new List<(int, int)>{(1,1), (2,2), (3,3)}, sut.GetPositions(new [] {new []{1, 1}, new []{3, 3}}));
            Assert.AreEqual(new List<(int, int)>{(9,7), (8,8), (7,9)}, sut.GetPositions(new [] {new []{9, 7}, new []{7, 9}}));
            Assert.AreEqual(new List<(int, int)>{(1,0), (2,1), (3,2), (4,3)}, sut.GetPositions(new [] {new []{1, 0}, new []{4, 3}}));
            Assert.AreEqual(new List<(int, int)>{(0,1), (1,2), (2,3), (3,4)}, sut.GetPositions(new [] {new []{0, 1}, new []{3, 4}}));
            Assert.AreEqual(new List<(int, int)>{(3,4), (2,3), (1,2), (0,1)}, sut.GetPositions(new [] {new []{3, 4}, new []{0, 1}}));
            Assert.AreEqual(new List<(int, int)>{(0,9), (1,9), (2,9), (3,9), (4,9), (5,9)}, sut.GetPositions(new [] {new []{0, 9}, new []{5, 9}}));
        }

        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day05Part2/testdata.txt");
            var sut = new Day05Part2();
            Assert.AreEqual(12, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day05Part2/data.txt");
            var sut = new Day05Part2();
            Assert.AreEqual(20121, sut.Run(data));
        }
    }
}