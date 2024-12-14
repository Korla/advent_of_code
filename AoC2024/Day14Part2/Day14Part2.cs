using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utils;

namespace AoC2024.Day14Part2;

public class Day14Part2
{
    private enum Quadrants
    {
        None, TopLeft, TopRight, BottomLeft, BottomRight
    }

    private class Guard(Vector Position, Vector Direction)
    {
        public Vector Position { get; set; } = Position;
        public Vector Direction { get; } = Direction;
    }
    private int Run(IEnumerable<string> data)
    {
        var guards = data.Select(row =>
        {
            var group = Regex.Matches(row, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)").First().Groups;
            var position = new Vector(int.Parse(group[1].Value), int.Parse(group[2].Value));
            var direction = new Vector(int.Parse(group[3].Value), int.Parse(group[4].Value));
            return new Guard(position, direction);
        })
        .ToList();
        
        var size = new Vector(101, 103);
        const int time = 8280;
        for (var i = 0; i < time; i++)
        {
            foreach (var guard in guards)
            {
                guard.Position = (guard.Position + guard.Direction + size) % size;
            }
        }
        // Log(time);

        return time;

        void Log(int t)
        {
            Console.WriteLine($"time {t}");
            var g = guards.Select(a => a.Position).ToHashSet();
            for (var y = 0; y < size.Y; y++)
            {
                for (var x = 0; x < size.X; x++)
                {
                    Console.Write(g.Contains(new Vector(x, y)) ? 'X' : '.');
                }
                Console.WriteLine();
            }
        }
    }

    private class Day14Part2Tests
    {
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day14Part2/data.txt");
            var sut = new Day14Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(8280));
        }
    }
}