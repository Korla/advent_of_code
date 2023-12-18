using NUnit.Framework;
using Utils;

namespace AoC2023.Day17Part2;

public class Day17Part2
{
    private record Location(Vector Position, Vector Direction, int StepCount);

    private int Run(IEnumerable<string> data)
    {
        var listData = data.ToList();
        var grid = new Dictionary<Vector, int>();
        for (var y = 0; y < listData.Count; y++)
        {
            for (var x = 0; x < listData.Count; x++)
            {
                grid.Add(new Vector(x, y), int.Parse("" + listData[y][x]));
            }
        }

        var target = new Vector(listData.First().Length - 1, listData.Count - 1);
        var dijkstra = new Dijkstra<Location>
        {
            neighbors = current =>
            {
                var (position, direction, stepCount) = current;
                var right = ClockWise(direction);
                var left = CounterClockWise(direction);
                var neighbors = new List<Location>();
                if (stepCount < 4)
                {
                    neighbors.Add(new Location(position.Add(direction), direction, stepCount + 1));
                }
                else if (stepCount == 10)
                {
                    neighbors.Add(new Location(position.Add(right), right, 1));
                    neighbors.Add(new Location(position.Add(left), left, 1));
                }
                else
                {
                    neighbors.Add(new Location(position.Add(right), right, 1));
                    neighbors.Add(new Location(position.Add(left), left, 1));
                    neighbors.Add(new Location(position.Add(direction), direction, stepCount + 1));
                }
                return neighbors;
            },
            target = currentNode => currentNode.Position == target && currentNode.StepCount >= 4,
            valid = location => grid.ContainsKey(location.Position),
            weight = location => grid[location.Position]
        };
        return Math.Min(
            dijkstra.Compute(new Location(Vector.Origo, Vector.Right, 0)),
            dijkstra.Compute(new Location(Vector.Origo, Vector.Down, 0))
        );
    }

    private static Vector ClockWise(Vector v)
    {
        return Vector.CardinalDirections[(Vector.CardinalDirections.IndexOf(v) + 1) % 4];
    }

    private static Vector CounterClockWise(Vector v)
    {
        return Vector.CardinalDirections[(Vector.CardinalDirections.IndexOf(v) + 3) % 4];
    }
    
    private class Day17Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day17Part2/testdata.txt");
            var sut = new Day17Part2();
            Assert.AreEqual(94, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day17Part2/data.txt");
            var sut = new Day17Part2();
            Assert.AreEqual(665, sut.Run(data));
        }

        [TestCase]
        public void RotationTests()
        {
            Assert.AreEqual(ClockWise(Vector.Up), Vector.Right);
            Assert.AreEqual(ClockWise(Vector.Right), Vector.Down);
            Assert.AreEqual(ClockWise(Vector.Down), Vector.Left);
            Assert.AreEqual(ClockWise(Vector.Left), Vector.Up);
            Assert.AreEqual(CounterClockWise(Vector.Right), Vector.Up);
            Assert.AreEqual(CounterClockWise(Vector.Down), Vector.Right);
            Assert.AreEqual(CounterClockWise(Vector.Left), Vector.Down);
            Assert.AreEqual(CounterClockWise(Vector.Up), Vector.Left);
        }
    }
}