using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2024.Day19Part1;

public class Day19Part1
{
    private int Run(IEnumerable<string> data)
    {
        data = data.ToList();
        var availableTowels = new HashSet<string>();
        foreach (var towel in data.First().Split(", "))
        {
            availableTowels.Add(towel);
        }
        
        return data.Skip(2).Count(CanBeMade);
        
        bool CanBeMade(string target)
        {
            var queue = new Stack<string>();
            queue.Push(target);
            while (queue.Count > 0)
            {
                var current = queue.Pop();
                foreach (var availableTowel in availableTowels.Where(current.StartsWith))
                {
                    var next = current[availableTowel.Length..];
                    if (next.Length == 0)
                        return true;
                    queue.Push(next);
                }
            }

            return false;
        }
    }

    private class Day19Part1Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day19Part1().Run(File.ReadAllLines("Day19Part1/testdata.txt")), Is.EqualTo(6));

        [Test] public void Data() =>
            Assert.That(new Day19Part1().Run(File.ReadAllLines("Day19Part1/data.txt")), Is.EqualTo(333));
    }
}