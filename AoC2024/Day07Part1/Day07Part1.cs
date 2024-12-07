using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2024.Day07Part1;

public class Day07Part1
{
    private enum Operators
    {
        None,
        Add,
        Multiply
    }

    private double Run(IEnumerable<string> data)
    {
        return data
            .Sum(row =>
            {
                var match = Regex.Matches(row, @"(\d+): ([\d+ ]+)");
                var target = double.Parse(match.First().Groups[1].Value);
                Console.WriteLine("---------------");
                Console.WriteLine($"target: {target}");
                var parts = match.First().Groups[2].Value.Split(" ").Select(double.Parse).ToArray();

                var results = new List<double> { parts.First() };
                foreach (var part in parts.Skip(1))
                {
                    var operators = new List<Operators> { Operators.Multiply, Operators.Add };
                    results = results.SelectMany(current =>
                        operators.Select(o => o == Operators.Add ? current + part : current * part)).ToList();
                }

                return results.FirstOrDefault(r => r == target);
            });
    }

    private class Day07Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day07Part1/testdata.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(3749));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day07Part1/data.txt");
            var sut = new Day07Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(1399219271639));
        }
    }
}