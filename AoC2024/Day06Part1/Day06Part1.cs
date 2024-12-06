using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day06Part1;

public class Day06Part1
{
    private int Run(IEnumerable<string> data)
    {
        var currentPosition = Vector.Origo;
        var currentDirection = Vector.Up;
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
                    break;
                case '^':
                    grid.Add(new Vector(x, y), false);
                    currentPosition = new Vector(x, y);
                    break;
            }
        }
        
        var visited = new HashSet<Vector>();
        while (grid.ContainsKey(currentPosition))
        {
            visited.Add(currentPosition);
            var nextPosition = currentPosition.Add(currentDirection);
            if (grid.TryGetValue(nextPosition, out var nextPositionIsObstacle) && nextPositionIsObstacle)
            {
                currentDirection = Rotate(currentDirection);
                nextPosition = currentPosition.Add(currentDirection);
            }
            currentPosition = nextPosition;
        }

        return visited.Count;
    }

    private static Vector Rotate(Vector vector)
    {
        if (vector == Vector.Up) return Vector.Right;
        if (vector == Vector.Right) return Vector.Down;
        if (vector == Vector.Down) return Vector.Left;
        if (vector == Vector.Left) return Vector.Up;
        throw new Exception("Invalid vector");
    }

    private class Day06Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day06Part1/testdata.txt");
            var sut = new Day06Part1();
            Assert.That(sut.Run(data), Is.EqualTo(41));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day06Part1/data.txt");
            var sut = new Day06Part1();
            Assert.That(sut.Run(data), Is.EqualTo(5208));
        }
    }
}