using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2024.Day03Part2;

public class Day03Part2
{
    private record Aggregate(int Sum, bool Enabled);

    private int Run(IEnumerable<string> data)
    {
        return Regex.Matches(string.Join("", data), @"mul\((\d+),(\d+)\)|do\(\)|don't\(\)")
            .Aggregate(
                new Aggregate(0, true),
                (aggregate, match) =>
                {
                    switch (match.ToString())
                    {
                        case "do()":
                            return aggregate with { Enabled = true };
                        case "don't()":
                            return aggregate with { Enabled = false };
                    }

                    if (aggregate.Enabled)
                    {
                        return aggregate with
                        {
                            Sum = aggregate.Sum + int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value)
                        };
                    }

                    return aggregate;
                })
            .Sum;
    }
    
    private class Day03Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day03Part2/testdata.txt");
            var sut = new Day03Part2();
            Assert.That(sut.Run(data), Is.EqualTo(48));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day03Part2/data.txt");
            var sut = new Day03Part2();
            Assert.That(sut.Run(data), Is.EqualTo(75920122));
        }
    }
}