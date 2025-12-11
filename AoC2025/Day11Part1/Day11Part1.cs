namespace AoC2025.Day11Part1;

public class Day11Part1
{
    private long Run(IEnumerable<string> d)
    {
        var graph = d.Select(d => d.Replace(":", "").Split(' '))
            .ToDictionary(s => s.First(), s => s.Skip(1));
        var pathsToOut = new List<List<string>>();
        var queue = new Queue<List<string>>([["you"]]);
        while (queue.Count != 0)
        {
            var currentConnection = queue.Dequeue();
            if (currentConnection.Last() == "out")
            {
                pathsToOut.Add(currentConnection);
            }
            else
            {
                foreach (var destination in graph[currentConnection.Last()])
                {
                    queue.Enqueue(currentConnection.Concat([destination]).ToList());
                }
            }
        }
        return pathsToOut.Count;
    }

    private class Day11Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day11Part1/testdata.txt");
            var sut = new Day11Part1();
            Assert.That(sut.Run(data), Is.EqualTo(5));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day11Part1/data.txt");
            var sut = new Day11Part1();
            Assert.That(sut.Run(data), Is.EqualTo(574));
        }
    }
}