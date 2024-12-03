using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day01Part2;

public class Day01Part2
{
    private readonly Dictionary<string, string> _digitMap = new()
    {
        {"1", "1"},
        {"2", "2"},
        {"3", "3"},
        {"4", "4"},
        {"5", "5"},
        {"6", "6"},
        {"7", "7"},
        {"8", "8"},
        {"9", "9"},
        {"one", "1"},
        {"two", "2"},
        {"three", "3"},
        {"four", "4"},
        {"five", "5"},
        {"six", "6"},
        {"seven", "7"},
        {"eight", "8"},
        {"nine", "9"},
    };

    private int Run(IEnumerable<string> data) =>
        data
            .Select(line => line
                .Select((_, i) => line[i..])
                .Select(substring => _digitMap.FirstOrDefault(d => substring.StartsWith(d.Key)).Value)
                .Where(v => v != null)
                .ToList()
            )
            .Sum(integers => int.Parse($"{integers.First()}{integers.Last()}"));

    private class Day01Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day01Part2/testdata.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(281));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day01Part2/data.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(54581));
        }
    }
}