using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day06Part2;

public class Day06Part2
{
    private long Run(IEnumerable<string> data, int nbrOfGenerations)
    {
        var s = data.First().Split(",").GroupBy(d => d).ToDictionary(d => int.Parse(d.Key), d => (long)d.Count());
        var buckets = Enumerable.Range(0, 9)
            .Select(start => s.ContainsKey(start) ? s[start] : 0)
            .ToList();
        for (var i = 0; i < nbrOfGenerations; i++)
        {
            var tmp = buckets[0];
            buckets[0] = buckets[1];
            buckets[1] = buckets[2];
            buckets[2] = buckets[3];
            buckets[3] = buckets[4];
            buckets[4] = buckets[5];
            buckets[5] = buckets[6];
            buckets[6] = buckets[7] + tmp;
            buckets[7] = buckets[8];
            buckets[8] = tmp;
        }

        return buckets.Aggregate((a, b) => a + b);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day06Part2/testdata.txt");
            var sut = new Day06Part2();
            Assert.That(sut.Run(data, 256), Is.EqualTo(26984457539));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day06Part2/data.txt");
            var sut = new Day06Part2();
            Assert.That(sut.Run(data, 256), Is.EqualTo(1682576647495));
        }
    }
}