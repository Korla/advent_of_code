using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day08Part1;

public class Day08Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .SelectMany(row => row.Split(" | ").Last().Split(" "))
            .GroupBy(digit => digit.Length)
            .Where(digit => digit.Key is 2 or 3 or 4 or 7)
            .Sum(digit => digit.Count());
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day08Part1/testdata.txt");
            var sut = new Day08Part1();
            Assert.That(sut.Run(data), Is.EqualTo(26));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day08Part1/data.txt");
            var sut = new Day08Part1();
            Assert.That(sut.Run(data), Is.EqualTo(352));
        }
    }
}