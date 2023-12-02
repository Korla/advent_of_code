using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day02Part2;

public class Day02Part2
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
            .Sum(game => game.gameHands.Values.Multiply());
    }
    
    private class Day02Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day02Part2/testdata.txt");
            var sut = new Day02Part2();
            Assert.AreEqual(2286, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day02Part2/data.txt");
            var sut = new Day02Part2();
            Assert.AreEqual(83707, sut.Run(data));
        }
    }
}