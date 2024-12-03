using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utils;

namespace AoC2024.Day03Part1;

public class Day03Part1
{
    private int Run(IEnumerable<string> data)
    {
        return Regex.Matches(string.Join("", data), @"mul\((\d+),(\d+)\)")
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
    }

    private class Day03Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day03Part1/testdata.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(161));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day03Part1/data.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(156388521));
        }
    }
}