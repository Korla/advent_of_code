using Utils;

namespace AoC2025.Day04Part1;

public class Day04Part1
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

        return paperRolls.Count(paperRoll => Vector.AllNeighbors.Select(paperRoll.Add).Count(paperRolls.Contains) < 4);
    }

    private class Day04Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day04Part1/testdata.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(13));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day04Part1/data.txt");
            var sut = new Day04Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1397));
        }
    }
}