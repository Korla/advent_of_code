using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2022.Day01Part1;

public class Day01Part1
{
    private int Run(IEnumerable<string> data)
    {
        var values = data
            .ToList();
        var elves = new List<int>();
        var current = 0;
        foreach (var value in values)
        {
            if (string.IsNullOrEmpty(value))
            {
                elves.Add(current);
                current = 0;
            }
            else
            {
                current += int.Parse(value);
            }
        }

        return elves.Max();
    }
    
    private class Day01Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day01Part1/testdata.txt");
            var sut = new Day01Part1();
            Assert.AreEqual(24000, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day01Part1/data.txt");
            var sut = new Day01Part1();
            Assert.AreEqual(69289, sut.Run(data));
        }
    }
}