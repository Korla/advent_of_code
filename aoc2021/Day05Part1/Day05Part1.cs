using NUnit.Framework;

namespace aoc2021.Day05Part1;

public class Day05Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(row => row.Split(" -> ").Select(c => c.Split(",").Select(int.Parse).ToList()).ToList())
            .Aggregate(new Dictionary<(int, int), int>(), (prev, curr) =>
            {
                var start = curr.First();
                var end = curr.Last();
                foreach (var coord in GetPositions(start, end))
                {
                    prev.TryGetValue(coord, out var previousValue);
                    prev[coord] = previousValue + 1;
                }

                return prev;
            })
            .Count(s => s.Value > 1);
    }

    private List<(int, int)> GetPositions(IReadOnlyCollection<int> start, IReadOnlyCollection<int> end)
    {
        var x1 = start.First();
        var y1 = start.Last();
        var x2 = end.First();
        var y2 = end.Last();
        if (x1 > x2)
        {
            (x1, x2) = (x2, x1);
        }
        if (y1 > y2)
        {
            (y1, y2) = (y2, y1);
        }
        return
            x1 == x2 ? Enumerable.Range(y1, y2 - y1 + 1).Select(y => (x1, y)).ToList() :
            y1 == y2 ? Enumerable.Range(x1, x2 - x1 + 1).Select(x => (x, y1)).ToList() :
            new List<(int, int)>();
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day05Part1/testdata.txt");
            var sut = new Day05Part1();
            Assert.AreEqual(5, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day05Part1/data.txt");
            var sut = new Day05Part1();
            Assert.AreEqual(6710, sut.Run(data));
        }
    }
}