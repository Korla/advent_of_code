using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day07Part2;

public class Day07Part2
{
    private int Run(IEnumerable<string> data)
    {
        var elements = data.First().Split(",").Select(int.Parse).ToList();
        var start = elements.First();
        var (min, max) = elements.Aggregate(
            (start, start),
            (prev, curr) => (Math.Min(prev.Item1, curr), Math.Max(prev.Item2, curr))
        );
        return Enumerable.Range(min, max + 1)
            .Min(value => elements
                .Select(element => Enumerable.Range(0, Math.Abs(element - value) + 1).Sum())
                .Sum()
            );
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day07Part2/testdata.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(168));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day07Part2/data.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(95167302));
        }
    }
}