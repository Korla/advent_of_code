namespace AoC2025.Day07Part2;

public class Day07Part2
{
    private long Run(IEnumerable<string> d)
    {
        var manifold = d.ToList();
        var cache = new Dictionary<(int row, int beam), long>();
        return RecSplitBeams(0, manifold.First().IndexOf('S'));

        long RecSplitBeams(int rowIndex, int beam)
        {
            rowIndex += 1;
            if (cache.TryGetValue((rowIndex, beam), out var cachedValue))
            {
                return cachedValue;
            }

            if (rowIndex == manifold.Count)
            {
                return 1;
            }

            var row = manifold[rowIndex];
            var result = row[beam] == '^'
                ? RecSplitBeams(rowIndex, beam - 1) +
                  RecSplitBeams(rowIndex, beam + 1)
                : RecSplitBeams(rowIndex, beam);
            cache.Add((rowIndex, beam), result);
            return result;
        }
    }

    private class Day07Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day07Part2/testdata.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(40));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day07Part2/data.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(47857642990160));
        }
    }
}