using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day01Part1;

public class Day01Part1
{
    private int Run(IEnumerable<string> data)
    {
        var values = data
            .Select(int.Parse)
            .ToList();
        return values
            .Zip(
                values.Skip(1),
                (first, second) => second > first
            )
            .Count(a => a);
    }

    private class Day01Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day01Part1/testdata.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(7));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day01Part1/data.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1446));
        }
    }
}