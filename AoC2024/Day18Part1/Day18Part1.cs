using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day18Part1;

public class Day18Part1
{
    private int Run(IEnumerable<string> data, int numberOfLines, int gridSize)
    {
        var corruptedMemories = new HashSet<Vector>();
        foreach (var parts in data.Take(numberOfLines).Select(r => r.Split(",").Select(int.Parse).ToList()))
        {
            corruptedMemories.Add(new Vector(parts.First(), parts.Last()));
        }

        var start = Vector.Origo;
        var end = new Vector(gridSize, gridSize);
        corruptedMemories.Print();
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

    private class Day18Part1Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day18Part1().Run(File.ReadAllLines("Day18Part1/testdata.txt"), 12, 6), Is.EqualTo(22));

        [Test] public void Data() =>
            Assert.That(new Day18Part1().Run(File.ReadAllLines("Day18Part1/data.txt"), 1024, 70), Is.EqualTo(270));
    }
}