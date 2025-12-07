namespace AoC2025.Day07Part1;

public class Day07Part1
{
    record Aggregate(int Splits, HashSet<int> Beams);

    private long Run(IEnumerable<string> d)
    {
        var data = d.ToList();
        var start = new Aggregate(0, [data.First().IndexOf('S')]);
        return data.Skip(1).Aggregate(start, (aggregate, row) =>
        {
            var newBeams = new HashSet<int>();
            foreach (var beam in aggregate.Beams)
            {
                if (row[beam] == '^')
                {
                    newBeams.Add(beam - 1);
                    newBeams.Add(beam + 1);
                    aggregate = aggregate with { Splits = aggregate.Splits + 1 };
                }
                else
                {
                    newBeams.Add(beam);
                }
            }
            return aggregate with
            {
                Beams = newBeams
            };
        }).Splits;
    }

    private class Day07Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day07Part1/testdata.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(21));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day07Part1/data.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1630));
        }
    }
}