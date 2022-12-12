using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day14Part2;

public class Day14Part2
{
    private long Run(IList<string> data)
    {
        var startString = data.First();
        var map = CreateMap(data);
        var counts = map.Keys.ToDictionary(key => key, _ => (long)0);
        foreach (var key in startString.Zip(startString.Skip(1), (a, b) => (a, b)))
        {
            counts[key] += 1;
        }
        foreach (var _ in Enumerable.Range(0, 40))
        {
            var nextCounts = map.Keys.ToDictionary(key => key, _ => (long)0);
            foreach (var (key, count) in counts)
            {
                foreach (var to in map[key])
                {
                    nextCounts[to] += 1 * count;
                }
            }

            counts = nextCounts;
        }
        return CountTotal(counts, startString.Last());
    }

    private long CountTotal(Dictionary<(char, char), long> counts, char last)
    {
        var groups = counts.GroupBy(
            a => a.Key.Item1,
            a => a.Value
        );
        var letterCounts = groups.Select(c => c.Key == last ? c.Sum() + 1 : c.Sum()).OrderBy(a => a).ToList();
        return letterCounts.Last() - letterCounts.First();
    }

    private Dictionary<(char, char), List<(char, char)>> CreateMap(IEnumerable<string> data)
    {
        return data.Skip(2).Select(s => s.Split(" -> "))
            .ToDictionary(
                c => (c.First()[0], c.First()[1]),
                c => new [] { (c.First()[0], c.Last()[0]), (c.Last()[0], c.First()[1]) }.ToList()
            );
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day14Part2/testdata.txt");
            var sut = new Day14Part2();
            Assert.AreEqual(2188189693529, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day14Part2/data.txt");
            var sut = new Day14Part2();
            Assert.AreEqual(2432786807053, sut.Run(data));
        }
    }
}