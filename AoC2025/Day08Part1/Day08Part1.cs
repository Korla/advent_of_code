using Utils;

namespace AoC2025.Day08Part1;

public class Day08Part1
{
    record struct Vector3(int X, int Y, int Z)
    {
        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }

    private long Run(IEnumerable<string> d, int count)
    {
        return d.Select(row => row.Split(","))
            .Select(parts => new Vector3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])))
            .AllPairs()
            .Select(pair => (pair, distance: EuclideanDistance(pair.Item1, pair.Item2)))
            .OrderBy(distance => distance.distance)
            .Take(count)
            .Aggregate(new List<List<Vector3>>(), (circuits, next) =>
            {
                List<Vector3> circuitWithFirst = [];
                List<Vector3> circuitWithSecond = [];
                foreach (var circuit in circuits)
                {
                    foreach (var box in circuit)
                    {
                        if (box == next.pair.Item1)
                        {
                            circuitWithFirst = circuit;
                        }

                        if (box == next.pair.Item2)
                        {
                            circuitWithSecond = circuit;
                        }
                    }
                }

                var newCircuits = circuits
                    .Where(circuit => circuit != circuitWithFirst && circuit != circuitWithSecond)
                    .ToList();
                var joinedCircuit = circuitWithFirst
                    .Concat(circuitWithSecond)
                    .Concat(new List<Vector3> { next.pair.Item1, next.pair.Item2 })
                    .Distinct()
                    .ToList();
                newCircuits.Add(joinedCircuit);
                return newCircuits;
            })
            .OrderByDescending(circuit => circuit.Count)
            .Take(3)
            .Multiply(circuit => circuit.Count);
    }

    private static double EuclideanDistance(Vector3 a, Vector3 b) => 
        Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) +  Math.Pow(a.Z - b.Z, 2));

    private class Day08Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day08Part1/testdata.txt");
            var sut = new Day08Part1();
            Assert.That(sut.Run(data, 10), Is.EqualTo(40));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day08Part1/data.txt");
            var sut = new Day08Part1();
            Assert.That(sut.Run(data, 1000), Is.EqualTo(54600));
        }
    }
}