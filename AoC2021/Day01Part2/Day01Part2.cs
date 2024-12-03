using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day01Part2;

public class Day01Part2
{
    private int Run(IEnumerable<string> data)
    {
        var values = data
            .Select(int.Parse)
            .ToList();
        var sums = values
            .Zip(
                values.Skip(1),
                (first, second) => (first, second)
            )
            .Zip(
                values.Skip(2),
                (firstTwo, third) => (firstTwo.first, firstTwo.second, third)
            )
            .Select(group => group.first + group.second + group.third)
            .ToList();
        return sums
            .Zip(
                sums.Skip(1),
                (first, second) => second > first
            )
            .Count(a => a);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day01Part2/testdata.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(5));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day01Part2/data.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1486));
        }
    }
}