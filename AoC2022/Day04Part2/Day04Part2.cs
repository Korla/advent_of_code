using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day04Part2;

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
            Assert.That(sut.Run(data), Is.EqualTo(4));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part2/data.txt");
            var sut = new Day04Part2();
            Assert.That(sut.Run(data), Is.EqualTo(911));
        }
    }
}