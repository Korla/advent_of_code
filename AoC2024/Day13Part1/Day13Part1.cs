using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2024.Day13Part1;

public class Day13Part1
{
    private record Point(int X, int Y);
    private int Run(IEnumerable<string> data)
    {
        var sum = 0;
        var ButtonACost = 3;
        var ButtonBCost = 1;
        Point ButtonA = default;
        Point ButtonB = default;
        foreach (var row in data)
        {
            var buttonAMatch = Regex.Matches(row, @"Button A: X\+(\d+), Y\+(\d+)");
            if (buttonAMatch.Any())
            {
                ButtonA = new Point(int.Parse(buttonAMatch.First().Groups[1].Value), int.Parse(buttonAMatch.First().Groups[2].Value));
            }
            var buttonBMatch = Regex.Matches(row, @"Button B: X\+(\d+), Y\+(\d+)");
            if (buttonBMatch.Any())
            {
                ButtonB = new Point(int.Parse(buttonBMatch.First().Groups[1].Value), int.Parse(buttonBMatch.First().Groups[2].Value));
            }
            
            var prizeMatch = Regex.Matches(row, @"Prize: X=(\d+), Y=(\d+)");
            if (prizeMatch.Any())
            {
                var target = new Point(
                    int.Parse(prizeMatch.First().Groups[1].Value),
                    int.Parse(prizeMatch.First().Groups[2].Value)
                );
                sum += GetCost(target);
            }
        }

        return sum;

        int GetCost(Point target)
        {
            var lowestCost = int.MaxValue;
            for (var a = 1; a <= 100; a++)
            {
                for (var b = 1; b <= 100; b++)
                {
                    if (target == new Point(ButtonA.X * a + ButtonB.X * b, ButtonA.Y * a + ButtonB.Y * b))
                    {
                        lowestCost = Math.Min(lowestCost, a * ButtonACost + b * ButtonBCost);
                    }
                }
            }

            return lowestCost == int.MaxValue ? 0 : lowestCost;
        }
    }

    private class Day13Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day13Part1/testdata.txt");
            var sut = new Day13Part1();
            Assert.That(sut.Run(data), Is.EqualTo(480));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day13Part1/data.txt");
            var sut = new Day13Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(25751));
        }
    }
}