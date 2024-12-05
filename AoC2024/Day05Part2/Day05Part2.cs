using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utils;

namespace AoC2024.Day05Part2;

public class Day05Part2
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

        var nonMatchingingUpdates = updates
            .Where(update =>
                !update
                    .Select((number, i) => (number, rest: update.Skip(i + 1).ToList()))
                    .All(g => g.rest.All(r => OkToBeBefore(g.number, r, beforeRules, afterRules)))
            );
        var corrected = new List<List<int>>();
        foreach (var update in nonMatchingingUpdates)
        {
            var oldUpdate = update;
            var changed = true;
            while (changed)
            {
                changed = false;
                var newUpdate = new List<int>();
                for (var i = 0; i < oldUpdate.Count; i++)
                {
                    if (oldUpdate.Count > i + 1 && !changed && !OkToBeBefore(oldUpdate[i], oldUpdate[i + 1], beforeRules, afterRules))
                    {
                        newUpdate.Add(oldUpdate[i + 1]);
                        newUpdate.Add(oldUpdate[i]);
                        i++;
                        changed = true;
                    }
                    else
                    {
                        newUpdate.Add(oldUpdate[i]);
                    }
                }
                oldUpdate = newUpdate;
            }
            corrected.Add(oldUpdate);
        }
        return corrected.Sum(matching => matching[(matching.Count - 1) / 2]);
    }

    private bool OkToBeBefore(int beforePage, int afterPage, Dictionary<int, List<int>> beforeRules, Dictionary<int, List<int>> afterRules)
    {
        return 
            (!beforeRules.TryGetValue(afterPage, out var befores) || befores.Contains(beforePage)) &&
            (!afterRules.TryGetValue(beforePage, out var afters) || afters.Contains(afterPage));
    }

    private class Day05Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day05Part2/testdata.txt");
            var sut = new Day05Part2();
            Assert.That(sut.Run(data), Is.EqualTo(123));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day05Part2/data.txt");
            var sut = new Day05Part2();
            Assert.That(sut.Run(data), Is.EqualTo(6456));
        }
    }
}