using System.Text.RegularExpressions;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day08Part2;

public class Day08Part2
{
    private double Run(IEnumerable<string> data)
    {
        var regex = new Regex(@"\b(?:\w\w\w)\b");
        var instructions = data.First();
        var locations = data.Skip(2)
            .Select(r => regex.Matches(r))
            .Select(matches => (node: matches[0].Value, left: matches[1].Value, right: matches[2].Value))
            .ToList();
        var initialLocations = locations.Select(l => l.node).Where(n => n.EndsWith("Z")).ToArray();
        var grid = locations.ToDictionary(a => a.node, a => (a.left, a.right));

        return initialLocations
            .Select(GetCycleLength)
            .Aggregate(MathHelpers.LowestCommonMultiple);

        double GetCycleLength(string location)
        {
            var index = 0;
            while (true)
            {
                var currentInstruction = index % instructions.Length;
                var instruction = instructions[currentInstruction] == 'L';
                location = instruction ? grid[location].left : grid[location].right;
                if (location.EndsWith("Z"))
                {
                    return index + 1;
                }
    
                index++;
            }
        }
    }

    private class Day08Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day08Part2/testdata.txt");
            var sut = new Day08Part2();
            Assert.AreEqual(6, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day08Part2/data.txt");
            var sut = new Day08Part2();
            Assert.AreEqual(12357789728873, sut.Run(data));
        }
    }
}