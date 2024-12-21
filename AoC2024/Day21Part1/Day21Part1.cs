using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day21Part1;

public class Day21Part1
{
    private readonly Dictionary<Vector, char> _directions = new()
    {
        { Vector.Up, '^' },
        { Vector.Right, '>' },
        { Vector.Down, 'v' },
        { Vector.Left, '<' },
    };

    private int Run(string[] data)
    {
        var numericKeypadMovements = GetFastestConnections(new Dictionary<Vector, char>
        {
            { new Vector(0, 0), '7' }, { new Vector(1, 0), '8' }, { new Vector(2, 0), '9' },
            { new Vector(0, 1), '4' }, { new Vector(1, 1), '5' }, { new Vector(2, 1), '6' },
            { new Vector(0, 2), '1' }, { new Vector(1, 2), '2' }, { new Vector(2, 2), '3' },
            { new Vector(0, 3), 'X' }, { new Vector(1, 3), '0' }, { new Vector(2, 3), 'A' },
        });
        var directionalKeypadMovements = GetFastestConnections(new Dictionary<Vector, char>
        {
            { new Vector(0, 0), 'X' }, { new Vector(1, 0), '^' }, { new Vector(2, 0), 'A' },
            { new Vector(0, 1), '<' }, { new Vector(1, 1), 'v' }, { new Vector(2, 1), '>' },
        });

        var total = 0;
        foreach (var row in data)
        {
            Console.WriteLine(row);
            var resultLength = int.MaxValue;

            foreach (var result in Traverse('A', row, "", numericKeypadMovements).Split(",").Where(s => s != string.Empty))
            {
                foreach (var result2 in Traverse('A', result, "", directionalKeypadMovements).Split(",").Where(s => s != string.Empty))
                {
                    foreach (var result3 in Traverse('A', result2, "", directionalKeypadMovements).Split(",").Where(s => s != string.Empty))
                    {
                        resultLength = Math.Min(resultLength, result3.Length);
                    }
                }
            }
            total += resultLength * int.Parse(row[..3]);
            Console.WriteLine(resultLength);
            Console.WriteLine(int.Parse(row[..3]));
            Console.WriteLine(total);
        }
        return total;

        string Traverse(char currentPosition, string remaining, string current, Dictionary<(char, char), List<string>> keypad)
        {
            if (remaining.Length == 0)
            {
                return current + ",";
            }
            var nextPosition = remaining[0];
            var movement = currentPosition != nextPosition ? keypad[(currentPosition, nextPosition)] : [""];
            return string.Join("", movement.SelectMany(m => Traverse(nextPosition, remaining[1..], current + m + "A", keypad)));
        }
    }

    private Dictionary<(char, char), List<string>> GetFastestConnections(Dictionary<Vector, char> keypad)
    {
        var blocked = keypad.Where(e => e.Value == 'X').Select(e => e.Key).Single();
        var keypadList = keypad.Where(e => e.Value != 'X').ToList();
        var result = new Dictionary<(char, char), List<string>>();
        foreach (var first in keypadList)
        {
            foreach (var second in keypadList.Where(second => first.Key != second.Key))
            {
                result.Add((first.Value, second.Value), GetFastestConnections(first.Key, second.Key, blocked));
            }
        }

        return result;
    }

    private List<string> GetFastestConnections(Vector first, Vector second, Vector blocked)
    {
        var result = new List<string>();

        var delta = second - first;
        var xMovement = Enumerable.Range(1, Math.Abs(delta.X)).Select(_ => new Vector(delta.X > 0 ? 1 : -1, 0)).ToList();
        var yMovement = Enumerable.Range(1, Math.Abs(delta.Y)).Select(_ => new Vector(0, delta.Y > 0 ? 1 : -1)).ToList();

        var current = first;
        if (xMovement.Count != 0)
        {
            var r = "";
            var vectors = xMovement.Concat(yMovement).ToList();
            var addX = true;
            foreach (var vector in vectors)
            {
                current += vector;
                r += _directions[vector];
                if (blocked == current) addX = false;
            }
            if (addX) result.Add(r);
        }
        
        current = first;
        if (yMovement.Count != 0)
        {
            var r = "";
            var vectors = yMovement.Concat(xMovement).ToList();
            var addY = true;
            foreach (var vector in vectors)
            {
                current += vector;
                r += _directions[vector];
                if (blocked == current) addY = false;
            }
            if (addY) result.Add(r);
        }

        return result;
    }


    private class Day21Part1Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day21Part1().Run(File.ReadAllLines("Day21Part1/testdata.txt")), Is.EqualTo(126384));

        [Test] public void Data() =>
            Assert.That(new Day21Part1().Run(File.ReadAllLines("Day21Part1/data.txt")), Is.EqualTo(163086));
    }
}
        
// Console.WriteLine("numericKeypadMovements");
// foreach (var numericKeypadMovement in numericKeypadMovements)
// {
//     Console.Write(numericKeypadMovement.Key);
//     Console.Write(" ");
//     foreach (var movement in numericKeypadMovement.Value)
//     {
//         Console.Write(string.Join(",", movement));
//         Console.Write(" ");
//     }
//     Console.WriteLine("");
// }
//
// Console.WriteLine("directionalKeypadMovements");
// foreach (var directionalKeypadMovement in directionalKeypadMovements)
// {
//     Console.Write(directionalKeypadMovement.Key);
//     Console.Write(" ");
//     foreach (var movement in directionalKeypadMovement.Value)
//     {
//         Console.Write(string.Join(",", movement));
//         Console.Write(" ");
//     }
//     Console.WriteLine("");
// }