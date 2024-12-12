using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils;
using Vector = Utils.Vector;

namespace AoC2024.Day12Part2;

public class Day12Part2
{
    private record Location(char Crop, bool Visited = false);

    private record Region(List<(Vector Position, Vector Direction, bool Visited)> Edges, int Area = 0)
    {
        public override string ToString()
        {
            return $"area: {Area}, edges: {string.Join(",", Edges)}";
        }
    }

    private int Run(IEnumerable<string> data)
    {
        var map = new Dictionary<Vector, Location>();
        var dataList = data.ToList();
        var yMax = dataList.Count;
        var xMax = dataList.First().Length;
        for (var y = 0; y < yMax; y++)
        {
            for (var x = 0; x < xMax; x++)
            {
                map.Add(new Vector(x, y), new Location(dataList[y][x]));
            }
        }

        var regions = new List<Region>();
        var currentRegion = new Region([]);
        
        var queue = new Queue<Vector>();
        queue.Enqueue(map.First().Key);
        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();
            map[currentPosition] = map[currentPosition] with { Visited = true };
            currentRegion = currentRegion with { Area = currentRegion.Area + 1 };
            foreach (var direction in Vector.CardinalDirections)
            {
                var neighbor = currentPosition.Add(direction);
                if (!map.ContainsKey(neighbor) || map[neighbor].Crop != map[currentPosition].Crop)
                {
                    currentRegion.Edges.Add((currentPosition, direction, false));
                }
                else if (!queue.Contains(neighbor) && !map[neighbor].Visited)
                {
                    queue.Enqueue(neighbor);
                }
            }

            if (queue.Count != 0) continue;

            regions.Add(currentRegion);

            var next = map.FirstOrDefault(p => !p.Value.Visited);
            if (next.Value == default) continue;

            currentRegion = new Region([]);
            queue.Enqueue(next.Key);
        }

        return regions.Sum(r => r.Area * GetEdgeCount(r));

        int GetEdgeCount(Region region)
        {
            return SumDirection([Vector.Right, Vector.Left], p => p.X, p => p.Y) + 
                   SumDirection([Vector.Up, Vector.Down], p => p.Y, p => p.X);

            int SumDirection(Vector[] edgeDirections, Func<Vector, int> groupByCoord, Func<Vector, int> orderByCoord)
            {
                return region.Edges
                    .Where(edge => edgeDirections.Contains(edge.Direction))
                    .GroupBy(edge => (groupByCoord(edge.Position), edge.Direction), edge => edge.Position)
                    .Sum(group => 1 + group.Select(orderByCoord).OrderBy(a => a).Pairwise((first, second) => Math.Abs(first - second)).Count(d => d != 1));
            }
        }
    }

    private class Day12Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day12Part2/testdata.txt");
            var sut = new Day12Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1206));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day12Part2/data.txt");
            var sut = new Day12Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(885394));
        }
    }
}