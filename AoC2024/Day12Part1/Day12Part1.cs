using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;
using Vector = Utils.Vector;

namespace AoC2024.Day12Part1;

public class Day12Part1
{
    private record Location(char Crop, bool Visited = false);
    private record Region(int Area = 0, int Perimeter = 0);

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
        var currentRegion = new Region();
        
        var queue = new Queue<Vector>();
        queue.Enqueue(map.First().Key);
        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();
            map[currentPosition] = map[currentPosition] with { Visited = true };
            currentRegion = currentRegion with { Area = currentRegion.Area + 1 };
            foreach (var neighbor in Vector.CardinalDirections.Select(currentPosition.Add))
            {
                if (!map.ContainsKey(neighbor) || map[neighbor].Crop != map[currentPosition].Crop)
                {
                    currentRegion = currentRegion with { Perimeter = currentRegion.Perimeter + 1 };
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

            currentRegion = new Region();
            queue.Enqueue(next.Key);
        }

        return regions.Sum(r => r.Area * r.Perimeter);
    }

    private class Day12Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day12Part1/testdata.txt");
            var sut = new Day12Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1930));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day12Part1/data.txt");
            var sut = new Day12Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(1421958));
        }
    }
}