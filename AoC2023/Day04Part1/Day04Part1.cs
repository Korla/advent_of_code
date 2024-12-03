using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day04Part1;

public class Day04Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(row => row.Split(": ").Last().Split(" | "))
            .Select(numberGroups => numberGroups
                .Select(numberGroup => numberGroup.Split(" ")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(number => int.Parse(number.Trim()))
                ).ToList()
            )
            .Sum(numbers => (int)Math.Pow(2, numbers.First().Intersect(numbers.Last()).Count() - 1));
    }

    private class Day04Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day04Part1/testdata.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(13));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part1/data.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(18519));
        }
    }
}