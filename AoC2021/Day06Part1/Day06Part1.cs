using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day06Part1;

public class Day06Part1
{
    private int Run(IEnumerable<string> data, int nbrOfGenerations)
    {
        return Enumerable.Range(0, nbrOfGenerations)
            .Aggregate(
                data.First().Split(",").Select(f => new Fish { Value = int.Parse(f) }).ToList(),
                (school, _) =>
                {
                    var newFishCount = 0;
                    foreach (var fish in school)
                    {
                        fish.Value--;
                        if (fish.Value == -1)
                        {
                            fish.Value = 6;
                            newFishCount++;
                        }
                    }

                    school.AddRange(Enumerable.Range(0, newFishCount)
                        .Select(_ => new Fish { Value = 8 }));
                    return school;
                }
            ).Count;
    }

    private class Fish
    {
        public int Value { get; set; }
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day06Part1/testdata.txt");
            var sut = new Day06Part1();
            Assert.That(sut.Run(data, 80), Is.EqualTo(5934));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day06Part1/data.txt");
            var sut = new Day06Part1();
            Assert.That(sut.Run(data, 80), Is.EqualTo(373378));
        }
    }
}