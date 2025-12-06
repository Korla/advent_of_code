using Utils;

namespace AoC2025.Day06Part1;

public class Day06Part1
{
    private long Run(IEnumerable<string> data)
    {
        return data
            .Select(row => row.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)))
            .Flip()
            .Sum(problem =>
            {
                var numbers = problem.SkipLast(1).Select(long.Parse);
                return problem.Last() switch
                {
                    "*" => numbers.Multiply(),
                    _ => numbers.Sum()
                };
            });
    }

    private class Day06Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day06Part1/testdata.txt");
            var sut = new Day06Part1();
            Assert.That(sut.Run(data), Is.EqualTo(4277556));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day06Part1/data.txt");
            var sut = new Day06Part1();
            Assert.That(sut.Run(data), Is.EqualTo(6757749566978));
        }
    }
}