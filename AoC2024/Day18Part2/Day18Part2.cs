using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day18Part2;

public class Day18Part2
{
    private string Run(IEnumerable<string> data, int gridSize)
    {
        var attemptedMemories = new Dictionary<int, List<Vector>> { { 0, [] } };
        var pathResults = new Dictionary<int, int>();
        var i = data.Count() / 2;
        while (true)
        {
            var pathLength = GetPathLength(i);
            pathResults.Add(i, pathLength);
            if (pathLength == int.MaxValue)
            {
                if (pathResults.ContainsKey(i - 1) && pathResults[i - 1] != int.MaxValue)
                {
                    return data.ToList()[i - 1];
                }

                i += (GetKeyBefore(i) - i) / 2;
            }
            else
            {
                if (pathResults.ContainsKey(i + 1) && pathResults[i + 1] == int.MaxValue)
                {
                    return data.ToList()[i];
                }

                i += (GetKeyAfter(i) - i) / 2;
            }
        }

        int GetPathLength(int count)
        {
            var corruptedMemories = GetCorruptedMemoriesAfter(count);
            var start = Vector.Origo;
            var end = new Vector(gridSize, gridSize);
            var dijkstra = new Dijkstra<Vector>
            {
                target = location => location == end,
                weight = _ => 1,
                valid = location => location.X >= 0 && location.Y >= 0 &&
                                    location.X <= gridSize && location.Y <= gridSize &&
                                    !corruptedMemories.Contains(location),
                neighbors = location => Vector.CardinalDirections.Select(location.Add).ToList()
            };
            return dijkstra.Compute(start);
        }

        List<Vector> GetCorruptedMemoriesAfter(int count)
        {
            var cachedCorruptedMemoriesKey = GetKeyBefore(count);
            var corruptedMemories = attemptedMemories[cachedCorruptedMemoriesKey].Select(x => x).ToList();
            foreach (var parts in data.Skip(cachedCorruptedMemoriesKey).Take(Math.Abs(count - cachedCorruptedMemoriesKey)).Select(r => r.Split(",").Select(int.Parse).ToList()))
            {
                corruptedMemories.Add(new Vector(parts.First(), parts.Last()));
            }
            attemptedMemories.Add(count, corruptedMemories);
            return corruptedMemories;
        }

        int GetKeyBefore(int key)
        {
            var lesserKeys = attemptedMemories.Keys.Order().Where(k => k < key).ToList();
            return lesserKeys.Count != 0 ? lesserKeys.Last() : 0;
        }

        int GetKeyAfter(int key)
        {
            var greaterKeys = attemptedMemories.Keys.Order().Where(k => k > key).ToList();
            return greaterKeys.Count != 0 ? greaterKeys.First() : data.Count();
        }
    }

    private class Day18Part2Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day18Part2().Run(File.ReadAllLines("Day18Part2/testdata.txt"), 6), Is.EqualTo("6,1"));

        [Test] public void Data() =>
            Assert.That(new Day18Part2().Run(File.ReadAllLines("Day18Part2/data.txt"), 70), Is.EqualTo("51,40"));
    }
}