using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC2021.Day18Part2;

public class Day18Part2
{
    private interface INumber
    {
        string ToString();
        int Sum();
    }

    private class SimpleNumber : INumber
    {
        public SimpleNumber(string part)
        {
            Part = int.Parse(part);
        }

        private int Part { get; }

        public override string ToString()
        {
            return $"{Part.ToString()}";
        }

        public int Sum()
        {
            return Part;
        }
    }

    private class ComplexNumber : INumber
    {
        public ComplexNumber(string input)
        {
            var start = 1;
            var end = input.Length - 1;
            var openCount = 0;
            for(var i = start; i < end; i++)
            {
                var c = input[i];
                if (c == '[') openCount++;
                if (c == ']') openCount--;
                if (openCount == 0)
                {
                    var part1String = input.Substring(start, i);
                    Part1 = part1String.Length == 1 ? new SimpleNumber(part1String) : new ComplexNumber(part1String);
                    var part2String = input.Substring(i + 2, end - (i + 2));
                    Part2 = part2String.Length == 1 ? new SimpleNumber(part2String) : new ComplexNumber(part2String);
                    break;
                }
            }
        }

        private INumber Part1 { get; }
        private INumber Part2 { get; }

        public override string ToString()
        {
            return $"[{Part1.ToString()},{Part2.ToString()}]";
        }

        public int Sum() => 3 * Part1.Sum() + 2 * Part2.Sum();
    }

    private class Salmon
    {
        public int Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }

    private long Run(IList<string> data)
    {
        var permutations = new List<string[]>();
        foreach (var s1 in data)
        {
            foreach (var s2 in data)
            {
                permutations.Add(new []{ s1, s2});
                permutations.Add(new []{ s2, s1});
            }
        }

        return permutations.Max(v => new ComplexNumber(Sum(v)).Sum());
    }

    private static string Sum(IEnumerable<string> input) => input.Aggregate(Add);

    private static string Explode(string input)
    {
        var integers = Regex.Matches(input, @"([0-9][0-9]|[0-9])").Select(m =>
        {
            var value = int.Parse(m.Value);
            var startIndex = m.Index;
            return new Salmon {Value = value, StartIndex = startIndex, EndIndex = startIndex + value.ToString().Length - 1};
        }).ToList();
        var integersReversed = integers.OrderBy(v => -v.StartIndex).ToList();
        var openCount = 0;
        for (var start = 0; start < input.Length; start++)
        {
            var c = input[start];
            if (c == '[') openCount++;
            if (c == ']') openCount--;

            if (openCount == 5)
            {
                int end;
                for (end = start; end < input.Length; end++)
                {
                    if (input[end] == ']') break;
                }
                var indexOfLeft = start + 1;
                integers.Add(new Salmon { Value = 0, StartIndex = start, EndIndex = start});
                var left = integers.First(v => v.StartIndex == indexOfLeft);
                var right = integers.First(v => v.StartIndex > indexOfLeft);
                integers.Remove(left);
                integers.Remove(right);
                var leftTarget = integersReversed.FirstOrDefault(v => v.StartIndex < indexOfLeft);
                var rightTarget = integers.FirstOrDefault(v => v.StartIndex > right.StartIndex);
                if (leftTarget != null) leftTarget.Value += left.Value;
                if (rightTarget != null) rightTarget.Value += right.Value;

                var result = "";
                for (var j = 0; j < input.Length; j++)
                {
                    var integer = integers.FirstOrDefault(v => v.StartIndex <= j && j <= v.EndIndex);
                    if (integer != null && integer.StartIndex == j) result += integer.Value;
                    else if (integer != null && integer.StartIndex != j) result += "";
                    else if (start <= j && j <= end) result += "";
                    else result += input[j];
                }

                return result;
            }
        }

        return input;
    }

    private static string Split(string input)
    {
        var integers = Regex.Matches(input, @"([0-9][0-9]|[0-9])").Select(m =>
        {
            var value = int.Parse(m.Value);
            var index = m.Index;
            return new Salmon { Value = value, StartIndex = index };
        }).ToList();
        var target = integers.FirstOrDefault(v => v.Value > 9);
        if (target == null) return input;
        var half = target.Value / (double) 2;
        var first = input[..target.StartIndex];
        var s = input[(target.StartIndex + 2)..];
        return $"{first}[{Math.Floor(half)},{Math.Ceiling(half)}]{s}";
    }

    private static string Add(string first, string second)
    {
        var newAdded = $"[{first},{second}]";
        while (true)
        {
            var oldAdded = newAdded;
            newAdded = Explode(newAdded);
            if (oldAdded != newAdded) continue;
            newAdded = Split(newAdded);
            if (oldAdded == newAdded) return newAdded;
        }
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day18Part2/testdata.txt");
            var sut = new Day18Part2();
            Assert.AreEqual(3993, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day18Part2/data.txt");
            var sut = new Day18Part2();
            Assert.AreEqual(4650, sut.Run(data));
        }
    }
}