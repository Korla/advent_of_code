using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day16Part2;

public class Day16Part2
{
    private record Location(Vector Position, Vector Direction, int Weight);

    private int Run(IEnumerable<string> data)
    {
        var memoizedNeighbors = new Dictionary<Location, List<Location>>();
        var actualStart = Vector.Origo;
        var actualEnd = Vector.Origo;
        var maze = new HashSet<Vector>();
        foreach (var cell in data.SelectMany((row, y) => row.Select((c, x) => (p: new Vector(x, y), c))))
        {
            switch (cell.c)
            {
                case 'S':
                    actualStart = cell.p;
                    maze.Add(cell.p);
                    break;
                case '.':
                    maze.Add(cell.p);
                    break;
                case 'E':
                    actualEnd = cell.p;
                    maze.Add(cell.p);
                    break;
            }
        }

        var (minWeight, _) = RunDijkstra(new Location(actualStart, Vector.Right, 0), actualEnd, int.MaxValue);
        var possibleRoutes = new List<Vector>();
        foreach (var position in maze)
        {
            var (firstPartWeight, end) = RunDijkstra(new Location(actualStart, Vector.Right, 0), position, minWeight);
            if (firstPartWeight > minWeight)
            {
                continue;
            }
            var (secondPartWeight, _) = RunDijkstra(end, actualEnd, minWeight - firstPartWeight);
            if (firstPartWeight + secondPartWeight == minWeight)
            {
                possibleRoutes.Add(position);
            }
        }
        return possibleRoutes.Distinct().Count() + 1;

        (int weight, Location end) RunDijkstra(Location start, Vector end, int maxWeight)
        {
            var dijkstra = new Dijkstra<Location>
            {
                target = location => location.Position == end,
                weight = location => location.Weight,
                valid = location => maze.Contains(location.Position),
                neighbors = location =>
                {
                    var isCached = memoizedNeighbors.TryGetValue(location, out var neighbors);
                    if (!isCached)
                    {
                        neighbors =
                        [
                            location with { Direction = location.Direction.Rotate(), Weight = 1000 },
                            location with { Direction = location.Direction.RotateACW(), Weight = 1000 },
                            location with { Position = location.Position.Add(location.Direction), Weight = 1 }
                        ];
                        memoizedNeighbors.Add(location, neighbors);
                    }

                    return neighbors;
                }
            };
            return dijkstra.ComputeWithEnd(start, maxWeight);
        }
    }

    private class Day16Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day16Part2/testdata.txt");
            var sut = new Day16Part2();
            Assert.That(sut.Run(data), Is.EqualTo(64));
        }

        [Test]
        [Explicit]
        public void Data()
        {
            var data = File.ReadAllLines("Day16Part2/data.txt");
            var sut = new Day16Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(109496));
        }
    }
}