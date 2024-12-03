using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using NUnit.Framework;

namespace AoC2021.Day19Part1;

public class Day19Part1
{
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
                    foreach (var (_, otherDeltas) in otherRotatedPositionDeltas)
                    {
                        var intersection = otherDeltas.Intersect(deltas);
                        if (intersection.Count() >= minimumMatching - 1)
                        {
                            var deltasToAdd = otherDeltas.Select(vBB => current + vBB);
                            intersected = new Scanner
                            {
                                Report = Report.Concat(deltasToAdd).Distinct().ToList()
                            };
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

        return result.Report.Count;
    }

    private static List<Scanner> Parse(IEnumerable<string> data)
    {
        var scanners = new List<Scanner>();
        foreach (var row in data)
        {
            var scannerMatch = row.StartsWith("--");
            var parts = row.Split(',');
            if (scannerMatch) scanners.Add(new Scanner());
            else if (parts.Length == 3)
                scanners.Last().Report.Add(new Vector3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
        }

        return scanners;
    }

    private static IEnumerable<List<Vector3>> AllRotations(List<Vector3> vectors)
    {
        foreach (var _ in Enumerable.Range(0, 2))
        {
            foreach (var __ in Enumerable.Range(0, 3))
            {
                vectors = vectors.Select(Roll).ToList();
                yield return vectors;
                foreach (var ___ in Enumerable.Range(0, 3))
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
            var data = File.ReadAllLines(@"Day19Part1/testdata.txt");
            var scanners = Parse(data);
            Assert.That(scanners.Count, Is.EqualTo(5));
            Assert.That(scanners.First().Report.Count, Is.EqualTo(25));
        }

        [Test]
        public void TestRotate()
        {
            var sequence = AllRotations(new List<Vector3> { new(1, 2, 3), new(1, 1, 1) }).Distinct();
            Assert.That(sequence.Count(), Is.EqualTo(24));
            Assert.That(sequence.First().Count, Is.EqualTo(2));
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
            Assert.That(scanner0.Match(scanner1, 3, out var intersected), Is.EqualTo(true));
            Assert.That(intersected, Is.Not.Null);
            Assert.That(intersected.Report.Contains(new Vector3(5, 3, 0)));
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
            Assert.That(scanner0.Match(scanner1, 3, out var intersected), Is.EqualTo(true));
            Assert.That(intersected, Is.Not.Null);
            Assert.That(intersected.Report.Contains(new Vector3(5, 3, 0)));
        }

        [Test]
        public void TestMatch()
        {
            var data = File.ReadAllLines(@"Day19Part1/testdata.txt");
            var scanners = Parse(data);
            Assert.That(scanners[0].Match(scanners[1], 12, out var intersected), Is.EqualTo(true));
            Assert.That(intersected, Is.Not.Null);
        }

        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day19Part1/testdata.txt");
            var sut = new Day19Part1();
            Assert.That(sut.Run(data), Is.EqualTo(79));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day19Part1/data.txt");
            var sut = new Day19Part1();
            Assert.That(sut.Run(data), Is.EqualTo(315));
        }
    }
}