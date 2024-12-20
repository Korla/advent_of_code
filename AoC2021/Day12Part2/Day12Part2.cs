﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day12Part2;

public class Day12Part2
{
    private int Run(IList<string> data)
    {
        var nodes = data.SelectMany(row =>
            {
                var parts = row.Split("-");
                return new[] { (parts.First(), parts.Last()), (parts.Last(), parts.First()) };
            })
            .GroupBy(a => a.Item1, a => a.Item2)
            .ToDictionary(a => a.Key, a => a.ToList());

        return GetAllPaths(new List<string>(), "start", nodes).Count(path => path.Last() == "end");
    }

    private IEnumerable<IReadOnlyCollection<string>> GetAllPaths(IReadOnlyCollection<string> path, string current,
        IReadOnlyDictionary<string, List<string>> nodes)
    {
        if (IsLowercase(current) && path.Contains(current))
        {
            if (current is not "start" or "end" && !path.Any(node => node.EndsWith("_alt")))
            {
                return nodes[current].SelectMany(next =>
                    GetAllPaths(path.Concat(new[] { current + "_alt" }).ToList(), next, nodes));
            }

            return new[] { path };
        }

        var nextPath = path.Concat(new[] { current }).ToList();
        return current == "end"
            ? new[] { nextPath }
            : nodes[current].SelectMany(next => GetAllPaths(nextPath, next, nodes));
    }

    private bool IsLowercase(string node) => node == node.ToLower();

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day12Part2/testdata.txt");
            var sut = new Day12Part2();
            Assert.That(sut.Run(data), Is.EqualTo(36));
        }

        [Test]
        public void TestData2()
        {
            var data = File.ReadAllLines(@"Day12Part2/testdata2.txt");
            var sut = new Day12Part2();
            Assert.That(sut.Run(data), Is.EqualTo(103));
        }

        [Test]
        public void TestData3()
        {
            var data = File.ReadAllLines(@"Day12Part2/testdata3.txt");
            var sut = new Day12Part2();
            Assert.That(sut.Run(data), Is.EqualTo(3509));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day12Part2/data.txt");
            var sut = new Day12Part2();
            Assert.That(sut.Run(data), Is.EqualTo(99138));
        }
    }
}