using Utils;

namespace AoC2025.Day04Part2;

public class Day04Part2
{
    private long Run(IEnumerable<string> data)
    {
        var grid = data.ToList();
        var paperRolls = new HashSet<Vector>();
        for (var y = 0; y < grid.Count; y++)
        {
            for (var x = 0; x < grid.Count; x++)
            {
                if (grid[y][x] == '@')
                {
                    paperRolls.Add(new Vector(x, y));
                }
            }
        }

        var originalCount = paperRolls.Count;
        var count = originalCount;
        int lastCount;
        do
        {
            lastCount = count;
            paperRolls = paperRolls.Where(paperRoll => Vector.AllNeighbors.Select(paperRoll.Add).Count(paperRolls.Contains) >= 4).ToHashSet();
        } while ((count = paperRolls.Count) != lastCount);

        return originalCount - count;
    }

    private class Day04Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day04Part2/testdata.txt");
            var sut = new Day04Part2();
            Assert.That(sut.Run(data), Is.EqualTo(43));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day04Part2/data.txt");
            var sut = new Day04Part2();
            Assert.That(sut.Run(data), Is.EqualTo(8758));
        }
    }
}