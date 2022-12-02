using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;
using NUnit.Framework;

namespace aoc2021.Day19Part2;

public class Day19Part2
{
    private static List<Vector3> Positions = new() {Vector3.Zero};
    private class Scanner
    {
        public List<Vector3> Report { get; set; } = new();

        public bool Match(Scanner other, int minimumMatching, out Scanner intersected)
        {
            var allRotatedOtherPositionDeltas = other.GetAllRotatedDeltas();
            foreach (var (current, deltas) in GetDeltas())
            {
                foreach (var otherRotatedPositionDeltas in allRotatedOtherPositionDeltas)
                {
                    foreach (var (otherCurrent, otherDeltas) in otherRotatedPositionDeltas)
                    {
                        var intersection = otherDeltas.Intersect(deltas);
                        if (intersection.Count() >= minimumMatching - 1)
                        {
                            var deltasToAdd = otherDeltas.Select(vBB => current + vBB);
                            intersected = new Scanner
                            {
                                Report = Report.Concat(deltasToAdd).Distinct().ToList()
                            };
                            Positions.Add(current - otherCurrent);
                            return true;
                        }
                    }
                }
            }

            intersected = new Scanner();
            return false;
        }

        private List<(Vector3 current, List<Vector3> deltas)> GetDeltas()
        {
            return Report.Select(current =>
            {
                var deltas = Report.Where(v => v != current).Select(v => v - current).ToList();
                return (current, deltas);
            }).ToList();
        }

        private List<List<(Vector3 current, List<Vector3> deltas)>> GetAllRotatedDeltas()
        {
            return AllRotations(Report).Select(rotatedReport => rotatedReport.Select(current =>
            {
                var deltas = rotatedReport.Where(v => v != current).Select(v => v - current).ToList();
                return (current, deltas);
            }).ToList()).ToList();
        }
    }

    private int Run(IEnumerable<string> data)
    {
        var scanners = Parse(data);
        var result = scanners.First();
        var queue = new Queue<Scanner>();
        foreach (var scanner in scanners.Skip(1))
        {
            queue.Enqueue(scanner);
        }
        while (queue.Any())
        {
            var current = queue.Dequeue();
            if (!result.Match(current, 12, out var nextResult))
            {
                queue.Enqueue(current);
            }
            else
            {
                result = nextResult;
            }
        }

        var combinations = new List<int>();
        foreach (var p1 in Positions)
        {
            foreach (var p2 in Positions.Where(p => p != p1))
            {
                combinations.Add(Manhattan(p1, p2));
            }
        }
        return combinations.Max();
    }

    private int Manhattan(Vector3 first, Vector3 second) => (int) (Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y) + Math.Abs(first.Z - second.Z));

    private static List<Scanner> Parse(IEnumerable<string> data)
    {
        var scanners = new List<Scanner>();
        foreach (var row in data)
        {
            var scannerMatch = row.StartsWith("--");
            var parts = row.Split(',');
            if(scannerMatch) scanners.Add(new Scanner());
            else if (parts.Length == 3)
                scanners.Last().Report.Add(new Vector3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
        }

        return scanners;
    }

    private static IEnumerable<List<Vector3>> AllRotations(List<Vector3> vectors)
    {
        foreach (var _ in Enumerable.Range(0,2))
        {
            foreach (var __ in Enumerable.Range(0,3))
            {
                vectors = vectors.Select(Roll).ToList();
                yield return vectors;
                foreach (var ___ in Enumerable.Range(0,3))
                {
                    vectors = vectors.Select(Turn).ToList();
                    yield return vectors;
                }
            }

            vectors = vectors.Select(Roll).ToList();
            vectors = vectors.Select(Turn).ToList();
            vectors = vectors.Select(Roll).ToList();
        }

        Vector3 Roll(Vector3 v) => new(v.X, v.Z, -v.Y);
        Vector3 Turn(Vector3 v) => new(-v.Y, v.X, v.Z);
    }

    private class Tests
    {
        [Test]
        public void TestParse()
        {
            var data = File.ReadAllLines(@"Day19Part2/testdata.txt");
            var scanners = Parse(data);
            Assert.AreEqual(5, scanners.Count);
            Assert.AreEqual(25, scanners.First().Report.Count);
        }
        
        [Test]
        public void TestRotate()
        {
            var sequence = AllRotations(new List<Vector3>{new(1, 2, 3), new(1, 1, 1)}).Distinct();
            Assert.AreEqual(24, sequence.Count());
            Assert.AreEqual(2, sequence.First().Count);
        }

        [Test]
        public void TestMatchSimple()
        {
            var scanner0 = new Scanner
            {
                Report = new()
                {
                    new(0, 2, 0),
                    new(4, 1, 0),
                    new(3, 3, 0)
                }
            };
            var scanner1 = new Scanner
            {
                Report = new()
                {
                    new(-1, -1, 0),
                    new(-5, 0, 0),
                    new(-2, 1, 0),
                    new(0, 1, 0)
                }
            };
            Assert.AreEqual(true, scanner0.Match(scanner1, 3, out var intersected));
            Assert.NotNull(intersected);
            Assert.Contains(new Vector3(5, 3, 0), intersected.Report);
        }
        
        [Test]
        public void TestMatchSimpleRotated()
        {
            var scanner0 = new Scanner
            {
                Report = new()
                {
                    new(0, 2, 0),
                    new(4, 1, 0),
                    new(3, 3, 0)
                }
            };
            var scanner1 = new Scanner
            {
                Report = new()
                {
                    new(-1, 1, 0),
                    new(1, 2, 0),
                    new(0, 5, 0),
                    new(1, 0, 0)
                }
            };
            Assert.AreEqual(true, scanner0.Match(scanner1, 3, out var intersected));
            Assert.NotNull(intersected);
            Assert.Contains(new Vector3(5, 3, 0), intersected.Report);
        }

        [Test]
        public void TestMatch()
        {
            var data = File.ReadAllLines(@"Day19Part2/testdata.txt");
            var scanners = Parse(data);
            Assert.AreEqual(true, scanners[0].Match(scanners[1], 12, out var intersected));
            Assert.NotNull(intersected);
        }

        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day19Part2/testdata.txt");
            var sut = new Day19Part2();
            Assert.AreEqual(13192, sut.Run(data));
        }
        
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day19Part2/data.txt");
            var sut = new Day19Part2();
            Assert.AreEqual(13192, sut.Run(data));
        }
    }
}