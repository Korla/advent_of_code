using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2021.Day15Part1;

public class Day15Part1
{
    private int Run(IList<string> data)
    {
        var maxY = data.Count;
        var maxX = data[0].Length;
        var map = new Dictionary<(int x, int y), (List<(int x, int y)> neighbors, int dist)>();
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                var dist = data[y][x] - '0';
                var neighbors = new[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) }.ToList();
                map.Add((x, y), (neighbors, dist));
            }
        }

        var currentNode = (0, 0);
        var targetNode = (maxX - 1, maxY - 1);
        return Pathfinding.Dijkstra(currentNode, targetNode, map);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day15Part1/testdata.txt");
            var sut = new Day15Part1();
            Assert.That(sut.Run(data), Is.EqualTo(40));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day15Part1/data.txt");
            var sut = new Day15Part1();
            Assert.That(sut.Run(data), Is.EqualTo(562));
        }
    }
}