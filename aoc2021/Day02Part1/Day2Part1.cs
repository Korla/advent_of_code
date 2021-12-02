using NUnit.Framework;

namespace aoc2021.Day02Part1;

public class Day02Part1
{
    private int Run(IEnumerable<string> data)
    {
        var values = data
            .Select(s => s.Split(" "));
        var (hor, vert) = values
            .Select(parts => (parts[0][0], int.Parse(parts[1])))
            .Aggregate(
                (0,0),
                (prev, curr) =>
                {
                    switch (curr.Item1)
                    {
                        case 'f': return (prev.Item1 + curr.Item2, prev.Item2);
                        case 'd': return (prev.Item1, prev.Item2 + curr.Item2);
                        case 'u': return (prev.Item1, prev.Item2 - curr.Item2);
                    }

                    return prev;
                }
            );
        return hor * vert;
    }
    
    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part1/testdata.txt");
            var sut = new Day02Part1();
            Assert.AreEqual(150, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part1/data.txt");
            var sut = new Day02Part1();
            Assert.AreEqual(1690020, sut.Run(data));
        }
    }
}