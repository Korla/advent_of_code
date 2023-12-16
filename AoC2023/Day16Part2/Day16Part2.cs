using NUnit.Framework;
using Utils;

namespace AoC2023.Day16Part2;

public class Day16Part2
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
        var starts = new List<(Vector position, Vector direction)>();
        for (var i = 0; i < data.Count(); i++)
        {
            starts.Add((new Vector(0, i), Vector.Right));
            starts.Add((new Vector(data.Count(), i), Vector.Left));
            starts.Add((new Vector(i, 0), Vector.Down));
            starts.Add((new Vector(i, data.Count()), Vector.Up));
        }
        return starts.Max(start => GetEnergizedCount(start, grid));
    }

    private static int GetEnergizedCount((Vector position, Vector direction) start, Dictionary<Vector, char> grid)
    {
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

    private class Day16Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day16Part2/testdata.txt");
            var sut = new Day16Part2();
            Assert.AreEqual(51, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day16Part2/data.txt");
            var sut = new Day16Part2();
            Assert.AreEqual(8539, sut.Run(data));
        }
    }
}