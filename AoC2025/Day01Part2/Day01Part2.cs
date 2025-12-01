namespace AoC2025.Day01Part2;

public class Day01Part2
{
    record State(int Count = 0, int Current = 50)
    {
        public State Add(int delta)
        {
            var revolutions = Math.Abs(delta) / 100;
            var actualDelta = delta % 100;
            var nextPosition = Current + actualDelta;
            var nextActual = (nextPosition % 100 + 100) % 100;
            if (Current != 0 && (nextPosition is < 0 or > 99 || nextActual == 0))
            {
                revolutions++;
            }

            return new State(Count + revolutions, nextActual);
        }
    }

    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(r => r.First() == 'L' ? int.Parse(r[1..]) : -int.Parse(r[1..]))
            .Aggregate(new State(), (state, delta) => state.Add(delta)).Count;
    }

    private class Day01Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day01Part2/testdata.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(6));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day01Part2/data.txt");
            var sut = new Day01Part2();
            Assert.That(sut.Run(data), Is.EqualTo(7101));
        }

        [TestCase(50, 251, 3)]
        [TestCase(50, 250, 3)]
        [TestCase(50, 249, 2)]
        [TestCase(50, -251, 3)]
        [TestCase(50, -250, 3)]
        [TestCase(50, -249, 2)]
        [TestCase(50, 51, 1)]
        [TestCase(50, 50, 1)]
        [TestCase(50, 49, 0)]
        [TestCase(50, -51, 1)]
        [TestCase(50, -50, 1)]
        [TestCase(50, -49, 0)]
        [TestCase(50, 25, 0)]
        [TestCase(50, -25, 0)]
        [TestCase(0, 10, 0)]
        [TestCase(0, -10, 0)]
        [TestCase(0, 201, 2)]
        [TestCase(0, 200, 2)]
        [TestCase(0, 199, 1)]
        [TestCase(0, -201, 2)]
        [TestCase(0, -200, 2)]
        [TestCase(0, -199, 1)]
        [TestCase(50, -68, 1)]
        [TestCase(82, -30, 0)]
        [TestCase(52, 48, 1)]
        public void StateTests(int start, int delta, int expected)
        {
            Assert.That(new State(0, start).Add(delta).Count, Is.EqualTo(expected));
        }
    }
}