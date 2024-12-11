using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utils;

namespace AoC2024.Day11Part1;

public class Day11Part1
{
    private int Run(IEnumerable<string> data)
    {
        var currentNumbers = data.First();
        const int numberOfBlinks = 25;
        for (var i = 0; i < numberOfBlinks; i++)
        {
            currentNumbers = string.Join(" ", currentNumbers.Split(" ").Select(BlinkNumber));
        }
        return currentNumbers.Split(" ").Length;

        string BlinkNumber(string input)
        {
            return input == "0" ? "1" : input.Length % 2 == 0 ? Split(input) : (double.Parse(input) * 2024).ToString();
        }

        string Split(string input)
        {
            var first = RemoveLeadingZeroes(input.Substring(0, input.Length / 2));
            var second = RemoveLeadingZeroes(input.Substring(input.Length / 2));
            return $"{first} {second}";
        }

        static string RemoveLeadingZeroes(string input)
        {
            var result = Regex.Replace(input, "^0+", "");
            return string.IsNullOrEmpty(result) ? "0" : result;
        }
    }

    private class Day11Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day11Part1/testdata.txt");
            var sut = new Day11Part1();
            Assert.That(sut.Run(data), Is.EqualTo(55312));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day11Part1/data.txt");
            var sut = new Day11Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(189092));
        }
    }
}