using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;
using Range = Utils.Range;

namespace AoC2023.Day05Part2;

public class Day05Part2
{
    private double Run(IEnumerable<string> data)
    {
        data = data.ToList();

        var seedIntegers = data.First().Split(" ").Skip(1)
            .Select(double.Parse)
            .ToList();
        var seeds = new List<Range>();
        for (var i = 0; i < seedIntegers.Count(); i += 2)
        {
            seeds.Add(new Range(seedIntegers[i], seedIntegers[i] + seedIntegers[i + 1]));
        }
        var allMaps = new List<List<(Range sourceRange, double distance)>>();
        List<(Range sourceRange, double distance)>? currentMap = null;
        foreach (var line in data.Skip(1))
        {
            if (string.IsNullOrEmpty(line)) continue;
            if (line.EndsWith("map:"))
            {
                if (currentMap != null)
                {
                    allMaps.Add(currentMap.OrderBy(m => m.sourceRange.start).ToList());
                }

                currentMap = new List<(Range sourceRange, double distance)>();
                continue;
            }

            var bounds = line.Split(" ").Select(double.Parse).ToList();
            currentMap.Add((
                new Range(bounds[1], bounds[1] + bounds[2]),
                bounds[0] - bounds[1]
            ));
        }

        foreach (var map in allMaps)
        {
            foreach (var mapRange in map)
            {
                var processedSeeds = new List<Range>();
                foreach (var seed in seeds)
                {
                    var newSeeds = seed.Split(mapRange.sourceRange)
                        .Select(range => mapRange.sourceRange
                            .Contains(range)
                            ? range.Move(mapRange.distance)
                            : range
                        );
                    processedSeeds.AddRange(newSeeds);
                }
                seeds = processedSeeds.Simplify().ToList();
            }

        }

        var min = seeds.Min(a => a.start);
        return min;
    }

    private class Day05Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day05Part2/testdata.txt");
            var sut = new Day05Part2();
            Assert.That(sut.Run(data), Is.EqualTo(46));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day05Part2/data.txt");
            var sut = new Day05Part2();
            Assert.That(sut.Run(data), Is.EqualTo(0));
        }
    }
}