using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day03Part1;

public class Day03Part1
{
    private int Run(IEnumerable<string> data)
    {
        var lastNumberPos = new Vector(-2, -2);
        var allNumbers = new List<(List<Vector> vectors, string value)>();
        var allNeighborsOfSymbols = new HashSet<Vector>();
        const string notSymbols = "1234567890.";
        var deltas = new List<Vector>
        {
            new(-1, -1),
            new(-1, 0),
            new(-1, 1),
            new(0, -1),
            new(0, 1),
            new(1, -1),
            new(1, 0),
            new(1, 1),
        };
        var board = data
            .SelectMany(
                (s, y) => s.Select((value, x) =>
                    (
                        pos: new Vector(x, y),
                        value: value.ToString()
                    )
                )
            );
        foreach (var (pos, value) in board)
        {
            if (int.TryParse(value, out _))
            {
                if (lastNumberPos.Subtract(pos) == new Vector(-1, 0))
                {
                    var number = allNumbers.Last();
                    allNumbers.Remove(number);
                    allNumbers.Add((
                        number.vectors.Concat(new List<Vector> { pos }).ToList(),
                        number.value + value
                    ));
                }
                else
                {
                    allNumbers.Add((new List<Vector> { pos }, value));
                }

                lastNumberPos = pos;
            }

            if (!notSymbols.Contains(value))
            {
                foreach (var vector in deltas)
                {
                    allNeighborsOfSymbols.Add(pos.Add(vector));
                }
            }
        }

        return allNeighborsOfSymbols
            .SelectMany(pos => allNumbers.Where(n => n.vectors.Contains(pos)))
            .DistinctBy(n => n.vectors.First())
            .ToDictionary(
                n => n.vectors.First(),
                n => n.value
            )
            .Sum(e => int.Parse(e.Value));
    }

    private class Day03Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day03Part1/testdata.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(4361));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day03Part1/data.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(560670));
        }
    }
}