using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day03Part1;

public class Day03Part1
{
    private int Run(IEnumerable<string> data)
    {
        const string scores = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return data
            .Select(a =>
                a.Take(a.Length / 2).Intersect(
                    a.Skip(a.Length / 2)
                )
            )
            .Sum(a => scores.IndexOf(a.Single()) + 1);
    }

    private class Day03Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day03Part1/testdata.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(157));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day03Part1/data.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(8240));
        }
    }
}