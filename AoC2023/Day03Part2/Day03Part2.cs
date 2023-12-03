using NUnit.Framework;
using Utils;

namespace AoC2023.Day03Part2;

public class Day03Part2
{
    private int Run(IEnumerable<string> data)
    {
        var lastNumberPos = new Vector(-2, -2);
        var allNumbers = new List<(List<Vector> vectors, string value)>();
        var allGears = new HashSet<Vector>();
        const string gears = "*";
        var deltas = new List<Vector>
        {
            new(-1, -1),
            new(-1, 0),
            new(-1, 1),
            new(0, -1),
            new(0, 1),
            new(1, -1),
            new(1, 0),
            new(1, 1),
        };
        var board = data
            .SelectMany(
                (s, y) => s.Select((value, x) =>
                    (
                        pos: new Vector(x, y),
                        value: value.ToString()
                    )
                )
            );
        foreach (var (pos, value) in board)
        {
            if (int.TryParse(value, out _))
            {
                if (lastNumberPos.Subtract(pos) == new Vector(-1, 0))
                {
                    var number = allNumbers.Last();
                    allNumbers.Remove(number);
                    allNumbers.Add((
                        number.vectors.Concat(new List<Vector> { pos }).ToList(),
                        number.value + value
                    ));
                }
                else
                {
                    allNumbers.Add((new List<Vector> { pos },value));
                }

                lastNumberPos = pos;
            }

            if (gears.Contains(value))
            {
                allGears.Add(pos);
            }
        }

        return allGears
            .Select(pos =>
                allNumbers
                    .Where(n => deltas.Select(d => d.Add(pos)).Intersect(n.vectors).Any())
                    .Select(n => int.Parse(n.value))
            )
            .Where(d => d.Count() == 2)
            .Sum(d => d.Multiply());
    }
    
    private class Day03Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day03Part2/testdata.txt");
            var sut = new Day03Part2();
            Assert.AreEqual(467835, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day03Part2/data.txt");
            var sut = new Day03Part2();
            Assert.AreEqual(91622824, sut.Run(data));
        }
    }
}