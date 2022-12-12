using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC2022.Day05Part1;

public class Day05Part1
{
    private string Run(string[] data)
    {
        var lists = new Dictionary<int, List<char>>();
        var j = 0;
        foreach (var row in data)
        {
            j++;
            if (!row.Contains("["))
            {
                break;
            }
            var i = 1;
            foreach (var s in Salmon(row, 4))
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    var exists = lists.TryGetValue(i, out var list);
                    if (!exists)
                    {
                        list = new List<char>();
                        lists.Add(i, list);
                    }
                    list.Add(s[1]);
                }
                i++;
            }
        }

        var stacks = lists
            .OrderBy(l => l.Key)
            .ToDictionary(
                l => l.Key,
                l =>
                {
                    l.Value.Reverse();
                    return new Stack<char>(l.Value);
                }
            );

        data = data.Skip(j + 1).ToArray();
        
        foreach (var row in data)
        {
            var instructions = Regex.Replace(row, "move |from |to ", "").Split(" ").Select(int.Parse).ToArray();
            foreach (var i in Enumerable.Range(0, instructions[0]))
            {
                var item = stacks[instructions[1]].Pop();
                stacks[instructions[2]].Push(item);
            }
        }

        return string.Join("", stacks.Select(s => s.Value.Peek()));
    }

    private IEnumerable<string> Salmon(string whole, int partLength)
    {
        while (whole.Any())
        {
            yield return string.Join("", whole.Take(partLength));
            whole = string.Join("", whole.Skip(partLength));
        }
    }
    
    private class Day05Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day05Part1/testdata.txt");
            var sut = new Day05Part1();
            Assert.AreEqual("CMZ", sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day05Part1/data.txt");
            var sut = new Day05Part1();
            Assert.AreEqual("TLNGFGMFN", sut.Run(data));
        }
    }
}