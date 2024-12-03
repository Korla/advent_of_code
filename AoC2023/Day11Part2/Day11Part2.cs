using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day11Part2;

public class Day11Part2
{
    private long Run(IEnumerable<string> data)
    {
        var space = Expand(data.ToList());
        return GetAllPairs(space.ToList())
            .Sum(vectors => vectors.Item1.ManhattanDistance(vectors.Item2));
    }

    private IEnumerable<LongVector> Expand(IReadOnlyList<string> data)
    {
        var galaxies = new List<LongVector>();
        var xToExpand = new List<long>();
        var yToExpand = new List<long>();
        for (var i = 0; i < data.Count; i++)
        {
            var noXGalaxy = true;
            var noYGalaxy = true;
            for (var j = 0; j < data.Count; j++)
            {
                if (data[i][j] == '#')
                {
                    galaxies.Add(new LongVector(i, j));
                    noXGalaxy = false;
                }

                if (data[j][i] == '#')
                {
                    noYGalaxy = false;
                }
            }
            if (noXGalaxy) xToExpand.Add(i);
            if (noYGalaxy) yToExpand.Add(i);
        }

        return galaxies.Select(g => new LongVector(
            g.X + xToExpand.Count(x => x <= g.X) * 999999,
            g.Y + yToExpand.Count(y => y <= g.Y) * 999999
        ));
    }

    private IEnumerable<(LongVector, LongVector)> GetAllPairs(List<LongVector> vectors)
    {
        for (var i = 0; i < vectors.Count; i++)
        {
            for (var j = i; j < vectors.Count; j++)
            {
                yield return (vectors[i], vectors[j]);
            }
        }
    }

    private class Day11Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day11Part2/testdata.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(82000210));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day11Part2/data.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(613686987427));
        }
    }
}