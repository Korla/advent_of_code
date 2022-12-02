using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace aoc2021.Day18Part1;

public class Day18Part1
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

    private long Run(IEnumerable<string> data)
    {
        return new ComplexNumber(Sum(data)).Sum();
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
        [TestCase("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]","[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]")]
        [TestCase("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]","[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]")]
        [TestCase("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]", "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]","[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]")]
        [TestCase("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]", "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]","[7,[5,[[3,8],[1,4]]]]")]
        [TestCase("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]", "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]","[[2,[2,2]],[8,[8,1]]]")]
        [TestCase("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]", "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]","[2,9]")]
        [TestCase("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]", "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]","[1,[[[9,3],9],[[9,0],[0,7]]]]")]
        [TestCase("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]", "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]","[[[5,[7,4]],7],1]")]
        [TestCase("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]","[[[[4,2],2],6],[8,7]]")]
        public void ReduceTests(string expected, string first, string second)
        {
            Assert.AreEqual(expected, Add(first, second));
        }

        [Theory]
        [TestCase("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
        [TestCase("[1,[[[[7,8],1],2],3],4]", "[8,[[[0,9],2],3],4]")]
        [TestCase("[1,[[[[15,8],1],2],3],4]", "[16,[[[0,9],2],3],4]")]
        [TestCase("[[[[[9,8],[9,8]],2],3],4]", "[[[[0,[17,8]],2],3],4]")]
        [TestCase("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
        [TestCase("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
        [TestCase("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
        [TestCase("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
        [TestCase("[[[[0,7],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[15,[0,13]]],[1,1]]")]
        [TestCase("[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
        [TestCase("[[[[4,0],[5,4]],[[7,0],[15,5]]],[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]]", "[[[[4,0],[5,4]],[[7,0],[15,5]]],[10,[[0,[11,3]],[[6,3],[8,8]]]]]")]
        public void ExplodeTests(string input, string expected)
        {
            Assert.AreEqual(expected, Explode(input));
        }
        
        [Theory]
        [TestCase("[11,1]", "[[5,6],1]")]
        [TestCase("[11,11]", "[[5,6],11]")]
        [TestCase("[1,12]", "[1,[6,6]]")]
        [TestCase("[1,[12,[1,2]]]", "[1,[[6,6],[1,2]]]")]
        public void SplitTests(string input, string expected)
        {
            Assert.AreEqual(expected, Split(input));
        }

        [Test]
        public void SumTestData()
        {
            var data = File.ReadAllLines(@"Day18Part1/testdata.txt");
            Assert.AreEqual("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", Sum(data));
        }

        [Theory]
        [TestCase("[[1,2],[[3,4],5]]", 143)]
        [TestCase("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
        [TestCase("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
        [TestCase("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
        [TestCase("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
        [TestCase("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
        public void SumNumbers(string number, int expected)
        {
            Assert.AreEqual(expected, new ComplexNumber(number).Sum());
        }
    
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day18Part1/testdata2.txt");
            var sut = new Day18Part1();
            Assert.AreEqual(4140, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day18Part1/data.txt");
            var sut = new Day18Part1();
            Assert.AreEqual(3675, sut.Run(data));
        }
    }
}