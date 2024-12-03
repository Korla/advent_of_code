using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day02Part2;

public class Day02Part2
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Count(
                row =>
                {
                    var integers = row
                        .Split(" ")
                        .Select(int.Parse)
                        .ToList();
                    var deltasCount = integers.Count;
                    return IncreasingOrDecreasing(integers) ||
                           Enumerable.Range(0, deltasCount)
                               .Select(i =>
                                   integers
                                       .Select((integer, i) => (integer, i))
                                       .Where(integer => integer.i != i)
                                       .Select(integer => integer.integer)
                               )
                               .Any(IncreasingOrDecreasing);
                }
            );
    }

    private bool IncreasingOrDecreasing(IEnumerable<int> deltas)
    {
        deltas = deltas
            .Pairwise((a, b) => b - a)
            .ToList();
        var isIncreasing = deltas.First() > 0;
        return deltas.All(delta =>
            isIncreasing ? delta.IsBetweenInclusive(1, 3) : delta.IsBetweenInclusive(-3, -1));
    }

    private class Day02Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day02Part2/testdata.txt");
            var sut = new Day02Part2();
            Assert.That(sut.Run(data), Is.EqualTo(4));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day02Part2/data.txt");
            var sut = new Day02Part2();
            Assert.That(sut.Run(data), Is.EqualTo(418));
        }
    }
}