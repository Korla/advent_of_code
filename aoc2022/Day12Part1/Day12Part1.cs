using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace aoc2022.Day12Part1;

public class Day12Part1
{
    private int Run(IList<string> data, (int x, int y) targetNode)
    {
        var maxY = data.Count;
        var maxX = data[0].Length;
        var currentNode = (0, 0);
        var map = new Dictionary<(int x, int y), (List<(int x, int y)> neighbors, int dist)>();
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                var currentValue = data[y][x];
                if (currentValue == 'S')
                {
                    currentNode = (x, y);
                    currentValue = 'a';
                }

                if (currentValue == 'E')
                {
                    targetNode = (x, y);
                    currentValue = 'z';
                }

                var valueTuples = new[] {(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)};
                var neighbors = valueTuples
                    .Where(v => v.Item1.IsBetweenInclusive(0, maxX - 1) && v.Item2.IsBetweenInclusive(0, maxY - 1) && data[v.Item2][v.Item1] - currentValue <= 1)
                    .ToList();
                map.Add((x, y), (neighbors, 1));
            }
        }

        foreach (var kvp in map)
        {
            Console.WriteLine($"([{data[kvp.Key.y][kvp.Key.x]}] {kvp.Key.x},{kvp.Key.y}) -> {string.Join(",", kvp.Value.neighbors.Select(neighbor => $"([{data[neighbor.y][neighbor.x]}] {neighbor.x},{neighbor.y})"))}");
        }
        return Pathfinding.Dijkstra(currentNode, targetNode, map);
    }

    private class Day12Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day12Part1/testdata.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(31, sut.Run(data, (5,2)));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day12Part1/data.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(449, sut.Run(data, (107,20)));
        }
    }
}