using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2024.Day07Part2;

public class Day07Part2
{
    private readonly char[] _operators = ['*', '+', '|']; 

    private double Run(IEnumerable<string> data)
    {
        return data
            .Sum(row =>
            {
                var match = Regex.Matches(row, @"(\d+): ([\d+ ]+)");
                var target = double.Parse(match.First().Groups[1].Value);
                var parts = match.First().Groups[2].Value.Split(" ").Select(double.Parse).ToArray();

                var results = new List<double> { parts.First() };
                return parts
                    .Skip(1)
                    .Aggregate(
                        results,
                        (result, part) => result
                            .SelectMany(current =>
                                _operators.Select(o => o switch
                                {
                                    '+' => current + part,
                                    '*' => current * part,
                                    _ => double.Parse($"{current}{part}")
                                }))
                            .ToList()
                    )
                    .FirstOrDefault(r => r == target);
            });
    }

    private class Day07Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day07Part2/testdata.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(11387));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day07Part2/data.txt");
            var sut = new Day07Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(275791737999003));
        }
    }
}