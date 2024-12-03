using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day02Part1;

public class Day02Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(r => r.ToCharArray())
            .Sum(v => v.Last() - 'W' + (v.Last() - v.First() + 2) % 3 * 3);
    }

    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part1/testdata.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(15));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part1/data.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(11475));
        }
    }
}