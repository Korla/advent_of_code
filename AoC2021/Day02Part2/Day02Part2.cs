using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day02Part2;

public class Day02Part2
{
    private int Run(IEnumerable<string> data)
    {
        var values = data
            .Select(s => s.Split(" "));
        var (hor, vert, _) = values
            .Select(parts => (parts[0][0], int.Parse(parts[1])))
            .Aggregate(
                (0, 0, 0),
                (prev, curr) =>
                {
                    switch (curr.Item1)
                    {
                        case 'f': return (prev.Item1 + curr.Item2, prev.Item2 + curr.Item2 * prev.Item3, prev.Item3);
                        case 'd': return (prev.Item1, prev.Item2, prev.Item3 + curr.Item2);
                        case 'u': return (prev.Item1, prev.Item2, prev.Item3 - curr.Item2);
                    }

                    return prev;
                }
            );
        return hor * vert;
    }

    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part2/testdata.txt");
            var sut = new Day02Part2();
            Assert.That(sut.Run(data), Is.EqualTo(900));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part2/data.txt");
            var sut = new Day02Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1408487760));
        }
    }
}