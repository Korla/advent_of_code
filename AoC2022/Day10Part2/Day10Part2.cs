using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day10Part2;

public class Day10Part2
{
    private class Instruction
    {
        public int Completion { get; set; }
        public int Value { get; set; }
    }

    private int Run(IEnumerable<string> data)
    {
        var x = 1;
        var instructions = new Queue<Instruction>();
        data = data.Concat(Enumerable.Range(0, 100).Select(_ => "noop"));
        var currentCrtRow = "";
        foreach (var (row, i) in data.Select((row, i) => (row, i + 1)))
        {
            if ((i - 1) % 40 == 0)
            {
                LogCurrentRow();
                currentCrtRow = "";
            }
            var parts = row.Split(" ");
            instructions.Enqueue(
                parts.First() == "addx"
                    ? new Instruction {Completion = 2, Value = int.Parse(parts.Last())}
                    : new Instruction {Completion = 1, Value = 0});
            var current = instructions.First();

            var pixelToDraw = (i - 1) % 40;

            var spritePosition = x % 40;
            currentCrtRow += Math.Abs(pixelToDraw - spritePosition) > 1 ? " " : "#";

            current.Completion -= 1;
            if (current.Completion == 0)
            {
                x += current.Value;
                instructions.Dequeue();
            }
        }
        LogCurrentRow();
        
        void LogStartPosition(int i) =>
            Console.WriteLine($"Start cycle   {i}: begin executing addx {instructions.First().Value}");

        void LogDuring(int pixelToDraw, int i) =>
            Console.WriteLine($"During cycle  {i}: CRT draws pixel in position {pixelToDraw}");

        void LogCurrentRow() =>
            Console.WriteLine($"Current CRT row: {currentCrtRow}");

        void LogSpritePosition(int spritePosition) =>
            Console.WriteLine($"Sprite position: {string.Join("", Enumerable.Range(0, 40).Select(rowPos => Math.Abs(rowPos - spritePosition) > 1 ? "." : "#"))}");

        void LogXRegister() =>
            Console.WriteLine($"Register X is now {x}");

        return -1;
    }

    private class Day10Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day10Part2/testdata.txt");
            var sut = new Day10Part2();
            Assert.AreEqual(-1, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day10Part2/data.txt");
            var sut = new Day10Part2();
            Assert.AreEqual(-1, sut.Run(data));
        }
    }
}