using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day03Part2;

public class Day03Part2
{
    private int Run(IReadOnlyList<string> data)
    {
        const string scores = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return Enumerable.Range(0, data.Count / 3)
            .Select((_, i) => i * 3)
            .Select(groupStart => data[groupStart].Intersect(data[groupStart + 1]).Intersect(data[groupStart + 2]).Single())
            .Sum(same => scores.IndexOf(same));
    }

    private class Day03Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day03Part2/testdata.txt");
            var sut = new Day03Part2();
            Assert.That(sut.Run(data), Is.EqualTo(70));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day03Part2/data.txt");
            var sut = new Day03Part2();
            Assert.That(sut.Run(data), Is.EqualTo(2587));
        }
    }
}