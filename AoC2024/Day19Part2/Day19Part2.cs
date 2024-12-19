using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day19Part2;

public class Day19Part2
{
    private long Run(string[] data)
    {
        var cache = new Dictionary<string, long>();
        var availableTowels = data.First().Split(", ").ToList();
        return data.Skip(2).Sum(CanBeMade);

        long CanBeMade(string remaining)
        {
            if (cache.TryGetValue(remaining, out var value)) return value;
            foreach (var availableTowel in availableTowels)
            {
                if (remaining == "") return 1;
                if (remaining.StartsWith(availableTowel))
                    value += CanBeMade(remaining[availableTowel.Length..]);
            }

            cache.Add(remaining, value);

            return value;
        }
    }

    private class Day19Part2Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day19Part2().Run(File.ReadAllLines("Day19Part2/testdata.txt")), Is.EqualTo(16));

        [Test] public void Data() =>
            Assert.That(new Day19Part2().Run(File.ReadAllLines("Day19Part2/data.txt")), Is.EqualTo(333));
    }
}