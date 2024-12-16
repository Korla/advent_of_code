using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day16Part1;

public class Day16Part1
{
    private record Location(Vector Position, Vector Direction, int Weight);

    private int Run(IEnumerable<string> data)
    {
        var start = Vector.Origo;
        var end = Vector.Origo;
        var maze = new HashSet<Vector>();
        foreach (var cell in data.SelectMany((row, y) => row.Select((c, x) => (p: new Vector(x, y), c))))
        {
            switch (cell.c)
            {
                case 'S':
                    start = cell.p;
                    maze.Add(cell.p);
                    break;
                case '.':
                    maze.Add(cell.p);
                    break;
                case 'E':
                    end = cell.p;
                    maze.Add(cell.p);
                    break;
            }
        }

        var dijkstra = new Dijkstra<Location>
        {
            target = location => location.Position == end,
            weight = location => location.Weight,
            valid = location => maze.Contains(location.Position),
            neighbors = location =>
            [
                location with { Direction = location.Direction.Rotate(), Weight = 1000 },
                location with { Direction = location.Direction.RotateACW(), Weight = 1000 },
                location with { Position = location.Position + location.Direction, Weight = 1 }
            ]
        };
        return dijkstra.Compute(new Location(start, Vector.Right, 0));
    }

    private class Day16Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day16Part1/testdata.txt");
            var sut = new Day16Part1();
            Assert.That(sut.Run(data), Is.EqualTo(11048));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day16Part1/data.txt");
            var sut = new Day16Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(109496));
        }
    }
}