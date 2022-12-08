using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace aoc2022.Day08Part2;

public class Day08Part2
{
    private readonly List<Vector> _directions = new()
    {
        new Vector(1, 0),
        new Vector(-1, 0),
        new Vector(0, 1),
        new Vector(0, -1)
    };

    private int Run(IReadOnlyList<string> data)
    {
        var forest = new Dictionary<Vector, int>();
        for (var y = 0; y < data.Count; y++)
        {
            var row = data[y];
            for (var x = 0; x < data.Count; x++)
            {
                forest.Add(new Vector(x, y), int.Parse(row[x].ToString()));
            }
        }

        return forest.Max(tree => _directions.Multiply(direction => LookInDirection(forest, tree, direction).Count()));
    }

    private IEnumerable<int> LookInDirection(IReadOnlyDictionary<Vector, int> forest, KeyValuePair<Vector, int> tree, Vector direction)
    {
        var current = tree.Key;
        while (forest.TryGetValue(current = new Vector(current.X + direction.X, current.Y + direction.Y), out var otherTree))
        {
            yield return otherTree;
            if (otherTree >= tree.Value) break;
        }
    }

    private class Day08Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day08Part2/testdata.txt");
            var sut = new Day08Part2();
            Assert.AreEqual(8, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day08Part2/data.txt");
            var sut = new Day08Part2();
            Assert.AreEqual(291840, sut.Run(data));
        }
    }
}