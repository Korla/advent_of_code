using NUnit.Framework;
using Utils;

namespace AoC2023.Day05Part1;

public class Day05Part1
{
    private double Run(IEnumerable<string> data)
    {
        data = data.ToList();

        var seeds = data.First().Split(" ").Skip(1).Select(double.Parse);
        var allMaps = new List<List<(double target, double source, double count)>>();
        foreach (var line in data.Skip(1))
        {
            if (string.IsNullOrEmpty(line)) continue;
            if (line.EndsWith("map:"))
            {
                allMaps.Add(new List<(double target, double source, double count)>());
                continue;
            }

            var bounds = line.Split(" ").Select(double.Parse).ToList();
            allMaps.Last().Add((bounds[0], bounds[1], bounds[2]));
        }

        return seeds.Min(seed =>
            allMaps.Aggregate(seed, (seed, map) =>
            {
                foreach (var (target, source, count) in map)
                {
                    if(seed >= source && seed < source + count)
                    {
                        return seed + target - source;
                    }
                }

                return seed;
            }));
    }

    private class Day05Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day05Part1/testdata.txt");
            var sut = new Day05Part1();
            Assert.AreEqual(35, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day05Part1/data.txt");
            var sut = new Day05Part1();
            Assert.AreEqual(322500873, sut.Run(data));
        }
    }
}