using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AoC2022.Day07Part1;

public class Day07Part1
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

        return directorySizes.Where(d => d.Value < 100000).Sum(d => d.Value);
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

    private class Day07Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day07Part1/testdata.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(95437));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day07Part1/data.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1443806));
        }
    }
}