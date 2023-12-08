using System.Text.RegularExpressions;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day08Part1;

public class Day08Part1
{
    private int Run(IEnumerable<string> data)
    {
        // AAA = (BBB, CCC)
        var regex = new Regex(@"\b[A-Za-z]{3}\b");
        var instructions = data.First();
        var grid = data.Skip(2)
            .Select(r => regex.Matches(r))
            .Select(matches => (node: matches[0].Value, left: matches[1].Value, right: matches[2].Value))
            .ToDictionary(a => a.node, a => (a.left, a.right));

        var location = "AAA";
        var currentInstruction = 0;
        while (true)
        {
            var instruction = instructions[currentInstruction % instructions.Length] == 'L';
            location = instruction ? grid[location].left : grid[location].right;
            if (location == "ZZZ")
            {
                return currentInstruction + 1;
            }
            currentInstruction++;
        }
    }

    private class Day08Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day08Part1/testdata.txt");
            var sut = new Day08Part1();
            Assert.AreEqual(6, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day08Part1/data.txt");
            var sut = new Day08Part1();
            Assert.AreEqual(24253, sut.Run(data));
        }
    }
}