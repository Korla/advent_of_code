using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace AoC2022.Day04Part1;

public class Day04Part1
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
            .Count(a =>
            {
                var first = a.First();
                var last = a.Last();
                var common = first.Union(last).ToList();
                    
                return common.Count == first.Count || common.Count == last.Count;
            });
    }
    
    private class Day04Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day04Part1/testdata.txt");
            var sut = new Day04Part1();
            Assert.AreEqual(2, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part1/data.txt");
            var sut = new Day04Part1();
            Assert.AreEqual(588, sut.Run(data));
        }
    }
}