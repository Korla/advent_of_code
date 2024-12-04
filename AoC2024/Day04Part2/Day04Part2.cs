using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day04Part2;

public class Day04Part2
{
    private int Run(IEnumerable<string> data)
    {
        var grid = data
            .SelectMany(
                (row, y) => row.Select((content, x) => (position: new Vector(x, y), content))
            )
            .ToDictionary(p => p.position, p => p.content);
        return grid
            .Where(p => p.Value == 'A')
            .Count(p => IsMas(p.Key, grid));
    }

    private static bool IsMas(Vector position, Dictionary<Vector,char> grid)
    {
        if (
            grid.TryGetValue(position.Add(Vector.UpLeft), out var upLeft) &&
            grid.TryGetValue(position.Add(Vector.UpRight), out var upRight) &&
            grid.TryGetValue(position.Add(Vector.DownRight), out var downRight) &&
            grid.TryGetValue(position.Add(Vector.DownLeft), out var downLeft)
        )
        {
            var neighbors = new List<char> { upLeft, upRight, downRight, downLeft };
            if (neighbors.Count(n => n == 'M') == 2 && neighbors.Count(n => n == 'S') == 2)
            {
                var isMas = !(
                    (upLeft == 'M' && downRight == 'M') ||
                    (upRight == 'M' && downLeft == 'M')
                );
                Console.WriteLine($"IsMas: {isMas}");
                return isMas;
            }
        }

        return false;
    }

    private class Day04Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day04Part2/testdata.txt");
            var sut = new Day04Part2();
            Assert.That(sut.Run(data), Is.EqualTo(9));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day04Part2/data.txt");
            var sut = new Day04Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1969));
        }
    }
}