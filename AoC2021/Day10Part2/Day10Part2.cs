using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace AoC2021.Day10Part2;

public class Day10Part2
{
    private long Run(IList<string> data)
    {
        var matching = new Dictionary<char, char> { {'(', ')'}, {'[', ']'}, {'{', '}'}, {'<', '>'} };
        var points = new Dictionary<char, int> { {')', 1}, {']', 2}, {'}', 3}, {'>', 4} };
        var scores = data.Select(row =>
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
                            return 0;
                        }
                    }
                }

                return unmatched.Reverse().Select(c => points[c]).Aggregate((long)0, (prev, curr) => prev * 5 + curr);
            })
            .Where(s => s != 0)
            .OrderBy(c => c)
            .ToList();
        return scores[scores.Count / 2];
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day10Part2/testdata.txt");
            var sut = new Day10Part2();
            Assert.AreEqual(288957, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day10Part2/data.txt");
            var sut = new Day10Part2();
            Assert.AreEqual(2360030859, sut.Run(data));
        }
    }
}