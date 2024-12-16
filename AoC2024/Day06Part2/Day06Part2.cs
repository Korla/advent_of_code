using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day06Part2;

public class Day06Part2
{
    private int Run(IEnumerable<string> data)
    {
        var currentPosition = Vector.Origo;
        var currentDirection = Vector.Up;
        var possibleObstructions = new HashSet<Vector>();
        var grid = new Dictionary<Vector, bool>();
        foreach (var (content, x, y) in data.SelectMany((row, y) => row.Select((content, x) => (content, x, y))))
        {
            switch (content)
            {
                case '#':
                    grid.Add(new Vector(x, y), true);
                    break;
                case '.':
                    grid.Add(new Vector(x, y), false);
                    possibleObstructions.Add(new Vector(x, y));
                    break;
                case '^':
                    grid.Add(new Vector(x, y), false);
                    currentPosition = new Vector(x, y);
                    break;
            }
        }

        return possibleObstructions
            .Count(obstruction => WalkGridWithObstruction(grid, currentPosition, currentDirection, obstruction));
    }

    private static bool WalkGridWithObstruction(Dictionary<Vector, bool> grid, Vector currentPosition, Vector currentDirection, Vector obstruction)
    {
        var visited = new HashSet<(Vector, Vector)>();
        while (grid.ContainsKey(currentPosition))
        {
            if (visited.Contains((currentPosition, currentDirection)))
            {
                return true;
            }
            visited.Add((currentPosition, currentDirection));
            var nextPosition = currentPosition.Add(currentDirection);
            if (nextPosition == obstruction || grid.TryGetValue(nextPosition, out var nextPositionIsObstacle) && nextPositionIsObstacle)
            {
                currentDirection = currentDirection.Rotate();
            }
            else
            {
                currentPosition = nextPosition;
            }
        }

        return false;
    }

    private class Day06Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day06Part2/testdata.txt");
            var sut = new Day06Part2();
            Assert.That(sut.Run(data), Is.EqualTo(6));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day06Part2/data.txt");
            var sut = new Day06Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1972));
        }
    }
}