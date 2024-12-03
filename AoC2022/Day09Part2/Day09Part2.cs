using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day09Part2;

public class Day09Part2
{
    private readonly Dictionary<string, Vector> _directions = new()
    {
        {"R", new Vector(0, 1)},
        {"L", new Vector(0, -1)},
        {"D", new Vector(1, 0)},
        {"U", new Vector(-1, 0)},
    };

    private readonly List<Vector> _possibleMoveDirections = new()
    {
        new Vector(1, 1),
        new Vector(1, 0),
        new Vector(1, -1),
        new Vector(0, 1),
        new Vector(0, -1),
        new Vector(-1, 1),
        new Vector(-1, 0),
        new Vector(-1, -1),
    };

    private int Run(IEnumerable<string> data)
    {
        var visited = new HashSet<Vector>();
        var start = new Vector(0, 0);
        var knots = new LinkedList<Vector>();
        foreach (var i in Enumerable.Range(0, 10))
        {
            knots.AddLast(start);
        }

        foreach (var row in data)
        {
            var parts = row.Split(' ');
            var length = int.Parse(parts.Last());
            foreach (var i in Enumerable.Range(0, length))
            {
                var knot = knots.First;
                knot.Value = knot.Value.Add(_directions[parts.First()]);
                while (knot.Next != null)
                {
                    var parent = knot;
                    knot = knot.Next;
                    if (!(knot.Value.Subtract(parent.Value).Length() >= 2)) continue;

                    var shortestMove = _possibleMoveDirections
                        .Select(moveDirection =>
                            (
                                moveDirection,
                                possibleDistanceToParent: parent.Value.Subtract(knot.Value.Add(moveDirection)).Length()
                            )
                        )
                        .MinBy(b => b.possibleDistanceToParent)
                        .moveDirection;
                    knot.Value = knot.Value.Add(shortestMove);
                }
                visited.Add(knots.Last());
            }
        }
        return visited.Count;
    }

    private class Day09Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day09Part2/testdata.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(36));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day09Part2/data.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(2597));
        }
    }
}