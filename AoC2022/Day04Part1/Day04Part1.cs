using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Assert.That(sut.Run(data), Is.EqualTo(2));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part1/data.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(588));
        }
    }
}