namespace AoC2025.Day01Part1;

public class Day01Part1
{
    class State
    {
        public int Count = 0;
        private int Current = 50;

        public State Add(int delta)
        {
            Current = (Current + delta + 100) % 100;
            if (Current == 0)
            {
                Count++;
            }

            return this;
        }
    }

    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(r => r.First() == 'L' ? -int.Parse(r[1..]) : int.Parse(r[1..]))
            .Aggregate(new State(), (state, delta) => state.Add(delta)).Count;
    }

    private class Day01Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day01Part1/testdata.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(3));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day01Part1/data.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1097));
        }
    }
}