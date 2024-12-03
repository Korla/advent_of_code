using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day14Part2;

public class Day14Part2
{
    private int Run(IEnumerable<string> data)
    {
        var cache = new Dictionary<string, int>();
        var listData = data.ToList();
        var cycle = 0;
        var jumpedAhead = false;
        while (cycle != 1000000000)
        {
            listData = RunOneCycle(listData);
            cycle++;
            if (jumpedAhead)
                continue;
            if (cache.TryGetValue(string.Join("", listData), out var lastCycle))
            {
                var cycleLength = cycle - lastCycle;
                var newCycle = 1000000000 - cycleLength - 1000000000 % cycleLength + cycle % cycleLength;
                cycle = newCycle;
                jumpedAhead = true;
            }
            else
            {
                cache.Add(string.Join("", listData), cycle);
            }
        }
        return GetWeights(listData);
    }

    private List<string> RunOneCycle(List<string> data)
    {
        return data
            .Flip90()
            .GetNextPosition()
            .Flip270()
            .GetNextPosition()
            .Flip270()
            .GetNextPosition()
            .Flip270()
            .GetNextPosition()
            .Flip90()
            .Flip90();
    }

    private int GetWeights(IEnumerable<string> data)
    {
        return data.Select((row, i) => row.Count(c => c == 'O') * (data.Count() - i)).Sum();
    }

    private class Day14Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day14Part2/testdata.txt");
            var sut = new Day14Part2();
            Assert.That(sut.Run(data), Is.EqualTo(64));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day14Part2/data.txt");
            var sut = new Day14Part2();
            Assert.That(sut.Run(data), Is.EqualTo(105606));
        }
    }
}

public static class Extensions
{
    public static List<string> GetNextPosition(this IEnumerable<string> flipped)
    {
        return flipped
            .Select(row =>
            {
                var newElements = new Dictionary<int, char>();
                var lastRockPosition = -1;
                for (var i = 0; i < row.Length; i++)
                {
                    if (row[i] == '#')
                    {
                        lastRockPosition = i;
                        newElements.Add(i, row[i]);
                    }

                    if (row[i] == 'O')
                    {
                        lastRockPosition++;
                        newElements.Add(lastRockPosition, row[i]);
                    }
                }

                return string.Join(
                    "",
                    Enumerable.Range(0, row.Length)
                        .Select(i => newElements.TryGetValue(i, out var newValue) ? newValue : '.')
                );
            })
            .ToList();
    }

    public static List<string> Flip90(this List<string> data)
    {
        var flipped = Enumerable.Range(0, data[0].Length).Select(_ => "").ToList();
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[0].Length; x++)
            {
                flipped[data[0].Length - x - 1] += data[y][x];
            }
        }

        return flipped;
    }

    public static List<string> Flip270(this List<string> data)
    {
        var flipped = Enumerable.Range(0, data[0].Length).Select(_ => "").ToList();
        for (var y = data.Count - 1; y >= 0; y--)
        {
            for (var x = data[0].Length - 1; x >= 0; x--)
            {
                flipped[x] += data[y][x];
            }
        }

        return flipped;
    }
}