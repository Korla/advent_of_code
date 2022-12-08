using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace aoc2022.Day08Part1;

public class Day08Part1
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

        return forest.Count(tree => !_directions.All(direction => IsNotVisible(forest, tree, direction)));
    }

    private bool IsNotVisible(IReadOnlyDictionary<Vector,int> forest, KeyValuePair<Vector,int> tree, Vector direction) => 
        TraverseDirection(forest, tree, direction).Any(otherTreeHeight => otherTreeHeight >= tree.Value);

    private IEnumerable<int> TraverseDirection(IReadOnlyDictionary<Vector, int> forest, KeyValuePair<Vector, int> tree, Vector direction)
    {
        var current = tree.Key;
        while (forest.TryGetValue(current = new Vector(current.X + direction.X, current.Y + direction.Y), out var otherTree))
        {
            yield return otherTree;
        }
    }

    private class Day08Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day08Part1/testdata.txt");
            var sut = new Day08Part1();
            Assert.AreEqual(21, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day08Part1/data.txt");
            var sut = new Day08Part1();
            Assert.AreEqual(1829, sut.Run(data));
        }
    }
}