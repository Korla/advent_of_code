using Utils;

namespace AoC2025.Day08Part2;

public class Day08Part2
{
    record struct Vector3(int X, int Y, int Z)
    {
        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }

    private long Run(IEnumerable<string> d)
    {
        var allVectors = d.Select(row => row.Split(","))
            .Select(parts => new Vector3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])))
            .ToList();
        var pairsByDistance = allVectors
            .AllPairs()
            .Select(pair => (pair, distance: EuclideanDistance(pair.Item1, pair.Item2)))
            .OrderBy(distance => distance.distance)
            .Select(p => p.pair)
            .ToList();

        var pairsQueue = new Queue<(Vector3, Vector3)>(pairsByDistance);
        var circuits = new List<List<Vector3>>();
        (Vector3, Vector3) nextPair = (default, default);
        while (circuits.FirstOrDefault()?.Count != allVectors.Count)
        {
            nextPair = pairsQueue.Dequeue();
            List<Vector3> circuitWithFirst = [];
            List<Vector3> circuitWithSecond = [];
            foreach (var circuit in circuits)
            {
                foreach (var box in circuit)
                {
                    if (box == nextPair.Item1)
                    {
                        circuitWithFirst = circuit;
                    }

                    if (box == nextPair.Item2)
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
                .Concat(new List<Vector3> { nextPair.Item1, nextPair.Item2 })
                .Distinct()
                .ToList();
            newCircuits.Add(joinedCircuit);
            circuits = newCircuits;
        }

        return nextPair.Item1.X * nextPair.Item2.X;
    }

    private static double EuclideanDistance(Vector3 a, Vector3 b) => 
        Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) +  Math.Pow(a.Z - b.Z, 2));

    private class Day08Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day08Part2/testdata.txt");
            var sut = new Day08Part2();
            Assert.That(sut.Run(data), Is.EqualTo(25272));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day08Part2/data.txt");
            var sut = new Day08Part2();
            Assert.That(sut.Run(data), Is.EqualTo(107256172));
        }
    }
}