using NUnit.Framework;

namespace aoc2021.Day22Part2;

public class Day22Part2
{
    private record struct Box(long X0, long X1, long Y0, long Y1, long Z0, long Z1);

    private long Run(IEnumerable<string> data)
    {
        var cubes = new Dictionary<Box, long>();
        foreach (var row in data)
        {
            var (newBox, nSign) = ParseRow(row);
            GetIntersectingCubes(cubes, newBox).ToList().ForEach(x =>
                cubes[x.Key] = cubes.TryGetValue(x.Key, out var e) ? e + x.Value : x.Value
            );
            if (nSign > 0)
            {
                cubes[newBox] = cubes.TryGetValue(newBox, out var e) ? e + nSign : nSign;
            }
        }

        // Calculate total
        return cubes.Sum(c =>
        {
            var ((x0, x1, y0, y1, z0, z1), sign) = c;
            return (x1 - x0 + 1) * (y1 - y0 + 1) * (z1 - z0 + 1) * sign;
        });
    }

    private static (Box, long) ParseRow(string row)
    {
        // Parse input
        var parts = row.Split(',');
        var nxBounds = parts[0].Split('=')[1].Split("..").Select(long.Parse).ToList();
        var nyBounds = parts[1].Split('=')[1].Split("..").Select(long.Parse).ToList();
        var nzBounds = parts[2].Split('=')[1].Split("..").Select(long.Parse).ToList();
        long nSign = row.Split(' ')[0] == "on" ? 1 : 0;
        return (new Box(nxBounds[0], nxBounds[1], nyBounds[0], nyBounds[1], nzBounds[0], nzBounds[1]), nSign);
    }

    private static Dictionary<Box, long> GetIntersectingCubes(Dictionary<Box, long> cubes, Box box)
    {
        var (nx0, nx1, ny0, ny1, nz0, nz1) = box;
        var newCubes = new Dictionary<Box, long>();
        foreach (var ((ex0, ex1, ey0, ey1, ez0, ez1), eSign) in cubes)
        {
            var ix0 = Math.Max(nx0, ex0);
            var ix1 = Math.Min(nx1, ex1);
            var iy0 = Math.Max(ny0, ey0);
            var iy1 = Math.Min(ny1, ey1);
            var iz0 = Math.Max(nz0, ez0);
            var iz1 = Math.Min(nz1, ez1);
            if (ix0 <= ix1 && iy0 <= iy1 && iz0 <= iz1)
            {
                newCubes[new Box(ix0, ix1, iy0, iy1, iz0, iz1)] =
                    newCubes.TryGetValue(new Box(ix0, ix1, iy0, iy1, iz0, iz1), out var e) ? e - eSign : -eSign;
            }
        }

        return newCubes;
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day22Part2/testdata.txt");
            var sut = new Day22Part2();
            Assert.AreEqual(2758514936282235, sut.Run(data));
        }
        
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day22Part2/data.txt");
            var sut = new Day22Part2();
            Assert.AreEqual(1263804707062415, sut.Run(data));
        }
    }
}