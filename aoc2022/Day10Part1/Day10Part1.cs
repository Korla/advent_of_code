using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace aoc2022.Day10Part1;

public class Day10Part1
{
    private class Instruction
    {
        public int Completion { get; set; }
        public int Value { get; set; }
    }

    private int Run(IEnumerable<string> data)
    {
        var result = 0;
        var x = 1;
        var salmon = new List<int> {20, 60, 100, 140, 180, 220};
        var instructions = new Queue<Instruction>();
        data = data.Concat(Enumerable.Range(0, 100).Select(_ => "noop"));
        foreach (var (parts, i) in data.Select((row, i) => (row.Split(" "), i + 2)))
        {
            instructions.Enqueue(
                parts.First() == "addx"
                    ? new Instruction {Completion = 2, Value = int.Parse(parts.Last())}
                    : new Instruction {Completion = 1, Value = 0});
            var current = instructions.First();
            current.Completion -= 1;
            if (current.Completion == 0)
            {
                x += current.Value;
                instructions.Dequeue();
            }

            if (salmon.Contains(i))
            {
                result += x * i;
            }
        }

        return result;
    }

    private class Day10Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day10Part1/testdata.txt");
            var sut = new Day10Part1();
            Assert.AreEqual(13140, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day10Part1/data.txt");
            var sut = new Day10Part1();
            Assert.AreEqual(13680, sut.Run(data));
        }
    }
}