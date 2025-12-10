namespace AoC2025.Day10Part1;

public class Day10Part1
{
    private long Run(IEnumerable<string> d)
    {
        return d.Sum(row =>
        {
            var parts = row.Split(' ');
            var target = parts.First().Skip(1).SkipLast(1)
                .Select(light => light == '#').ToList();
            var buttons = parts.Skip(1).SkipLast(1)
                .Select(b => b.Substring(1, b.Length - 2).Split(',').Select(int.Parse).ToList()).ToList();
            return FindShortest(target, buttons);
        });
    }

    private record State(List<bool> Lights, List<List<int>> Buttons);

    private static int FindShortest(List<bool> target, List<List<int>> buttons)
    {
        var states = new List<State> { new(target, buttons) };
        for (var i = 0; i <= buttons.Count; i++)
        {
            var nextStates = new List<State>();
            foreach (var state in states)
            {
                foreach (var button in state.Buttons.Where(button => button.Any(position => state.Lights[position])))
                {
                    var newLights = state.Lights.Select((b, pos) => button.Contains(pos) ? !b : b).ToList();
                    if (newLights.All(bit => !bit)) return i + 1;
                    nextStates.Add(new State(newLights, buttons.Where(b => b != button).ToList()));
                }
            }
            states = nextStates;
        }

        return int.MaxValue;
    }

    private class Day10Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day10Part1/testdata.txt");
            var sut = new Day10Part1();
            Assert.That(sut.Run(data), Is.EqualTo(7));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day10Part1/data.txt");
            var sut = new Day10Part1();
            Assert.That(sut.Run(data), Is.EqualTo(514));
        }
    }
}