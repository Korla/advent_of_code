using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2022.Day04Part2;

public class Day04Part2
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(a =>
                a.Split(',')
                    .Select(b => b.Split('-').Select(int.Parse).ToList())
                    .Select(c => Enumerable.Range(c.First(), c.Last() - c.First() + 1).ToList())
                    .ToList()
            )
            .Count(a => a.First().Intersect(a.Last()).Any());
    }
    
    private class Day04Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day04Part2/testdata.txt");
            var sut = new Day04Part2();
            Assert.AreEqual(4, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part2/data.txt");
            var sut = new Day04Part2();
            Assert.AreEqual(911, sut.Run(data));
        }
    }
}