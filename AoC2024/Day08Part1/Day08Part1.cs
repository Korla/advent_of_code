using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day08Part1;

public class Day08Part1
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
                var first = groupedAntennas[i];
                for (var j = i + 1; j < groupedAntennas.Count; j++)
                {
                    var second = groupedAntennas[j];
                    antinodes.Add(first.Add(first.Subtract(second)));
                    antinodes.Add(second.Add(second.Subtract(first)));
                }
            }
        }
        return antinodes
            .Count(node => node.X.IsBetweenInclusive(0, xMax - 1) && node.Y.IsBetweenInclusive(0, yMax - 1));
    }

    private class Day08Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day08Part1/testdata.txt");
            var sut = new Day08Part1();
            Assert.That(sut.Run(data), Is.EqualTo(14));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day08Part1/data.txt");
            var sut = new Day08Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(409));
        }
    }
}