using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace AoC2021.Day14Part1;

public class Day14Part1
{
    private int Run(IList<string> data)
    {
        var start = data.First();
        var map = data.Skip(2).Select(s => s.Split(" -> ")).ToDictionary(c => c.First(), c => c.First()[..1] + c.Last() + c.First()[1..]);
        var res = Enumerable.Range(0, 10).Aggregate(start, (prev, _) => InsertBetween(prev, map).Replace("_", ""));
        var a = res.GroupBy(a => a).OrderBy(g => g.Count());
        var letterCounts = a.Select(g => g.Count()).ToList();
        return letterCounts.Last() - letterCounts.First();
    }

    private string InsertBetween(string input, Dictionary<string, string> map)
    {
        var pairs = input.Zip(input.Skip(1), (a, b) => (a, b)).Select((t, i) =>
        {
            var pair = new string(new[] {t.a, t.b});
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
            Assert.AreEqual(1588, sut.Run(data));
        }

        [Test]
        public void InsertBetween()
        {
            Assert.AreEqual("ABA", new Day14Part1().InsertBetween("AA", new Dictionary<string, string> {{"AA", "ABA"}}));
            Assert.AreEqual("AABAA", new Day14Part1().InsertBetween("ABA", new Dictionary<string, string> {{"AB", "AAB"}, {"BA", "BAA"}}));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day14Part1/data.txt");
            var sut = new Day14Part1();
            Assert.AreEqual(2345, sut.Run(data));
        }
    }
}