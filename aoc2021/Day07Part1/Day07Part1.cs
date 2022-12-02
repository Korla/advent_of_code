using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2021.Day07Part1;

public class Day07Part1
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
                .Select(element => Math.Abs(element - value))
                .Sum()
            );
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day07Part1/testdata.txt");
            var sut = new Day07Part1();
            Assert.AreEqual(37, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day07Part1/data.txt");
            var sut = new Day07Part1();
            Assert.AreEqual(336701, sut.Run(data));
        }
    }
}