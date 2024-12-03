using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day09Part1;

public class Day09Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(line => line.Split(' ').Select(int.Parse))
            .Select(GetDifferences)
            .Sum(GetNumberAfterLast);
    }

    private IEnumerable<IEnumerable<int>> GetDifferences(IEnumerable<int> numbers)
    {
        var differences = new List<IEnumerable<int>> { numbers };
        while (true)
        {
            var nextDifferences = differences.Last()
                .Pairwise((a, b) => b - a)
                .ToList();
            differences.Add(nextDifferences);
            if (nextDifferences.All(d => d == 0))
            {
                return differences;
            }
        }
    }

    private int GetNumberAfterLast(IEnumerable<IEnumerable<int>> differences)
    {
        return differences.Reverse()
            .Aggregate(0, (lastDiff, difference) => lastDiff + difference.Last());
    }

    private class Day09Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day09Part1/testdata.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(114));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day09Part1/data.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1708206096));
        }
    }
}