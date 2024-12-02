using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2024.Day01Part2;

public class Day01Part2
{
    private int Run(IEnumerable<string> data)
    {
        var lists = data.Aggregate(
            (new List<int>(), new List<int>()),
            (prev, curr) =>
            {
                var parts = curr.Split("   ").Select(int.Parse).ToList();
                prev.Item1.Add(parts.First());
                prev.Item2.Add(parts.Last());
                return prev;
            });
        return lists.Item1
            .Sum(i => lists.Item2.Count(a => a == i) * i);
    }
    
    private class Day01Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day01Part2/testdata.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(31));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day01Part2/data.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(21142653));
        }
    }
}