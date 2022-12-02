using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2021.Day13Part1;

public class Day13Part1
{
    private int Run(IList<string> data)
    {
        var coords = new Dictionary<(int X, int Y), bool>(); 
        var folds = new List<(bool alongX, int pos)>();
        foreach (var row in data)
        {
            var xy = row.Split(",");
            if (xy.Length == 2)
            {
                coords.Add((int.Parse(xy[0]), int.Parse(xy[1])), true);
                continue;
            }

            var foldInstr = row.Split(" ");
            if (foldInstr.Length == 3)
            {
                var instr = foldInstr.Last().Split("=");
                folds.Add((instr[0] == "x", int.Parse(instr[1])));
            }
        }

        var (alongX, pos) = folds.First();
        var result = new Dictionary<(int X, int Y), bool>();
        foreach (var (key, value) in coords)
        {
            switch (alongX)
            {
                case true when key.X > pos:
                    result.TryAdd((pos * 2 - key.X, key.Y), true);
                    break;
                case false when key.Y > pos:
                    result.TryAdd((key.X, pos * 2 - key.Y), true);
                    break;
                default:
                    result.TryAdd(key, true);
                    break;
            }
        }
        return result.Count;
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day13Part1/testdata.txt");
            var sut = new Day13Part1();
            Assert.AreEqual(17, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day13Part1/data.txt");
            var sut = new Day13Part1();
            Assert.AreEqual(689, sut.Run(data));
        }
    }
}