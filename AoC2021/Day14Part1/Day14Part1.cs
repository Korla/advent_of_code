using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day14Part1;

public class Day14Part1
{
    private int Run(IList<string> data)
    {
        var start = data.First();
        var map = data.Skip(2).Select(s => s.Split(" -> "))
            .ToDictionary(c => c.First(), c => c.First()[..1] + c.Last() + c.First()[1..]);
        var res = Enumerable.Range(0, 10).Aggregate(start, (prev, _) => InsertBetween(prev, map).Replace("_", ""));
        var a = res.GroupBy(a => a).OrderBy(g => g.Count());
        var letterCounts = a.Select(g => g.Count()).ToList();
        return letterCounts.Last() - letterCounts.First();
    }

    private string InsertBetween(string input, Dictionary<string, string> map)
    {
        var pairs = input.Zip(input.Skip(1), (a, b) => (a, b)).Select((t, i) =>
        {
            var pair = new string(new[] { t.a, t.b });
            var result = map.TryGetValue(pair, out var res) ? res : pair;
            return i != 0 && result.Length == 3 ? result[1..] : result;
        });
        return string.Join("", pairs);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day14Part1/testdata.txt");
            var sut = new Day14Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1588));
        }

        [Test]
        public void InsertBetween()
        {
            Assert.That(new Day14Part1().InsertBetween("AA", new Dictionary<string, string> { { "AA", "ABA" } }),
                Is.EqualTo("ABA"));
            Assert.That(new Day14Part1().InsertBetween("ABA",
                    new Dictionary<string, string> { { "AB", "AAB" }, { "BA", "BAA" } }),
                Is.EqualTo("AABAA"));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day14Part1/data.txt");
            var sut = new Day14Part1();
            Assert.That(sut.Run(data), Is.EqualTo(2345));
        }
    }
}