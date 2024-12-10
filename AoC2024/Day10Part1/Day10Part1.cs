using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day10Part1;

public class Day10Part1
{
    private int Run(IEnumerable<string> data)
    {
        var dataList = data.ToList();
        var grid = new Dictionary<Vector, int>();
        var yMax = dataList.Count;
        var xMax = dataList.First().Length;
        for (var y = 0; y < yMax; y++)
        {
            for (var x = 0; x < xMax; x++)
            {
                grid.Add(new Vector(x, y), int.Parse(dataList[y][x].ToString()));
            }
        }
        
        var graph = new Dictionary<Vector, List<Vector>>();
        foreach (var (position, value) in grid)
        {
            var neighhbors = Vector.CardinalDirections
                .Select(dir => position.Add(dir))
                .Where(neighbor =>
                    neighbor.X.IsBetweenInclusive(0, xMax - 1) &&
                    neighbor.Y.IsBetweenInclusive(0, yMax - 1) &&
                    grid[neighbor] - value == 1
                )
                .ToList();
            graph.Add(position, neighhbors);
        }

        return grid.Where(g => g.Value == 0).SelectMany(kvp => RecWalk(kvp.Key).Distinct()).Count();

        IEnumerable<Vector> RecWalk(Vector node)
        {
            return grid[node] == 9 ? [node] : graph[node].SelectMany(RecWalk);
        }
    }

    private class Day10Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day10Part1/testdata.txt");
            var sut = new Day10Part1();
            Assert.That(sut.Run(data), Is.EqualTo(36));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day10Part1/data.txt");
            var sut = new Day10Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(798));
        }
    }
}