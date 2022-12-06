using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2022.Day06Part2;

public class Day06Part2
{
    private int Run(IEnumerable<string> data)
    {
        const int windowSize = 14;
        return data
            .First()
            .SlidingWindowValues(windowSize)
            .Select((w, i) => (w, i))
            .First(a => a.w.Distinct().Count() == a.w.Count).i + windowSize;
    }
    
    private class Day06Part2Tests
    {
        [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
        [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
        [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 23)]
        [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
        [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
        public void TestData(string input, int expected)
        {
            var sut = new Day06Part2();
            Assert.AreEqual(expected, sut.Run(new[] {input}));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day06Part2/data.txt");
            var sut = new Day06Part2();
            Assert.AreEqual(3613, sut.Run(data));
        }
    }
}