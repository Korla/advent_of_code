using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day01Part2;

public class Day01Part2
{
    private int Run(IEnumerable<string> data)
    {
        var values = data
            .ToList();
        var elves = new List<int>();
        var current = 0;
        foreach (var value in values)
        {
            if (string.IsNullOrEmpty(value))
            {
                elves.Add(current);
                current = 0;
            }
            else
            {
                current += int.Parse(value);
            }
        }
        elves.Add(current);

        return elves.OrderByDescending(a => a).Take(3).Sum();
    }

    private class Day01Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day01Part2/testdata.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(45000));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day01Part2/data.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(205615));
        }
    }
}