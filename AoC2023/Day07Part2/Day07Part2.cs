using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day07Part2;

public class Day07Part2
{
    // 50000: five-of-a-kind
    // 41000: four-of-a-kind
    // 32000: full-house
    // 31100: three-of-a-kind
    // 22100: two-pairs
    // 21110: one-pair
    // 11111: high-card

    // 5J -> 50000
    // 4J -> 50000
    // 3J
    //  1 1 -> 41000
    //  2 0 -> 50000
    // 2J
    //  1 1 1 -> 31100
    //  2 1 0 -> 41000
    //  3 0 0 -> 50000
    // 1J
    //  1 1 1 1 -> 21110
    //  2 1 1 0 -> 31100
    //  2 2 0 0 -> 32000
    //  3 1 0 0 -> 41000
    //  4 0 0 0 -> 50000
    private int Run(IEnumerable<string> data)
    {
        const string order = "J23456789TQKA";
        return data
            .Select(line => line.Split(" "))
            .Select(parts =>
            {
                var hand = parts.First();
                var handValues = string.Join("", hand.Select(card => order.IndexOf(card).ToString().PadLeft(2, '0')));
                var bid = int.Parse(parts.Last());

                var handCountByCard = new Dictionary<char, int>();
                foreach (var c in hand)
                {
                    handCountByCard.IncreaseOrAdd(c);
                }

                handCountByCard.Remove('J', out var jokerCount);
                var value = jokerCount == 5 ? "50000" : string.Join(
                    "",
                    handCountByCard
                        .Select(c => (c: c.Key, count: c.Value))
                        .OrderByDescending(c => c.count)
                        .Select((a, i) => i == 0 ? a.count + jokerCount : a.count)
                        .OrderByDescending(a => a)
                ).PadRight(5, '0');

                return (hand, handValues, bid, value);
            })
            .OrderBy(a => a.value)
            .ThenBy(a => a.handValues)
            .Select((a, i) => a.bid * (i + 1))
            .Sum();
    }

    private class Day07Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day07Part2/testdata.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(5905));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day07Part2/data.txt");
            var sut = new Day07Part2();
            Assert.That(sut.Run(data), Is.EqualTo(248781813));
        }
    }
}