using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day04Part1;

public class Day04Part1
{
    private int Run(IEnumerable<string> data)
    {
        var grid = data
            .SelectMany(
                (row, y) => row.Select((content, x) => (position: new Vector(x, y), content))
            )
            .ToDictionary(p => p.position, p => p.content);
        return grid
            .Where(p => p.Value == 'X')
            .Sum(p =>
                Vector.AllNeighbors.Count(direction => FindXmas(p.Key, direction, grid, 'X'))
            );
    }

    private static bool FindXmas(Vector position, Vector direction, Dictionary<Vector, char> grid, char currenCharacter)
    {
        const string xmas = "XMAS";
        if (currenCharacter == xmas.Last())
        {
            return true;
        }
        var nextPosition = position.Add(direction);
        var nextCharacter = xmas[xmas.IndexOf(currenCharacter) + 1];
        if (!grid.TryGetValue(nextPosition, out var value) || value != nextCharacter)
        {
            return false;
        }

        return FindXmas(nextPosition, direction, grid, nextCharacter);
    }

    private class Day04Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day04Part1/testdata.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(18));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day04Part1/data.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(2507));
        }
    }
}