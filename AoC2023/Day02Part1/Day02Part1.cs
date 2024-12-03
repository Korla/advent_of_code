using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day02Part1;

public class Day02Part1
{
    private int Run(IEnumerable<string> data)
    {
        var maximums = new Dictionary<string, int>
        {
            {"red", 12},
            {"green", 13},
            {"blue", 14}
        };
        return data.Select(gameRow =>
            {
                var p1 = gameRow.Split(": ");
                var gameIndex = int.Parse(p1.First().Split(" ").Last());
                var gameHands = new Dictionary<string, int>
                {
                    {"red", 0},
                    {"green", 0},
                    {"blue", 0}
                };
                var hands = p1.Last().Split("; ")
                    .SelectMany(p2 => p2.Split(", "));
                foreach (var hand in hands)
                {
                    var handParts = hand.Split(" ");
                    var count = int.Parse(handParts.First());
                    var color = handParts.Last();
                    gameHands[color] = Math.Max(gameHands[color], count);
                }

                return (gameIndex, gameHands);
            })
            .Where(game => game.gameHands.All(h => maximums[h.Key] >= h.Value))
            .Sum(game => game.gameIndex);
    }

    private class Day02Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part1/testdata.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(8));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part1/data.txt");
            var sut = new Day02Part1();
            Assert.That(sut.Run(data), Is.EqualTo(2685));
        }
    }
}