using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2022.Day02Part2;

public class Day02Part2
{
    private int Run(IEnumerable<string> data) =>
        data
            .Sum(v =>
                (v.First() + v.Last() - 'X') % 3 + 1 +
                (v.Last() - 1) % 3 * 3
            );

    private class Day02Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part2/testdata.txt");
            var sut = new Day02Part2();
            Assert.AreEqual(12, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part2/data.txt");
            var sut = new Day02Part2();
            Assert.AreEqual(16862, sut.Run(data));
        }
    }
}