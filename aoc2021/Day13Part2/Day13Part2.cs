using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2021.Day13Part2;

public class Day13Part2
{
    private int Run(IList<string> data)
    {
        var coords = new HashSet<(int X, int Y)>(); 
        var folds = new List<(bool alongX, int pos)>();
        foreach (var row in data)
        {
            var xy = row.Split(",");
            if (xy.Length == 2)
            {
                coords.Add((int.Parse(xy[0]), int.Parse(xy[1])));
                continue;
            }

            var foldInstr = row.Split(" ");
            if (foldInstr.Length == 3)
            {
                var instr = foldInstr.Last().Split("=");
                folds.Add((instr[0] == "x", int.Parse(instr[1])));
            }
        }

        var result = folds.Aggregate(coords, (prev, curr) =>
        {
            var nextCoords = new HashSet<(int X, int Y)>();
            var (alongX, pos) = curr;
            foreach (var key in prev)
            {
                switch (alongX)
                {
                    case true when key.X > pos:
                        nextCoords.Add((pos * 2 - key.X, key.Y));
                        break;
                    case false when key.Y > pos:
                        nextCoords.Add((key.X, pos * 2 - key.Y));
                        break;
                    default:
                        nextCoords.Add(key);
                        break;
                }
            }

            return nextCoords;
        });
        for (var y = 0; y <= 6; y++)
        {
            for (var x = 0; x <= 40; x++)
            {
                if (result.TryGetValue((x, y), out var val))
                {
                    Console.Write("x");
                }
                else
                {
                    Console.Write("-");
                }

            }

            Console.WriteLine();
        }

        return result.Count;
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day13Part2/testdata.txt");
            var sut = new Day13Part2();
            Assert.AreEqual(16, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day13Part2/data.txt");
            var sut = new Day13Part2();
            Assert.AreEqual(91, sut.Run(data));
        }
    }
}