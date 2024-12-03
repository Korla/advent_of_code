using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day15Part2;

public class Day15Part2
{
    private record Sensor(Vector Position, Vector Beacon, int DistanceToBeacon);

    private long Run(IReadOnlyList<string> data, int maxCoord)
    {
        var pointsToCheck = new ConcurrentBag<Vector>();
        var sensors = new ConcurrentBag<Sensor>();

        Parallel.For(0, data.Count, i =>
        {
            var vectors = data[i]
                .Replace("x=", "").Replace("y=", "").Replace(", ", ",").Split("Sensor at ").Last()
                .Split(": closest beacon is at ")
                .Select(VectorExtensions.From)
                .ToArray();
            var position = vectors[0];
            var beacon = vectors[1];
            var distanceToBeacon = position.ManhattanDistance(beacon);
            foreach (var distanceToRow in Enumerable.Range(0, distanceToBeacon))
            {
                var xDistance = distanceToBeacon - distanceToRow;
                var points = new[]
                {
                    new Vector(position.X + xDistance + 1, position.Y + distanceToRow),
                    new Vector(position.X + xDistance + 1, position.Y - distanceToRow),
                    new Vector(position.X - xDistance - 1, position.Y + distanceToRow),
                    new Vector(position.X - xDistance - 1, position.Y - distanceToRow),
                };
                foreach (var vector in points.Where(p => p.X.IsBetweenInclusive(0, maxCoord) && p.Y.IsBetweenInclusive(0, maxCoord)))
                {
                    pointsToCheck.Add(vector);
                }
            }

            sensors.Add(new Sensor(position, beacon, distanceToBeacon));
        });

        var point = pointsToCheck.First(p => !sensors.Any(s => s.Position.ManhattanDistance(p) <= s.DistanceToBeacon));
        return (long)point.X * 4000000 + point.Y;
    }

    private class Day15Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day15Part2/testdata.txt");
            var sut = new Day15Part2();
            Assert.That(sut.Run(data, 20), Is.EqualTo(56000011));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day15Part2/data.txt");
            var sut = new Day15Part2();
            Assert.That(sut.Run(data, 4000000), Is.EqualTo(12691026767556));
        }
    }
}