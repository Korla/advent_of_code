using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day10Part2;

public class Day10Part2
{
    private enum Color
    {
        Filled,
        Empty,
        Loop
    }
    private static readonly Vector Dummy = new(-1, -1);
    private static readonly Vector North = new(0, -1);
    private static readonly Vector East = new(1, 0);
    private static readonly Vector South = new(0, 1);
    private static readonly Vector West = new(-1, 0);
    private static readonly List<Vector> Neighbors = new() { North, East, South, West };
    private static readonly Dictionary<char, List<Vector>> ValueMap = new()
    {
        {'|', new List<Vector>{ North, South }},
        {'-', new List<Vector>{ West, East }},
        {'L', new List<Vector>{ North, East }},
        {'J', new List<Vector>{ North, West }},
        {'7', new List<Vector>{ West, South }},
        {'F', new List<Vector>{ East, South }},
    };

    private int Run(IList<string> data)
    {
        var (start, grid) = BuildGrid(data);
        var loop = GetLoop(start, grid)
            .ToList();
        // one more point between each point
        loop.Add(loop.First());
        var newLoop = new HashSet<Vector>();
        foreach (var (a, middle, b) in loop
                     .Select(v => v.Multiply(2))
                     .Pairwise((a, b) => (a, middle: a.Add(b), b)))
        {
            newLoop.Add(a);
            newLoop.Add(new Vector(middle.X / 2, middle.Y / 2));
            newLoop.Add(b);
        }

        // flood fill from corner
        var expandedGrid = new Dictionary<Vector, Color>();
        for (var y = 0; y <= grid.Max(g => g.Key.Y); y++)
        {
            for (var x = 0; x <= grid.Max(g => g.Key.X); x++)
            {
                var vector1 = new Vector(2 * x, 2 * y);
                expandedGrid.Add(vector1, newLoop.Contains(vector1) ? Color.Loop : Color.Empty);
                var vector2 = new Vector(2 * x, 2 * y + 1);
                expandedGrid.Add(vector2, newLoop.Contains(vector2) ? Color.Loop : Color.Empty);
                var vector3 = new Vector(2 * x + 1, 2 * y);
                expandedGrid.Add(vector3, newLoop.Contains(vector3) ? Color.Loop : Color.Empty);
                var vector4 = new Vector(2 * x + 1, 2 * y + 1);
                expandedGrid.Add(vector4, newLoop.Contains(vector4) ? Color.Loop : Color.Empty);
            }
        }

        FloodFill(expandedGrid, new Vector(0, 0), Color.Empty, Color.Filled);

        // count only even points
        return expandedGrid
            .Count(e =>
                e.Value == Color.Empty &&
                e.Key.X % 2 == 0 && e.Key.Y % 2 == 0
            );
    }

    private (Vector start, Dictionary<Vector, List<Vector>> grid) BuildGrid(IList<string> data)
    {
        var start = Dummy;
        var grid = new Dictionary<Vector, List<Vector>>();
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data.First().Length; x++)
            {
                var vector = new Vector(x, y);
                var value = data[y][x];
                if (value == 'S')
                {
                    start = vector;
                }
                else if (ValueMap.ContainsKey(value))
                {
                    grid.Add(vector, ValueMap[value].Select(v => v.Add(vector)).ToList());
                }
            }
        }

        var startNeighbors = grid
            .Where(a => a.Value.Contains(start))
            .Select(a => a.Key)
            .ToList();
        grid.Add(start, startNeighbors);

        return (start, grid);
    }

    private IEnumerable<Vector> GetLoop(Vector start, IReadOnlyDictionary<Vector, List<Vector>> grid)
    {
        var visited = new HashSet<Vector> { start };
        while (true)
        {
            var possibleMoves = grid[visited.Last()].Except(visited).ToList();
            if (!possibleMoves.Any())
            {
                return visited;
            }

            visited.Add(possibleMoves.First());
        }
    }

    private static void FloodFill(IDictionary<Vector, Color> grid, Vector pt, Color targetColor, Color replacementColor)
    {
        var maxX = grid.Max(g => g.Key.X);
        var maxY = grid.Max(g => g.Key.Y);
        var pixels = new Stack<Vector>();
        pixels.Push(pt);

        while (pixels.Count > 0)
        {
            var a = pixels.Pop();
            if (a.X < maxX && a.X >= 0 && a.Y < maxY && a.Y >= 0)
            {
                if (grid[a] == targetColor)
                {
                    grid[a] = replacementColor;
                    foreach (var neighbor in Neighbors)
                    {
                        pixels.Push(a.Add(neighbor));
                    }
                }
            }
        }
    }

    private class Day10Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day10Part2/testdata.txt");
            var sut = new Day10Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day10Part2/data.txt");
            var sut = new Day10Part2();
            Assert.That(sut.Run(data), Is.EqualTo(423));
        }
    }
}