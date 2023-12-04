using NUnit.Framework;
using Utils;

namespace AoC2023.Day04Part2;

public class Day04Part2
{
    private int Run(IEnumerable<string> data)
    {
        var winCounts = new Dictionary<int, int>();
        var counts = data
            .Select(row => row.Split(": ").Last().Split(" | "))
            .Select(numberGroups => numberGroups
                .Select(numberGroup => numberGroup.Split(" ")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(number => int.Parse(number.Trim()))
                ).ToList()
            )
            .Select((numbers, i) =>
            {
                var instanceCount = 1 + (winCounts.TryGetValue(i, out var count) ? count : 0);
                var matches = numbers.First().Intersect(numbers.Last()).Count();
                foreach (var i2 in Enumerable.Range(0, matches))
                {
                    winCounts.IncreaseOrAdd(i + i2 + 1, instanceCount);
                }

                return instanceCount;
            })
            .Sum();
        return counts;
    }
    
    private class Day04Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day04Part2/testdata.txt");
            var sut = new Day04Part2();
            Assert.AreEqual(30, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part2/data.txt");
            var sut = new Day04Part2();
            Assert.AreEqual(11787590, sut.Run(data));
        }
    }
}