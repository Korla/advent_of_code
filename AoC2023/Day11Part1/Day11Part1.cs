using NUnit.Framework;
using Utils;

namespace AoC2023.Day11Part1;

public class Day11Part1
{
    private int Run(IEnumerable<string> data)
    {
        var space = Expand(data.ToList());
        return GetAllPairs(space.ToList())
            .Sum(vectors => vectors.Item1.ManhattanDistance(vectors.Item2));
    }

    private IEnumerable<Vector> Expand(IReadOnlyList<string> data)
    {
        var galaxies = new List<Vector>();
        var xToExpand = new List<int>();
        var yToExpand = new List<int>();
        for (var i = 0; i < data.Count; i++)
        {
            var noXGalaxy = true;
            var noYGalaxy = true;
            for (var j = 0; j < data.Count; j++)
            {
                if (data[i][j] == '#')
                {
                    galaxies.Add(new Vector(i, j));
                    noXGalaxy = false;
                }

                if (data[j][i] == '#')
                {
                    noYGalaxy = false;
                }
            }
            if(noXGalaxy) xToExpand.Add(i);
            if(noYGalaxy) yToExpand.Add(i);
        }

        return galaxies.Select(g => new Vector(
            g.X + xToExpand.Count(x => x <= g.X),
            g.Y + yToExpand.Count(y => y <= g.Y)
        ));
    }

    private IEnumerable<(Vector, Vector)> GetAllPairs(List<Vector> vectors)
    {
        for (var i = 0; i < vectors.Count; i++)
        {
            for (var j = i; j < vectors.Count; j++)
            {
                yield return (vectors[i], vectors[j]);
            }
        }
    }

    private class Day11Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day11Part1/testdata.txt");
            var sut = new Day11Part1();
            Assert.AreEqual(374, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day11Part1/data.txt");
            var sut = new Day11Part1();
            Assert.AreEqual(9214785, sut.Run(data));
        }
    }
}