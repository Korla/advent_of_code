using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day09Part1;

public class Day09Part1
{
    private readonly Dictionary<string, Vector> _directions = new()
    {
        {"R", new Vector(1, 0)},
        {"L", new Vector(-1, 0)},
        {"U", new Vector(0, 1)},
        {"D", new Vector(0, -1)}
    };

    private int Run(IReadOnlyList<string> data)
    {
        var visited = new HashSet<Vector>();
        var currentHead = new Vector(0, 0);
        var currentTail = new Vector(0, 0);
        visited.Add(currentTail);
        foreach (var row in data)
        {
            var parts = row.Split(' ');
            var direction = _directions[parts.First()];
            var length = int.Parse(parts.Last());
            foreach (var i in Enumerable.Range(0, length))
            {
                var oldHead = currentHead;
                currentHead = currentHead.Add(direction);
                if (currentHead.Subtract(currentTail).Length() >= 2)
                {
                    currentTail = oldHead;
                }
                visited.Add(currentTail);
            }
        }
        return visited.Count;
    }

    private class Day09Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day09Part1/testdata.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(13));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day09Part1/data.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(6098));
        }
    }
}