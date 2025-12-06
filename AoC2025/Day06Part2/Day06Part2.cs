using Utils;

namespace AoC2025.Day06Part2;

public class Day06Part2
{
    record Aggregate(long result, List<long> numbers);

    private long Run(IEnumerable<string> data)
    {
        var problems = data.RotateClockWise().RotateClockWise().RotateClockWise();
        var sumChars = new[] { '+', '*' };
        return problems
            .Where(row => !string.IsNullOrWhiteSpace(row))
            .Aggregate(new Aggregate(0, []), (aggregate, row) =>
            {
                if (!sumChars.Contains(row.Last()))
                {
                    aggregate.numbers.Add(long.Parse(row));
                    return aggregate;
                }

                aggregate.numbers.Add(long.Parse(row.TrimEnd(sumChars)));
                return new Aggregate(
                    aggregate.result + row.Last() switch
                    {
                        '*' => aggregate.numbers.Multiply(),
                        _ => aggregate.numbers.Sum()
                    },
                    []);
            })
            .result;
    }

    private class Day06Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day06Part2/testdata.txt");
            var sut = new Day06Part2();
            Assert.That(sut.Run(data), Is.EqualTo(3263827));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day06Part2/data.txt");
            var sut = new Day06Part2();
            Assert.That(sut.Run(data), Is.EqualTo(10603075273949));
        }
    }
}