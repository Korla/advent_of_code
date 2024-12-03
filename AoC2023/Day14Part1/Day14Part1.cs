using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day14Part1;

public class Day14Part1
{
    private int Run(IEnumerable<string> data)
    {
        return new List<string> { string.Join("", Enumerable.Range(0, data.First().Length).Select(_ => "#")) }
            .Concat(data)
            .Flip()
            .Sum(row =>
            {
                var weight = 0;
                var lastRockPosition = 0;
                for (var i = 1; i < row.Length; i++)
                {
                    if (row[i] == '#')
                    {
                        lastRockPosition = i;
                    }

                    if (row[i] == 'O')
                    {
                        lastRockPosition++;
                        weight += row.Length - lastRockPosition;
                    }
                }

                return weight;
            });
    }

    private class Day14Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day14Part1/testdata.txt");
            var sut = new Day14Part1();
            Assert.That(sut.Run(data), Is.EqualTo(136));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day14Part1/data.txt");
            var sut = new Day14Part1();
            Assert.That(sut.Run(data), Is.EqualTo(112048));
        }
    }
}