using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2024.Day11Part2;

public class Day11Part2
{
    private double Run(IEnumerable<string> data)
    {
        var cache = new Dictionary<(string number, int blinkCount), double>();
        var currentNumbers = data.First().Split(" ");
        const int numberOfBlinks = 75;
        return currentNumbers
            .Select(number => BlinkNumber(number, numberOfBlinks)).Sum();

        double BlinkNumber(string number, int blinksRemaining)
        {
            if (blinksRemaining == 0) return 1;
            var cacheKey = (number, blinksRemaining);
            if (cache.TryGetValue(cacheKey, out var stoneCount)) return stoneCount;
            var blinkNumberOnce = BlinkNumberOnce(number);
            cache[cacheKey] = blinkNumberOnce
                .Select(nextNumber => BlinkNumber(nextNumber, blinksRemaining - 1)).Sum();
            return cache[cacheKey];
        }

        List<string> BlinkNumberOnce(string input)
        {
            return input == "0" ? ["1"] :
                input.Length % 2 == 0 ? 
                    Split(input) : 
                    [(double.Parse(input) * 2024).ToString()];
        }

        List<string> Split(string input)
        {
            return
            [
                RemoveLeadingZeroes(input.Substring(0, input.Length / 2)),
                RemoveLeadingZeroes(input.Substring(input.Length / 2))
            ];
        }

        static string RemoveLeadingZeroes(string input)
        {
            var result = Regex.Replace(input, "^0+", "");
            return string.IsNullOrEmpty(result) ? "0" : result;
        }
    }

    private class Day11Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day11Part2/testdata.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(65601038650482));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day11Part2/data.txt");
            var sut = new Day11Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(224869647102559));
        }
    }
}