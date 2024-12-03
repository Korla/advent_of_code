using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day16Part1;

public class Day16Part1
{
    private static Dictionary<(char, Vector), List<Vector>> bounceMap = new()
    {
        {('.', Vector.Up), new List<Vector> { Vector.Up }},
        {('.', Vector.Right), new List<Vector> { Vector.Right }},
        {('.', Vector.Down), new List<Vector> { Vector.Down }},
        {('.', Vector.Left), new List<Vector> { Vector.Left }},

        {('/', Vector.Up), new List<Vector> { Vector.Right }},
        {('/', Vector.Right), new List<Vector> { Vector.Up }},
        {('/', Vector.Down), new List<Vector> { Vector.Left }},
        {('/', Vector.Left), new List<Vector> { Vector.Down }},

        {('\\', Vector.Up), new List<Vector> { Vector.Left }},
        {('\\', Vector.Right), new List<Vector> { Vector.Down }},
        {('\\', Vector.Down), new List<Vector> { Vector.Right }},
        {('\\', Vector.Left), new List<Vector> { Vector.Up }},

        {('-', Vector.Up), new List<Vector> { Vector.Left, Vector.Right }},
        {('-', Vector.Right), new List<Vector> { Vector.Right }},
        {('-', Vector.Down), new List<Vector> { Vector.Left, Vector.Right }},
        {('-', Vector.Left), new List<Vector> { Vector.Left }},

        {('|', Vector.Up), new List<Vector> { Vector.Up }},
        {('|', Vector.Right), new List<Vector> { Vector.Up, Vector.Down }},
        {('|', Vector.Down), new List<Vector> { Vector.Down }},
        {('|', Vector.Left), new List<Vector> { Vector.Up, Vector.Down }},
    };

    private int Run(IEnumerable<string> data)
    {
        var grid = data
            .SelectMany((row, y) => row.Select((c, x) => (pos: new Vector(x, y), value: c)))
            .ToDictionary(a => a.pos, a => a.value);
        var start = (Vector.Origo, Vector.Right);
        var unvisited = new Queue<(Vector position, Vector direction)>();
        unvisited.Enqueue(start);
        var cache = new HashSet<(Vector position, Vector direction)>();

        var energized = new HashSet<Vector>();
        while (unvisited.Any())
        {
            var current = unvisited.Dequeue();
            if (!grid.ContainsKey(current.position)) continue;

            energized.Add(current.position);
            cache.Add(current);

            var bounces = bounceMap[(grid[current.position], current.direction)]
                .Select(direction => (current.position.Add(direction), direction))
                .Where(v => !cache.Contains(v));
            foreach (var vector in bounces)
            {
                unvisited.Enqueue(vector);
            }
        }

        return energized.Count;
    }

    private class Day16Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day16Part1/testdata.txt");
            var sut = new Day16Part1();
            Assert.That(sut.Run(data), Is.EqualTo(46));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day16Part1/data.txt");
            var sut = new Day16Part1();
            Assert.That(sut.Run(data), Is.EqualTo(8539));
        }
    }
}