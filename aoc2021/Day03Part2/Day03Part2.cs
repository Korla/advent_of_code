using NUnit.Framework;

namespace aoc2021.Day03Part2;

public class Day03Part2
{
    private int Run(IEnumerable<string> data)
    {
        data = data.ToList();
        var s = data
            .Select(x => Convert.ToInt32(x, 2))
            .ToList();
        return Recurse(s, data.First().Length - 1, true) * Recurse(s, data.First().Length - 1, false);
    }

    private int Recurse(IReadOnlyCollection<int> values, int pos, bool mode)
    {
        var desiredValue = values.Count(b => (b & (1 << pos)) != 0) * 2 >= values.Count ? 1 : 0;
        if (!mode) desiredValue = 1 - desiredValue;
        var newValues = values.Where(b => ((b >> pos) & 1) == desiredValue).ToList();
        return newValues.Count == 1 ? newValues.First() : Recurse(newValues, pos - 1, mode);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day03Part2/testdata.txt");
            var sut = new Day03Part2();
            Assert.AreEqual(230, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day03Part2/data.txt");
            var sut = new Day03Part2();
            Assert.AreEqual(6822109, sut.Run(data));
        }
    }
}