using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day09Part2;

public class Day09Part2
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(line => line.Split(' ').Select(int.Parse))
            .Select(GetDifferences)
            .Sum(GetNumberBeforeFirst);
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

    private int GetNumberBeforeFirst(IEnumerable<IEnumerable<int>> differences)
    {
        return differences.Reverse()
            .Aggregate(0, (lastDiff, difference) => difference.First() - lastDiff);
    }

    private class Day09Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day09Part2/testdata.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(2));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day09Part2/data.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1050));
        }
    }
}