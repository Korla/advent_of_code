using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day08Part2;

public class Day08Part2
{
    private int Run(IEnumerable<string> data)
    {
        var dataList = data.ToList();
        var antennas = new Dictionary<char, List<Vector>>();
        var yMax = dataList.Count;
        var xMax = dataList.First().Length;
        for (var y = 0; y < yMax; y++)
        {
            for (var x = 0; x < xMax; x++)
            {
                var type = dataList[y][x];
                if (dataList[y][x] != '.')
                {
                    antennas.AddToListOrAdd(type, new Vector(x, y));
                }
            }
        }

        var antinodes = new HashSet<Vector>();
        foreach (var groupedAntennas in antennas.Values)
        {
            for (var i = 0; i < groupedAntennas.Count - 1; i++)
            {
                for (var j = i + 1; j < groupedAntennas.Count; j++)
                {
                    AddAntinodes(groupedAntennas[i], groupedAntennas[j]);
                    AddAntinodes(groupedAntennas[j], groupedAntennas[i]);
                }
            }
        }

        return antinodes.Count;

        void AddAntinodes(Vector first, Vector second)
        {
            var distance = first.Subtract(second);
            var current = first;
            while (current.X.IsBetweenInclusive(0, xMax - 1) && current.Y.IsBetweenInclusive(0, yMax - 1))
            {
                antinodes.Add(current);
                current = current.Add(distance);
            }
        }
    }

    private class Day08Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day08Part2/testdata.txt");
            var sut = new Day08Part2();
            Assert.That(sut.Run(data), Is.EqualTo(34));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day08Part2/data.txt");
            var sut = new Day08Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(0));
        }
    }
}