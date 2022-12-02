using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2021.Day10Part1;

public class Day10Part1
{
    private int Run(IList<string> data)
    {
        var matching = new Dictionary<char, char> { {'(', ')'}, {'[', ']'}, {'{', '}'}, {'<', '>'} };
        var points = new Dictionary<char, int> { {')', 3}, {']', 57}, {'}', 1197}, {'>', 25137} };
        return data.Sum(row =>
        {
            var unmatched = "";
            foreach (var c in row)
            {
                if (matching.TryGetValue(c, out var end))
                {
                    unmatched += end;
                }
                else
                {
                    if (unmatched.EndsWith(c))
                    {
                        unmatched = unmatched.Remove(unmatched.Length - 1);
                    }
                    else
                    {
                        return points[c];
                    }
                }
            }

            return 0;
        });
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day10Part1/testdata.txt");
            var sut = new Day10Part1();
            Assert.AreEqual(26397, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day10Part1/data.txt");
            var sut = new Day10Part1();
            Assert.AreEqual(321237, sut.Run(data));
        }
    }
}