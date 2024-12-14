using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utils;

namespace AoC2024.Day14Part1;

public class Day14Part1
{
    private enum Quadrants
    {
        None, TopLeft, TopRight, BottomLeft, BottomRight
    }
    private record Guard(Vector Position, Vector Direction);
    private int Run(IEnumerable<string> data)
    {
        var guards = data.Select(row =>
        {
            var group = Regex.Matches(row, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)").First().Groups;
            var position = new Vector(int.Parse(group[1].Value), int.Parse(group[2].Value));
            var direction = new Vector(int.Parse(group[3].Value), int.Parse(group[4].Value));
            return new Guard(position, direction);
        });
        
        var size = new Vector(101, 103);
        var middle = size / 2;
        const int time = 100;
        var result = guards.Select(MoveGuard);
        var groups = result.GroupBy(a =>
        {
            if (a.X < middle.X && a.Y < middle.Y) return Quadrants.TopLeft;
            if (a.X > middle.X && a.Y < middle.Y) return Quadrants.TopRight;
            if (a.X > middle.X && a.Y > middle.Y) return Quadrants.BottomRight;
            if (a.X < middle.X && a.Y > middle.Y) return Quadrants.BottomLeft;
            return Quadrants.None;
        });

        return groups.Where(g => g.Key != Quadrants.None).Select(g => g.Count()).Multiply();

        Vector MoveGuard(Guard guard)
        {
            var currentPosition = guard.Position;
            for (var i = 0; i < time; i++)
            {
                currentPosition += guard.Direction;
                currentPosition = currentPosition.Add(size).Modulo(size);
            }

            return currentPosition;
        }
    }

    private class Day14Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day14Part1/testdata.txt");
            var sut = new Day14Part1();
            Assert.That(sut.Run(data), Is.EqualTo(21));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day14Part1/data.txt");
            var sut = new Day14Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(231019008));
        }
    }
}