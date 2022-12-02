using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2022.Day02Part1;

public class Day02Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(r => r.ToCharArray())
            .Sum(v => v.Last() - 'W' + (v.Last() - v.First() + 2) % 3 * 3);
    }
    
    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part1/testdata.txt");
            var sut = new Day02Part1();
            Assert.AreEqual(15, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part1/data.txt");
            var sut = new Day02Part1();
            Assert.AreEqual(11475, sut.Run(data));
        }
    }
}