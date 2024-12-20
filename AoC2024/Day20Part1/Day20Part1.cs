using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day20Part1;

public class Day20Part1
{
    private int Run(string[] data)
    {
        var maxCheatLength = 2;
        var start = Vector.Origo;
        var pathElements = new HashSet<Vector>();
        var walls = new HashSet<Vector>();
        for (var y = 0; y < data.Length; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                var vector = new Vector(x, y);
                if (data[y][x] == '#')
                {
                    walls.Add(vector);
                    continue;
                };
                if(data[y][x] == 'S') start = vector;
                pathElements.Add(vector);
            }
        }
        
        var totalWalk = pathElements.Count;
        var path = new Dictionary<Vector, int>();
        var last = Vector.Origo;
        var current = start;
        var walked = 0;
        while (current != default)
        {
            path.Add(current, totalWalk - walked);
            var next = Vector.CardinalDirections.Select(current.Add)
                .SingleOrDefault(p => pathElements.Contains(p) && p != last);
            last = current;
            current = next;
            walked++;
        }

        return path.Keys.SelectMany(RunMaze).Count();

        IEnumerable<int> RunMaze(Vector start)
        {
            var valueTuples = Cheat(start).ToList();
            return valueTuples
                .Select(p => path[start] - path[p.Position] - p.Position.ManhattanDistance(start))
                .Where(p => p >= 100);
        }
        
        IEnumerable<(Vector Position, int Distance)> Cheat(Vector start)
        {
            HashSet<Vector> visited = [];
            var queue = new Queue<(Vector Position, int Distance)>();
            queue.Enqueue((start, 0));
            while (queue.TryDequeue(out var queueElement))
            {
                if (!visited.Add(queueElement.Position))
                    continue;

                if (pathElements.Contains(queueElement.Position) && queueElement.Distance > 0)
                    yield return queueElement;

                if (queueElement.Distance >= maxCheatLength)
                    continue;

                foreach (var direction in Vector.CardinalDirections)
                {
                    queue.Enqueue((queueElement.Position + direction, queueElement.Distance + 1));
                }
            }
        }
    }


    private class Day20Part1Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day20Part1().Run(File.ReadAllLines("Day20Part1/testdata.txt")), Is.EqualTo(0));

        [Test] public void Data() =>
            Assert.That(new Day20Part1().Run(File.ReadAllLines("Day20Part1/data.txt")), Is.EqualTo(1426));
    }
}