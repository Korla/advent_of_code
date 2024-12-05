using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utils;

namespace AoC2024.Day05Part1;

public class Day05Part1
{
    private int Run(IEnumerable<string> data)
    {
        var afterRules = new Dictionary<int, List<int>>();
        var beforeRules = new Dictionary<int, List<int>>();
        var updates = new List<List<int>>();
        foreach (var row in data)
        {
            var match = Regex.Matches(row, @"(\d+)\|(\d+)");
            if (match.Any())
            {
                afterRules.AddToListOrAdd(int.Parse(match[0].Groups[1].Value), int.Parse(match[0].Groups[2].Value));
                beforeRules.AddToListOrAdd(int.Parse(match[0].Groups[2].Value), int.Parse(match[0].Groups[1].Value));
            }
            var match2 = Regex.Matches(row, @"^\d+(,\d+)*$");
            if (match2.Any())
            {
                updates.Add(row.Split(',').Select(int.Parse).ToList());
            }
        }

        return updates
            .Where(update =>
                update
                    .Select((number, i) => (number, rest: update.Skip(i + 1).ToList()))
                    .All(g => g.rest.All(r => OkToBeBefore(g.number, r, beforeRules, afterRules)))
            )
            .Sum(matching => matching[(matching.Count - 1) / 2]);
    }

    private bool OkToBeBefore(int before, int after, Dictionary<int, List<int>> beforeRules, Dictionary<int, List<int>> afterRules)
    {
        return 
            (!beforeRules.TryGetValue(after, out var befores) || befores.Contains(before)) &&
            (!afterRules.TryGetValue(before, out var afters) || afters.Contains(after));
    }

    private class Day05Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day05Part1/testdata.txt");
            var sut = new Day05Part1();
            Assert.That(sut.Run(data), Is.EqualTo(143));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day05Part1/data.txt");
            var sut = new Day05Part1();
            Assert.That(sut.Run(data), Is.EqualTo(4569));
        }
    }
}