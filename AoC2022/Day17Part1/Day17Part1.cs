using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day17Part1;

public class Day17Part1
{
    private List<Rock> _rocks = new()
    {
        new(
            new[]
            {
                new LongVector(0, 0),
                new LongVector(1, 0),
                new LongVector(2, 0),
                new LongVector(3, 0),
            },
            3,
            0
        ),
        new(
            new[]
            {
                new LongVector(1, 0),
                new LongVector(0, 1),
                new LongVector(1, 1),
                new LongVector(2, 1),
                new LongVector(1, 2),
            },
            2,
            0
        ),
        new(
            new[]
            {
                new LongVector(0, 0),
                new LongVector(1, 0),
                new LongVector(2, 0),
                new LongVector(2, 1),
                new LongVector(2, 2),
            },
            2,
            0
        ),
        new(
            new[]
            {
                new LongVector(0, 0),
                new LongVector(0, 1),
                new LongVector(0, 2),
                new LongVector(0, 3),
            },
            0,
            0
        ),
        new(
            new[]
            {
                new LongVector(0, 0),
                new LongVector(0, 1),
                new LongVector(1, 0),
                new LongVector(1, 1),
            },
            1,
            0
        )
    };

    private double Run(IEnumerable<string> data)
    {
        var board = new LongMatrix(7);
        long maxY = 0;
        var currentPos = new LongVector(2, 4);

        var jets = data.First();
        var currentJet = 0;
        const double totalRockCount = 2022;
        bool jet;
        Rock rock;
        LongVector nextHorizontalPos;
        LongVector verticalIntersectChecker;
        var verticalMove = new LongVector(0, -1);
        var leftMove = new LongVector(-1, 0);
        var rightMove = new LongVector(1, 0);
        for (double rockCount = 0; rockCount < totalRockCount; rockCount++)
        {
            rock = _rocks[(int)rockCount % 5];
            while (true)
            {
                jet = jets[currentJet] == '>';
                currentJet = (currentJet + 1) % jets.Length;

                // attempt lateral move
                nextHorizontalPos = currentPos.Add(jet ? rightMove : leftMove);
                var isWithinTheBounds = 0 <= nextHorizontalPos.X + rock.MinX && 6 >= nextHorizontalPos.X + rock.MaxX;
                if (isWithinTheBounds && !board.Overlaps(rock.Elements.Select(p => p.Add(nextHorizontalPos))))
                {
                    currentPos = nextHorizontalPos;
                }
                // board.Concat(rock.Select(r => r.Add(currentPos))).Print();

                // check if we are at rest
                verticalIntersectChecker = currentPos.Add(verticalMove);
                if (!board.Overlaps(rock.Elements.Select(p => p.Add(verticalIntersectChecker))))
                {
                    currentPos = verticalIntersectChecker;
                    // board.Concat(rock.Select(r => r.Add(currentPos))).Print();
                }
                else
                {
                    // board = board.Concat(rock.Select(pos => pos.Add(currentPos))).ToHashSet();
                    foreach (var t in rock.Elements)
                    {
                        board.AddVector(t.Add(currentPos));
                    }
                    // board.Print();

                    maxY = board.MaxY;
                    currentPos = currentPos with { X = 2, Y = maxY + 4 };
                    break;
                }
            }
        }

        return maxY;
    }

    public record Rock(LongVector[] Elements, int MaxX, int MinX);

    private class Day17Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day17Part1/testdata.txt");
            var sut = new Day17Part1();
            Assert.That(sut.Run(data), Is.EqualTo(3068));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day17Part1/data.txt");
            var sut = new Day17Part1();
            Assert.That(sut.Run(data), Is.EqualTo(3184));
        }
    }
}