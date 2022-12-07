using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace aoc2022.Day07Part2;

public class Day07Part2
{
    private int Run(IEnumerable<string> data)
    {
        var directorySizes = new Dictionary<string, int>();
        var dir = new Stack<string>();
        foreach (var row in data)
        {
            ParseRow(row, directorySizes, dir);
        }

        while (dir.Count > 1)
        {
            ParseRow("$ cd ..", directorySizes, dir);
        }
        
        var orderedDirectorySizes = directorySizes.Select(d => d.Value).OrderBy(d => d).ToList();
        return orderedDirectorySizes.First(d => d > orderedDirectorySizes.Last() - 40000000);
    }

    private static void ParseRow(string row, IDictionary<string, int> directorySizes, Stack<string> dir)
    {
        if (row.StartsWith("$ cd .."))
        {
            var containedSize = directorySizes[string.Join("", dir)];
            dir.Pop();
            directorySizes[string.Join("", dir)] += containedSize;
            return;
        }

        if (row.StartsWith("$ cd"))
        {
            dir.Push(row.Split(" ").Last());
            directorySizes.Add(string.Join("", dir), 0);
            return;
        }

        if (Regex.IsMatch(row, "^[0-9]"))
        {
            directorySizes[string.Join("", dir)] += int.Parse(row.Split(" ").First());
        }
    }

    private class Day07Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day07Part2/testdata.txt");
            var sut = new Day07Part2();
            Assert.AreEqual(24933642, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day07Part2/data.txt");
            var sut = new Day07Part2();
            Assert.AreEqual(942298, sut.Run(data));
        }
    }
}