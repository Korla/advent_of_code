namespace AoC2025.Day03Part1;

public class Day03Part1
{
    private const int Total = 2;

    private record State(int Value = 0, int Index = 0);

    private long Run(IEnumerable<string> data)
    {
        return data.Select(row => row.Select((c, index) => new State(int.Parse(c.ToString()), index + 1)).ToList())
            .Select(integers =>
            {
                var list = Enumerable.Range(0, Total).Aggregate(new List<State> { new() }, (list, digitIndex) =>
                    {
                        var previous = list.Last();
                        var next = integers
                            .Skip(previous.Index)
                            .Take(integers.Count - (Total - digitIndex) - previous.Index + 1)
                            .Aggregate((state, integer) => integer.Value > state.Value ? integer : state);
                        list.Add(next);
                        return list;
                    })
                    .Select(state => state.Value);
                return long.Parse(string.Join("", list));
            })
            .Sum();
    }

    private class Day03Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day03Part1/testdata.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(357));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day03Part1/data.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(17359));
        }
    }
}