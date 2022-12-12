using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace aoc2022.Day12Part2;

public class Day12Part2
{
    private int Run(IList<string> data, (int x, int y) targetNode)
    {
        var maxY = data.Count;
        var maxX = data[0].Length;
        var map = new Dictionary<(int x, int y), (List<(int x, int y)> neighbors, int dist)>();
        var startNodes = new List<(int x, int y)>();
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                var currentValue = data[y][x];
                if (currentValue == 'S')
                {
                    currentValue = 'a';
                }

                if (currentValue == 'E')
                {
                    targetNode = (x, y);
                    currentValue = 'z';
                }

                if (currentValue == 'a')
                {
                    startNodes.Add((x,y));
                }

                var valueTuples = new[] {(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)};
                var neighbors = valueTuples
                    .Where(v => v.Item1.IsBetweenInclusive(0, maxX - 1) && v.Item2.IsBetweenInclusive(0, maxY - 1) && data[v.Item2][v.Item1] - currentValue <= 1)
                    .ToList();
                map.Add((x, y), (neighbors, 1));
            }
        }

        return startNodes.Min(currentNode => Pathfinding.Dijkstra(currentNode, targetNode, map));
    }

    private class Day12Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day12Part2/testdata.txt");
            var sut = new Day12Part2();
            Assert.AreEqual(29, sut.Run(data, (5,2)));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day12Part2/data.txt");
            var sut = new Day12Part2();
            Assert.AreEqual(443, sut.Run(data, (107,20)));
        }
    }
}