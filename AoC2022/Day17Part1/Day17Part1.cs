using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day17Part1;

public class Day17Part1
{
    private List<Vector[]> _rocks = new()
    {
        new[]
        {
            new Vector(0, 0),
            new Vector(1, 0),
            new Vector(2, 0),
            new Vector(3, 0),
        },
        new[]
        {
            new Vector(1, 0),
            new Vector(0, 1),
            new Vector(1, 1),
            new Vector(2, 1),
            new Vector(1, 2),
        },
        new[]
        {
            new Vector(0, 0),
            new Vector(1, 0),
            new Vector(2, 0),
            new Vector(2, 1),
            new Vector(2, 2),
        },
        new[]
        {
            new Vector(0, 0),
            new Vector(0, 1),
            new Vector(0, 2),
            new Vector(0, 3),
        },
        new[]
        {
            new Vector(0, 0),
            new Vector(0, 1),
            new Vector(1, 0),
            new Vector(1, 1),
        }
    };

    private int Run(IEnumerable<string> data)
    {
        var board = Enumerable.Range(0, 7).Select(x => new Vector(x, 0)).ToHashSet();
        var maxY = 0;
        var currentPos = new Vector(2, 4);
        
        using var jets = Jets(data.First()).GetEnumerator();
        for(var rockCount = 0; rockCount < 2022; rockCount++)
        {
            var rock = _rocks[rockCount % 5];
            while (true)
            {
                jets.MoveNext();
                var jet = jets.Current;
        
                // attempt lateral move
                var lateralMove = new Vector(jet ? 1 : -1, 0);
                var lateralIntersectChecker = currentPos.Add(lateralMove);
                var newLateralRockPositions = rock.Select(pos => pos.Add(lateralIntersectChecker));
                if (!newLateralRockPositions.Any(p => board.Contains(p) || !p.X.IsBetweenInclusive(0, 6)))
                {
                    currentPos = currentPos.Add(lateralMove);
                    currentPos = currentPos with
                    {
                        X = currentPos.X.Limit(0, 6)
                    };
                }
                // board.Concat(rock.Select(r => r.Add(currentPos))).Print();
        
                // check if we are at rest
                var verticalMove = new Vector(0, -1);
                var verticalIntersectChecker = currentPos.Add(verticalMove);
                var newVerticalRockPositions = rock.Select(pos => pos.Add(verticalIntersectChecker)).ToList();
                if (!newVerticalRockPositions.Any(p => board.Contains(p)))
                {
                    currentPos = currentPos.Add(verticalMove);
                    // board.Concat(rock.Select(r => r.Add(currentPos))).Print();
                }
                else
                {
                    foreach (var newVerticalRockPosition in rock.Select(pos => pos.Add(currentPos)))
                    {
                        board.Add(newVerticalRockPosition);
                    }
                    // board.Print();
        
                    maxY = board.Max(pos => pos.Y);
                    currentPos = currentPos with {Y = maxY + 4, X = 2};
                    break;
                }
            }
        }

        return maxY;
    }

    private static IEnumerable<bool> Jets(string input)
    {
        while (true)
        {
            foreach (var s in input)
            {
                yield return s == '>';
            }
        }
    }

    private class Day17Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day17Part1/testdata.txt");
            var sut = new Day17Part1();
            Assert.AreEqual(3068, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day17Part1/data.txt");
            var sut = new Day17Part1();
            Assert.AreEqual(-1, sut.Run(data));
        }
    }
}