using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day07Part1;

public class Day07Part1
{
    private int Run(IEnumerable<string> data)
    {
        const string order = "23456789TJQKA";
        return data
            .Select(line => line.Split(" "))
            .Select(parts =>
            {
                var hand = parts.First();
                var handValues = string.Join("", hand.Select(card => order.IndexOf(card).ToString().PadLeft(2)));
                var bid = int.Parse(parts.Last());
                var value = string.Join(
                    "",
                    hand.ToList()
                        .GroupBy(a => a)
                        .Select(a => a.Count())
                        .OrderByDescending(a => a)
                );
                return (hand, handValues, bid, value);
            })
            .OrderBy(a => a.value)
            .ThenBy(a => a.handValues)
            .Select((a, i) => a.bid * (i + 1))
            .Sum();
    }

    private class Day07Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day07Part1/testdata.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(6440));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day07Part1/data.txt");
            var sut = new Day07Part1();
            Assert.That(sut.Run(data), Is.EqualTo(248453531));
        }
    }
}