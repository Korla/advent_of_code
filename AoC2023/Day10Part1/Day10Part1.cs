using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day10Part1;

public class Day10Part1
{
    private static readonly Vector Dummy = new(-1, -1);
    private static readonly Vector North = new(0, -1);
    private static readonly Vector East = new(1, 0);
    private static readonly Vector South = new(0, 1);
    private static readonly Vector West = new(-1, 0);
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
        return GetLoop(start, grid).Count() / 2;
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
        var visited = new List<Vector> { start };
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

    private class Day10Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day10Part1/testdata.txt");
            var sut = new Day10Part1();
            Assert.That(sut.Run(data), Is.EqualTo(4));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day10Part1/data.txt");
            var sut = new Day10Part1();
            Assert.That(sut.Run(data), Is.EqualTo(6697));
        }
    }
}