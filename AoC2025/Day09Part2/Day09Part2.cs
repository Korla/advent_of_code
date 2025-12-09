using Utils;

namespace AoC2025.Day09Part2;

public class Day09Part2
{
    private long Run(IEnumerable<string> data)
    {
        var d = data.ToList();
        d.Add(d.First());
        var allCorners = d
            .Select(row => row.Split(',').Select(int.Parse).ToList())
            .Select(p => new Vector(p.First(), p.Last()))
            .ToList();
        var allLines = allCorners.Pairwise(MakeLine).SelectMany(line => line).ToHashSet();

        return allCorners
            .AllPairs()
            .Where(pair =>
            {
                var xMin = Math.Min(pair.Item1.X, pair.Item2.X) + 1;
                var yMin = Math.Min(pair.Item1.Y, pair.Item2.Y) + 1;
                var xLength = Math.Abs(pair.Item1.X - pair.Item2.X) - 1;
                var yLength = Math.Abs(pair.Item1.Y - pair.Item2.Y) - 1;

                return xLength > 0
                       && yLength > 0
                       && !Enumerable.Range(xMin, xLength).Any(x => allLines.Contains(new Vector(x, yMin)))
                       && !Enumerable
                           .Range(xMin, xLength)
                           .Any(x => allLines.Contains(new Vector(x, yMin + yLength - 1)))
                       && !Enumerable
                           .Range(yMin, yLength)
                           .Any(y => allLines.Contains(new Vector(xMin + xLength - 1, y)))
                       && !Enumerable
                           .Range(yMin, yLength)
                           .Any(y => allLines.Contains(new Vector(xMin, y)));
            })
            .Select(pair => (long)(Math.Abs(pair.Item1.X - pair.Item2.X) + 1) * (Math.Abs(pair.Item1.Y - pair.Item2.Y) + 1))
            .Max();
    }

    private static HashSet<Vector> MakeLine(Vector v1, Vector v2)
    {
        return (v1.X == v2.X
                ? Enumerable.Range(Math.Min(v1.Y, v2.Y), Math.Abs(v1.Y - v2.Y) + 1)
                    .Select(y => v1 with { Y = y })
                : Enumerable.Range(Math.Min(v1.X, v2.X), Math.Abs(v1.X - v2.X) + 1)
                    .Select(x => v1 with { X = x })
            ).ToHashSet();
    }

    private class Day09Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day09Part2/testdata.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(24));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day09Part2/data.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(1572047142));
        }
    }
}