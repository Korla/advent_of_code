using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day02Part1;

public class Day02Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Count(
                row =>
                {
                    var deltas = row
                            .Split(" ")
                            .Select(int.Parse)
                            .Pairwise((a, b) => b - a);
                    var isIncreasing = deltas.First() > 0;
                    return deltas.All(delta =>
                        isIncreasing ? delta.IsBetweenInclusive(1, 3) : delta.IsBetweenInclusive(-3, -1));
                }   
            );
    }
    
    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day02Part1/testdata.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(2));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day02Part1/data.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(359));
        }
    }
}