using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Utils;
using static System.Text.Json.JsonSerializer;

namespace AoC2022.Day13Part1;

public class Day13Part1
{
    private int Run(IReadOnlyList<string> data)
    {
        var index = 1;
        var count = 0;
        for (var i = 0; i < data.Count; i += 3)
        {
            // Console.WriteLine($"Comparing {data[i]}");
            // Console.WriteLine($"     with {data[i + 1]}");
            if (Compare(data[i], data[i + 1]) >= 0)
            {
                count += index;
            }
            // Console.WriteLine();
            index++;
        }

        return count;
    }

    private static int Compare(string left, string right)
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

    private class Day13Part1Tests
    {
        [TestCase("1", "1", 0)]
        [TestCase("[1]", "1", 0)]
        [TestCase("1", "2", 1)]
        [TestCase("2", "1", -1)]
        [TestCase("1,2", "1", -1)]
        [TestCase("5,6,7", "5,6,0", -1)]
        [TestCase("5,6,7", "5,6,8", 1)]
        [TestCase("7,7,7,7", "7,7,7", -1)]
        [TestCase("7,7,7,7", "7,7,7,7", 0)]
        [TestCase("9", "8,7,6", -1)]
        [TestCase("7", "8,7,6", 1)]
        [TestCase("1,1,3,1,1", "1,1,5,1,1", 1)]
        [TestCase("[1,1,3,1,1]", "[1,1,5,1,1]", 1)]
        [TestCase("[[1],[2,3,4]]", "[[1],4]", 1)]
        [TestCase("[9]", "[[8,7,6]]", -1)]
        [TestCase("[[4,4],4,4]", "[[4,4],4,4,4]", 1)]
        [TestCase("[7,7,7,7]", "[7,7,7]", -1)]
        [TestCase("[7,7,10]", "[7,7,7]", -1)]
        [TestCase("[7,7,7]", "[7,7,10]", 1)]
        [TestCase("10", "9", -1)]
        [TestCase("[]", "[3]", 1)]
        [TestCase("[[[]]]", "[[]]", -1)]
        [TestCase("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", -1)]
        [TestCase("[]", "[]", 0)]
        public void TestCompare(string left, string right, int expected)
        {
            var compare = Compare(left, right);
            Assert.That(compare, Is.EqualTo(expected));
        }

        [TestCase("[1]", new object[] { "1" })]
        [TestCase("[1,2]", new object[] { "1", "2" })]
        [TestCase("[1,[2]]", new object[] { "1", "[2]" })]
        [TestCase("[1,[2],3]", new object[] { "1", "[2]", "3" })]
        [TestCase("[[1],[2,3,4]]", new object[] { "[1]", "[2,3,4]" })]
        [TestCase("[]", new object[] { "" })]
        [TestCase("5,6", new object[] { "5", "6" })]
        [TestCase("5,10", new object[] { "5", "10" })]
        [TestCase("[[7,[[4,7],7,[2,8,2]],5,7,[[5,0,7,8,0]]],[[5]],[[0,[3,9]],8,[1,4,2],[[10,1,3,5,0],[4],[],3]],[[[6,4],[4,9,9,4]],[],[]]]", new object[] { "[7,[[4,7],7,[2,8,2]],5,7,[[5,0,7,8,0]]]", "[[5]]", "[[0,[3,9]],8,[1,4,2],[[10,1,3,5,0],[4],[],3]]", "[[[6,4],[4,9,9,4]],[],[]]" })]
        public void TestGetParts(string input, object[] expected)
        {
            var serialize = Serialize(expected);
            var strings = GetParts(input);
            var actual = Serialize(strings);
            Assert.That(
                actual,
                Is.EqualTo(serialize)
            );
        }

        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day13Part1/testdata.txt");
            var sut = new Day13Part1();
            Assert.That(sut.Run(data), Is.EqualTo(13));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day13Part1/data.txt");
            var sut = new Day13Part1();
            Assert.That(sut.Run(data), Is.EqualTo(4809));
        }
    }
}