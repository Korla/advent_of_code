using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day06Part1;

public class Day06Part1
{
    private int Run(IEnumerable<string> data)
    {
        const int windowSize = 4;
        return data
            .First()
            .SlidingWindowValues(windowSize)
            .Select((w, i) => (w, i))
            .First(a => a.w.Distinct().Count() == a.w.Count).i + windowSize;
    }
    
    private class Day06Part1Tests
    {
        [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
        [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        public void TestData(string input, int expected)
        {
            var sut = new Day06Part1();
            Assert.AreEqual(expected, sut.Run(new[] {input}));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day06Part1/data.txt");
            var sut = new Day06Part1();
            Assert.AreEqual(1640, sut.Run(data));
        }
    }
}