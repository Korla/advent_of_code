using Utils;

namespace AoC2025.Day09Part1;

public class Day09Part1
{
    private long Run(IEnumerable<string> d)
    {
        return d
            .Select(d => d.Split(',').Select(int.Parse))
            .Select(p => new Vector(p.First(), p.Last()))
            .AllPairs()
            .Select(pair => (long)(Math.Abs(pair.Item1.X - pair.Item2.X) + 1) * (Math.Abs(pair.Item1.Y - pair.Item2.Y) + 1))
            .Max();
    }

    private class Day09Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day09Part1/testdata.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(50));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day09Part1/data.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(4749929916));
        }
    }
}