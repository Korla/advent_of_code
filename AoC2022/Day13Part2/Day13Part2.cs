using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day13Part2;

public class Day13Part2 : IComparer<string>
{
    private int Run(IReadOnlyList<string> data)
    {
        const string d1 = "[[2]]";
        const string d2 = "[[6]]";
        var ordered = data.Concat(new[] { d1, d2 }).Where(d => !string.IsNullOrEmpty(d)).OrderDescending(new Day13Part2())
            .ToList();

        return (ordered.IndexOf(d1) + 1) * (ordered.IndexOf(d2) + 1);
    }

    public int Compare(string left, string right)
    {
        if (string.IsNullOrEmpty(left) && string.IsNullOrEmpty(right)) return Return(0, left, right);
        if (string.IsNullOrEmpty(left)) return Return(1, left, right);
        if (string.IsNullOrEmpty(right)) return Return(-1, left, right);
        var leftIsInteger = !left.Contains('[') && !left.Contains(',');
        var rightIsInteger = !right.Contains('[') && !right.Contains(',');
        if (leftIsInteger && rightIsInteger)
        {
            return Return((int.Parse(right) - int.Parse(left)).Limit(-1, 1), left, right);
        }

        if (!leftIsInteger && !rightIsInteger)
        {
            var leftParts = GetParts(left);
            var rightParts = GetParts(right);
            for (var i = 0; i < Math.Max(leftParts.Length, rightParts.Length); i++)
            {
                if (i == leftParts.Length) return Return(1, left, right);
                if (i == rightParts.Length) return Return(-1, left, right);
                var result = Compare(leftParts[i], rightParts[i]);
                if (result < 0) return Return(-1, left, right);
                if (result > 0) return Return(1, left, right);
            }

            return 0;
        }

        if (!leftIsInteger && rightIsInteger)
        {
            return Compare(left, $"[{right}]");
        }

        if (leftIsInteger && !rightIsInteger)
        {
            return Compare($"[{left}]", right);
        }

        return 1;
    }

    private static int Return(int value, string left, string right)
    {
        Console.WriteLine($"Returning {value} for {left} - {right}");
        return value;
    }

    private static string[] GetParts(string input)
    {
        var delta = input.StartsWith('[') ? 1 : 0;
        var output = new List<string>();
        var numberOfOpenBrackets = 0;
        var currentPart = "";
        for (var i = delta; i < input.Length - delta; i++)
        {
            var currentSymbol = input[i];
            switch (currentSymbol)
            {
                case ',':
                    if (numberOfOpenBrackets == 0)
                    {
                        output.Add(currentPart);
                        currentPart = "";
                    }
                    else
                    {
                        currentPart += currentSymbol;
                    }
                    break;
                case '[':
                    numberOfOpenBrackets++;
                    currentPart += currentSymbol;
                    break;
                case ']':
                    numberOfOpenBrackets--;
                    currentPart += currentSymbol;
                    break;
                default:
                    currentPart += currentSymbol;
                    break;
            }
        }
        output.Add(currentPart);

        return output.ToArray();
    }

    private class Day13Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day13Part2/testdata.txt");
            var sut = new Day13Part2();
            Assert.That(sut.Run(data), Is.EqualTo(140));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day13Part2/data.txt");
            var sut = new Day13Part2();
            Assert.That(sut.Run(data), Is.EqualTo(22600));
        }
    }
}