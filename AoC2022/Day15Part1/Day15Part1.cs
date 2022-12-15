using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day15Part1;

public class Day15Part1
{
    private record Sensor(Vector Position, Vector Beacon, int DistanceToBeacon, int OverlapWithRow, IEnumerable<int> XRangeOnRow);

    private int Run(IEnumerable<string> data, int yCoord)
    {
        var xOverlapping = data
            .Select(d =>
                {
                    var vectors = d
                        .Replace("x=", "").Replace("y=", "").Replace(", ", ",").Split("Sensor at ").Last()
                        .Split(": closest beacon is at ")
                        .Select(VectorExtensions.From)
                        .ToArray();
                    var position = vectors[0];
                    var beacon = vectors[1];
                    var distanceToBeacon = Math.Abs(position.X - beacon.X) + Math.Abs(position.Y - beacon.Y);
                    var distanceToRow = Math.Abs(position.Y - yCoord);
                    var overlap = distanceToBeacon - distanceToRow + 1;
                    var xRangeOnRow = overlap > 0
                        ? Enumerable.Range(position.X - (overlap - 1), (overlap - 1) * 2 + 1)
                        : Enumerable.Empty<int>();
                    return new Sensor(position, beacon, distanceToBeacon, overlap, xRangeOnRow);
                }
            )
            .ToList();
        var covered = new HashSet<int>();
        var beacons = xOverlapping.Select(x => x.Beacon).Where(b => b.Y == yCoord);
        foreach (var x in xOverlapping.SelectMany(s => s.XRangeOnRow).Where(x => !beacons.Contains(new Vector(x, yCoord))))
        {
            covered.Add(x);
        }
        return covered.Count;
    }

    private class Day15Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day15Part1/testdata.txt");
            var sut = new Day15Part1();
            Assert.AreEqual(26, sut.Run(data, 10));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day15Part1/data.txt");
            var sut = new Day15Part1();
            Assert.AreEqual(4883971, sut.Run(data, 2000000));
        }
    }
}