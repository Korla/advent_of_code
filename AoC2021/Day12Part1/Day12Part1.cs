using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace AoC2021.Day12Part1;

public class Day12Part1
{
    private int Run(IList<string> data)
    {
        var nodes = data.SelectMany(row =>
            {
                var parts = row.Split("-");
                return new[] {(parts.First(), parts.Last()), (parts.Last(), parts.First())};
            })
            .GroupBy(a => a.Item1, a => a.Item2)
            .ToDictionary(a => a.Key, a => a.ToList());

        return GetAllPaths(new List<string>(), "start", nodes).Count(path => path.Last() == "end");
    }

    private IEnumerable<IReadOnlyCollection<string>> GetAllPaths(IReadOnlyCollection<string> path, string current, IReadOnlyDictionary<string, List<string>> nodes)
    {
        if (IsLowercase(current) && path.Contains(current))
        {
            return new[] {path};
        }
        var nextPath = path.Concat(new []{current}).ToList();
        return current == "end" ?
            new[] {nextPath} :
            nodes[current].SelectMany(next => GetAllPaths(nextPath, next, nodes));
    }

    private bool IsLowercase(string node) => node == node.ToLower();

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day12Part1/testdata.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(10, sut.Run(data));
        }
        
        [Test]
        public void TestData2()
        {
            var data = File.ReadAllLines(@"Day12Part1/testdata2.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(19, sut.Run(data));
        }
        
        [Test]
        public void TestData3()
        {
            var data = File.ReadAllLines(@"Day12Part1/testdata3.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(226, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day12Part1/data.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(3761, sut.Run(data));
        }
    }
}