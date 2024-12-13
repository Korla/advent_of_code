using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Vector = Utils.LongVector;

namespace AoC2024.Day13Part2;

public class Day13Part2
{
    private double Run(IEnumerable<string> data)
    {
        double sum = 0;
        Vector ButtonA = default;
        Vector ButtonB = default;
        foreach (var row in data)
        {
            var buttonAMatch = Regex.Matches(row, @"Button A: X\+(\d+), Y\+(\d+)");
            if (buttonAMatch.Any())
            {
                ButtonA = new Vector(long.Parse(buttonAMatch.First().Groups[1].Value), long.Parse(buttonAMatch.First().Groups[2].Value));
            }
            var buttonBMatch = Regex.Matches(row, @"Button B: X\+(\d+), Y\+(\d+)");
            if (buttonBMatch.Any())
            {
                ButtonB = new Vector(long.Parse(buttonBMatch.First().Groups[1].Value), long.Parse(buttonBMatch.First().Groups[2].Value));
            }
            
            var prizeMatch = Regex.Matches(row, @"Prize: X=(\d+), Y=(\d+)");
            if (prizeMatch.Count != 0)
            {
                var target = new Vector(
                    10000000000000 + long.Parse(prizeMatch.First().Groups[1].Value),
                    10000000000000 + long.Parse(prizeMatch.First().Groups[2].Value)
                );
                sum += GetCost(target);
            }
        }

        return sum;

        double GetCost(Vector target)
        {
            var det = ButtonA.X * ButtonB.Y - ButtonA.Y * ButtonB.X;
            var a = (target.X * ButtonB.Y - target.Y * ButtonB.X) / det;
            var b = (ButtonA.X * target.Y - ButtonA.Y * target.X) / det;
            var longVector = new Vector(ButtonA.X * a + ButtonB.X * b, ButtonA.Y * a + ButtonB.Y * b);
            if (longVector == target)
            {
                return a * 3 + b;
            }

            return 0;
        }
    }

    private class Day13Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day13Part2/testdata.txt");
            var sut = new Day13Part2();
            Assert.That(sut.Run(data), Is.EqualTo(875318608908));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day13Part2/data.txt");
            var sut = new Day13Part2();
            var actual = sut.Run(data);
            Console.WriteLine(actual.ToString("F"));
            Assert.That(actual, Is.EqualTo(108528956728655));
        }
    }
}