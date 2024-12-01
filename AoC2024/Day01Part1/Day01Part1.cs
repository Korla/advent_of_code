using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2024.Day01Part1;

public class Day01Part1
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
        lists.Item1.Sort();
        lists.Item2.Sort();
        return lists.Item1.Zip(lists.Item2, (item1, item2) => Math.Abs(item1 - item2)).Sum();
    }
    
    private class Day01Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day01Part1/testdata.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(11));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day01Part1/data.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(2285373));
        }
    }
}