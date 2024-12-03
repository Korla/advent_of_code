using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day18Part2;

public class Day18Part2
{
    private long Run(IEnumerable<string> data)
    {
        var res = data.Select(s => s.Split(" ").Last().Skip(2).ToList())
            .Aggregate(
                (loop: new HashSet<Vector>(), current: Vector.Origo, perimeter: 0L),
                (prev, curr) =>
                {
                    var enumerable = curr.Take(5);
                    var length = long.Parse(string.Join("", enumerable), System.Globalization.NumberStyles.HexNumber);
                    var direction = Vector.CardinalDirections[(curr[5] + 1) % 4];
                    var (loop, current, perimeter) = prev;

                    perimeter += length;

                    loop.Add(current);
                    for (var i = 0L; i < length; i++)
                    {
                        current = current.Add(direction);
                    }

                    loop.Add(current);

                    return (loop, current, perimeter);
                }
            );
        var (loop, _, perimeter) = res;

        return loop.Pairwise((a, b) => (long)a.X * b.Y - (long)a.Y * b.X).Sum() / 2 + perimeter / 2 + 1;
    }

    private class Day18Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day18Part2/testdata.txt");
            var sut = new Day18Part2();
            Assert.That(sut.Run(data), Is.EqualTo(952408144115));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day18Part2/data.txt");
            var sut = new Day18Part2();
            Assert.That(sut.Run(data), Is.EqualTo(45757884535661));
        }
    }
}