using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day18Part1;

public class Day18Part1
{
    private int Run(IEnumerable<string> data)
    {
        var res = data.Select(s => s.Split(" ").ToList()).Select(p => (Vector.Parse(p[0]), int.Parse(p[1])))
            .Aggregate(
                (loop: new HashSet<Vector>(), current: Vector.Origo, perimeter: 0),
                (prev, curr) =>
                {
                    var (direction, length) = curr;
                    var (loop, current, perimeter) = prev;

                    perimeter += length;

                    loop.Add(current);
                    foreach (var _ in Enumerable.Range(0, length))
                    {
                        current = current.Add(direction);
                    }

                    loop.Add(current);

                    return (loop, current, perimeter);
                }
            );
        var (loop, _, perimeter) = res;

        return loop.Pairwise((a, b) => a.X * b.Y - a.Y * b.X).Sum() / 2 + perimeter / 2 + 1;
    }

    private class Day18Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day18Part1/testdata.txt");
            var sut = new Day18Part1();
            Assert.That(sut.Run(data), Is.EqualTo(62));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day18Part1/data.txt");
            var sut = new Day18Part1();
            Assert.That(sut.Run(data), Is.EqualTo(48652));
        }
    }
}